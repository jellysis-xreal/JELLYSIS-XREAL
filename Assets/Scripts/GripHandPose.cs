using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class GripHandPose : MonoBehaviour
{
    public HandData leftHandPose;
    public HandData rightHandPose;
    private Vector3 _startingHandPosition;
    private Vector3 _finalHandPosition;
    private Quaternion _startingHandRotation;
    private Quaternion _finalHandRotation;
    private Quaternion[] _startingFingerRotations;
    private Quaternion[] _finalFingerRotations;
    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(SetUpPose);
        grabInteractable.selectExited.AddListener(UnSetPose);

        leftHandPose.gameObject.SetActive(false);
        rightHandPose.gameObject.SetActive(false);
    }

    public void SetUpPose(BaseInteractionEventArgs arg)
    {
        if (arg.interactorObject is XRDirectInteractor)
        {
            HandData handData = arg.interactorObject.transform.parent.GetComponentInChildren<HandData>();
            handData.animator.enabled = false;

            if (handData.handModelType == HandData.HandModelType.Left)
            {
                SetHandDataValues(handData, leftHandPose);    
            }
            else
            {
                SetHandDataValues(handData, rightHandPose);
            }

            SetHandData(handData, _finalHandPosition, _finalHandRotation, _finalFingerRotations);
        }
    }

    public void UnSetPose(BaseInteractionEventArgs arg)
    {
        if (arg.interactorObject is XRDirectInteractor)
        {
            HandData handData = arg.interactorObject.transform.root.GetComponentInChildren<HandData>();
            handData.animator.enabled = true;
            
            SetHandData(handData, _startingHandPosition, _startingHandRotation, _startingFingerRotations);
        }        
    }
    public void SetHandDataValues(HandData h1, HandData h2)
    {
        _startingHandPosition = new Vector3(h1.root.localPosition.x / h1.root.localScale.x,
            h1.root.localPosition.y / h1.root.localScale.y, h1.root.localPosition.z / h1.root.localScale.z);
        _finalHandPosition = new Vector3(h2.root.localPosition.x / h2.root.localScale.x,
            h2.root.localPosition.y / h2.root.localScale.y, h2.root.localPosition.z / h2.root.localScale.z);
        
        //_startingHandPosition = h1.root.localPosition;
        //_finalHandPosition = h2.root.localPosition;

        _startingHandRotation = h1.root.localRotation;
        _finalHandRotation = h2.root.localRotation;

        _startingFingerRotations = new Quaternion[h1.fingerBones.Length];
        _finalFingerRotations = new Quaternion[h1.fingerBones.Length];

        for (int i = 0; i < h1.fingerBones.Length; i++)
        {
            _startingFingerRotations[i] = h1.fingerBones[i].localRotation;
            _finalFingerRotations[i] = h2.fingerBones[i].localRotation;
        }
    }

    public void SetHandData(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation)
    {
        h.root.localPosition = newPosition;
        h.root.localRotation = newRotation;
        for (int i = 0; i < newBonesRotation.Length; i++)
        {
            h.fingerBones[i].localRotation = newBonesRotation[i];
        }
    }
}
