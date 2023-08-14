using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace UnityEngine.XR.Content.Interaction
{
    /// <summary>
    /// An interactable lever that snaps into an on or off position by a direct interactor
    /// </summary>
    public class XRLever : XRBaseInteractable
    {
        const float k_LeverDeadZone = 0.1f; // Prevents rapid switching between on and off states when right in the middle

        [SerializeField]
        [Tooltip("The object that is visually grabbed and manipulated")]
        Transform m_Handle = null;

        [SerializeField]
        [Tooltip("The value of the lever")]
        bool m_Value = false;

        [SerializeField]
        [Tooltip("If enabled, the lever will snap to the value position when released")]
        bool m_LockToValue;

        [SerializeField]
        [Tooltip("Angle of the lever in the 'on' position")]
        [Range(-90.0f, 90.0f)]
        float m_MaxAngle = 90.0f;

        [SerializeField]
        [Tooltip("Angle of the lever in the 'off' position")]
        [Range(-90.0f, 90.0f)]
        float m_MinAngle = -90.0f;

        [SerializeField]
        [Tooltip("Events to trigger when the lever activates")]
        UnityEvent m_OnLeverActivate = new UnityEvent();

        [SerializeField]
        [Tooltip("Events to trigger when the lever deactivates")]
        UnityEvent m_OnLeverDeactivate = new UnityEvent();

        [SerializeField]
        [Tooltip("Speed at which the lever rotates when not locked to min/max angles")]
        float rotationSpeed = 45.0f;

        [SerializeField]
        [Tooltip("The pivot point around which the lever rotates")]
        Transform pivotTransform;

        private float currentAngle = 0.0f;

        IXRSelectInteractor m_Interactor;
        float startGrabAngle = 0.0f;

        /// <summary>
        /// The object that is visually grabbed and manipulated
        /// </summary>
        public Transform handle
        {
            get => m_Handle;
            set => m_Handle = value;
        }

        /// <summary>
        /// The value of the lever
        /// </summary>
        public bool value
        {
            get => m_Value;
            set => SetValue(value, true);
        }

        /// <summary>
        /// If enabled, the lever will snap to the value position when released
        /// </summary>
        public bool lockToValue { get; set; }

        /// <summary>
        /// Angle of the lever in the 'on' position
        /// </summary>
        public float maxAngle
        {
            get => m_MaxAngle;
            set => m_MaxAngle = value;
        }

        /// <summary>
        /// Angle of the lever in the 'off' position
        /// </summary>
        public float minAngle
        {
            get => m_MinAngle;
            set => m_MinAngle = value;
        }

        /// <summary>
        /// Events to trigger when the lever activates
        /// </summary>
        public UnityEvent onLeverActivate => m_OnLeverActivate;

        /// <summary>
        /// Events to trigger when the lever deactivates
        /// </summary>
        public UnityEvent onLeverDeactivate => m_OnLeverDeactivate;

        void Start()
        {
            SetValue(m_Value, true);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            selectEntered.AddListener(StartGrab);
            selectExited.AddListener(EndGrab);
        }

        protected override void OnDisable()
        {
            selectEntered.RemoveListener(StartGrab);
            selectExited.RemoveListener(EndGrab);
            base.OnDisable();
        }

        void StartGrab(SelectEnterEventArgs args)
        {
            m_Interactor = args.interactorObject;
            startGrabAngle = GetCurrentAngle(); // 레버를 처음 그랩했을 때의 각도를 저장
        }

        void EndGrab(SelectExitEventArgs args)
        {
            var lookDirection = GetLookDirection();
            var lookAngle = Mathf.Atan2(lookDirection.z, lookDirection.y) * Mathf.Rad2Deg;

            // 초기 그랩 각도를 빼줌으로써 변경 없이 회전
            lookAngle += Time.deltaTime * rotationSpeed - startGrabAngle;

            SetHandleAngle(lookAngle);
        }

        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            base.ProcessInteractable(updatePhase);

            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            {
                if (isSelected)
                {
                    UpdateValue();
                }
            }
        }

        Vector3 GetLookDirection()
        {
            Vector3 direction = m_Interactor.GetAttachTransform(this).position - m_Handle.position;
            direction = transform.InverseTransformDirection(direction);
            direction.x = 0;

            return direction.normalized;
        }

        void UpdateValue()
        {
            var lookDirection = GetLookDirection();
            var lookAngle = Mathf.Atan2(lookDirection.z, lookDirection.y) * Mathf.Rad2Deg;

            lookAngle += Time.deltaTime * rotationSpeed;

            SetHandleAngle(lookAngle);
        }

        void SetValue(bool isOn, bool forceRotation = false)
        {
            if (m_Value == isOn)
            {
                if (forceRotation)
                    SetHandleAngle(m_Value ? m_MaxAngle : m_MinAngle);

                return;
            }

            m_Value = isOn;

            if (m_Value)
            {
                m_OnLeverActivate.Invoke();
            }
            else
            {
                m_OnLeverDeactivate.Invoke();
            }

            if (!isSelected && (m_LockToValue || forceRotation))
                SetHandleAngle(m_Value ? m_MaxAngle : m_MinAngle);
        }

        void SetHandleAngle(float angle)
        {
            if (m_Handle != null)
                m_Handle.localRotation = Quaternion.Euler(angle, 0.0f, 0.0f);
        }

        void OnDrawGizmosSelected()
        {
            var angleStartPoint = transform.position;

            if (m_Handle != null)
                angleStartPoint = m_Handle.position;

            const float k_AngleLength = 0.25f;

            var angleMaxPoint = angleStartPoint + transform.TransformDirection(Quaternion.Euler(m_MaxAngle, 0.0f, 0.0f) * Vector3.up) * k_AngleLength;
            var angleMinPoint = angleStartPoint + transform.TransformDirection(Quaternion.Euler(m_MinAngle, 0.0f, 0.0f) * Vector3.up) * k_AngleLength;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(angleStartPoint, angleMaxPoint);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(angleStartPoint, angleMinPoint);
        }

        void OnValidate()
        {
            SetHandleAngle(m_Value ? m_MaxAngle : m_MinAngle);
        }



        float GetCurrentAngle()
        {
            if (m_Handle != null)
            {
                var localRotation = m_Handle.localRotation.eulerAngles;
                return localRotation.x;
            }

            return 0.0f;
        }

        void SetHandleAngleAroundPivot(float angle)
        {
            if (m_Handle != null && pivotTransform != null)
            {
                Vector3 toPivot = m_Handle.position - pivotTransform.position;
                Quaternion rotation = Quaternion.Euler(angle, 0.0f, 0.0f);
                Vector3 rotatedPosition = pivotTransform.position + rotation * toPivot;
                m_Handle.position = rotatedPosition;
            }
        }
    }
}