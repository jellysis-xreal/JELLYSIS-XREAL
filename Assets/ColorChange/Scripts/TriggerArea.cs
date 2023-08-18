using System;
using System.Collections;
using System.Collections.Generic;
using EnumTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Content.Interaction.ColorChanger;
using UnityEngine.XR.Interaction.Toolkit;
using static PourDetector;

namespace ColorChanger
{
    public class TriggerArea : XRBaseInteractor
    {
        private XRBaseInteractable currentInteractable = null;
        [SerializeField]private List<XRBaseInteractable> interactablesInsideTrigger = new List<XRBaseInteractable>();

        private bool redCollided = false;
        private bool blueCollided = false;
        private bool yellowCollided = false;
        private bool whiteCollided = false;

        public Material bearMaterial;
        public Material liquidMaterial;
        public MeshRenderer meshRenderer;
        public GameObject streamPrefabColor;
        [SerializeField] private bool isPrefabInstantiated = false;
        public Stream currentStream = null;
        public Transform origin = null;

        public bool pourCheck = false;
        [SerializeField]private bool isPouring = false;

        public static event Action<Color> OnColorDetected;

        public XRKnob xrKnob;
        public Transform leverTransform;
        public PourDetector pourDetector;
        public Color color;

        public PuddingSpawn puddingSpawn1;
        public PuddingSpawn puddingSpawn2;
        public PuddingSpawn puddingSpawn3;
        public PuddingSpawn puddingSpawn4;


        private void Start()
        {
            liquidMaterial = meshRenderer.material;
        }

        private void Update()
        {
        }

        private void OnTriggerStay(Collider other)
        {
            
            //Debug.Log("Name is " +other.name+" has interactable" + TryGetInteractable(other, out XRBaseInteractable interactable1));
            SetInteractiable(other);

            if (TryGetInteractable(other, out XRBaseInteractable interactable))
            {
                //Debug.Log(interactable.transform.name);
                if (!interactablesInsideTrigger.Contains(interactable))
                {
                    Debug.Log("addinteractabale");
                    interactablesInsideTrigger.Add(interactable);
                    Debug.Log("Object entered the trigger area: " + interactable.name);
                    //CheckCollision();
                    return;
                }
                if (!isPrefabInstantiated)
                {   
                    CheckCollision();    
                }
            }
        }


        private void SetInteractiable(Collider other)
        {
            if (TryGetInteractable(other, out XRBaseInteractable interactable))
            {
                if (currentInteractable == null)
                    currentInteractable = interactable;
            }
            
        }


        private void OnTriggerExit(Collider other)
        {
            Debug.Log("TriggerExit");
            ClearInteractable(other);
        }

        private void ClearInteractable(Collider other)
        {
            if (TryGetInteractable(other, out XRBaseInteractable interactable))
            {
                if (currentInteractable == interactable)
                    currentInteractable = null;

                if (interactablesInsideTrigger.Contains(interactable))
                {
                    interactablesInsideTrigger.Remove(interactable);
                    Debug.Log("Object exited the trigger area: " + interactable.name);
                    CheckCollision();
                }
            }
        }

        private bool TryGetInteractable(Collider collider, out XRBaseInteractable interactable)
        {
            interactable = interactionManager.GetInteractableForCollider(collider);
            return interactable != null;
        }


        public override void GetValidTargets(List<XRBaseInteractable> validTargets)
        {
            validTargets.Clear();
            validTargets.Add(currentInteractable);
        }

        public override bool CanHover(XRBaseInteractable interactable)
        {
            return base.CanHover(interactable) && currentInteractable == interactable && !interactable.isSelected;
        }

        public override bool CanSelect(XRBaseInteractable interactable)
        {
            return false;
        }

        private void CheckCollision()
        {
            Debug.Log("CheckCollision");
            redCollided = false;
            blueCollided = false;
            yellowCollided = false;
            whiteCollided = false;

            foreach (XRBaseInteractable interactable in interactablesInsideTrigger)
            {
                string tag = interactable.gameObject.tag;


                if (tag == "Red")
                {
                    redCollided = true;
                    //Debug.Log("Object with Tag1 is inside the trigger area.");

                }
                if (tag == "Blue")
                {
                    blueCollided = true;
                    //Debug.Log("Object with Tag2 is inside the trigger area.");

                }
                if (tag == "Yellow")
                {
                    yellowCollided = true;
                    //Debug.Log("Object with Tag3 is inside the trigger area.");

                }
                if (tag == "White")
                {
                    whiteCollided = true;
                    //Debug.Log("Object with Tag2 is inside the trigger area.");
                }

            }
            if (pourCheck) //&& currentStream!= null)
            {

                StartPour();
                // 색 바꾸기 전 알아야 할 것 : 
                switch (redCollided, blueCollided, yellowCollided, whiteCollided)
                {
                    case (true, true, false, false):
                        Debug.Log("Red + Blue = Purple");
                        bearMaterial.color = new Color(1.0f, 0.0f, 1.0f);
                        liquidMaterial.SetColor("_Tint", new Color(1.0f, 0.0f, 1.0f)); // Purple color
                        currentStream.SetLineColor(new Color(1.0f, 0.0f, 1.0f));
                        color = new Color(1.0f, 0.0f, 1.0f);
                        pourDetector.bearColorType = BearColorType.Purple;
                        break;
                    case (true, false, true, false):
                        Debug.Log("Red + Blue = Orange");
                        bearMaterial.color = new Color(1.0f, 0.5f, 0.0f);
                        liquidMaterial.SetColor("_Tint", new Color(1.0f, 0.5f, 0.0f));// Orange color
                        currentStream.SetLineColor(new Color(1.0f, 0.5f, 0.0f));
                        color = new Color(1.0f, 0.5f, 0.0f);
                        pourDetector.bearColorType = BearColorType.Orange;
                        break;
                    case (true, false, false, true):
                        Debug.Log("Red + White = Pink");
                        bearMaterial.color = new Color(1.0f, 0.75f, 0.75f);
                        liquidMaterial.SetColor("_Tint", new Color(1.0f, 0.75f, 0.75f)); // Pink color
                        currentStream.SetLineColor(new Color(1.0f, 0.75f, 0.75f));
                        color = new Color(1.0f, 0.75f, 0.75f);
                        pourDetector.bearColorType = BearColorType.Pink;
                        break;
                    case (false, true, true, false):
                        Debug.Log("Blue + Yellow = Green");
                        bearMaterial.color = new Color(0.0f, 1.0f, 0.0f);
                        liquidMaterial.SetColor("_Tint", new Color(0.0f, 1.0f, 0.0f)); // Green color
                        currentStream.SetLineColor(new Color(0.0f, 1.0f, 0.0f));
                        color = new Color(0.0f, 1.0f, 0.0f);
                        pourDetector.bearColorType = BearColorType.Green;
                        break;
                    case (false, true, false, true):
                        Debug.Log("Blue + White = Skyblue");
                        bearMaterial.color = new Color(0.0f, 1.0f, 1.0f);
                        liquidMaterial.SetColor("_Tint", new Color(0.0f, 1.0f, 1.0f)); // Sky blue color
                        currentStream.SetLineColor(new Color(0.0f, 1.0f, 1.0f));
                        color = new Color(0.0f, 1.0f, 1.0f);
                        pourDetector.bearColorType = BearColorType.PastelBlue;

                        break;
                    case (false, false, true, true):
                        Debug.Log("Yellow + White = Pastel Yellow");
                        bearMaterial.color = new Color(1.0f, 1.0f, 0.0f);
                        liquidMaterial.SetColor("_Tint", new Color(1.0f, 1.0f, 0.0f)); // Pastel Yellow color
                        currentStream.SetLineColor(new Color(1.0f, 1.0f, 0.0f));
                        color = new Color(1.0f, 1.0f, 0.0f);
                        pourDetector.bearColorType = BearColorType.PastelYellow;
                        break;

                    default:
                        switch (redCollided, blueCollided, yellowCollided, whiteCollided)
                        {
                            case (true, false, false, false):
                                Debug.Log("Red");
                                bearMaterial.color = new Color(1.0f, 0.0f, 0.0f);
                                liquidMaterial.SetColor("_Tint", new Color(1,0,0));
                                currentStream.SetLineColor(Color.red);
                                color =Color.red;
                                pourDetector.bearColorType = BearColorType.Red;
                                break;
                            case (false, true, false, false):
                                //Debug.Log("Blue");
                                bearMaterial.color = new Color(0.0f, 0.0f, 1.0f);
                                liquidMaterial.SetColor("_Tint", new Color(0,0,1));
                                currentStream.SetLineColor(Color.blue);
                                color = Color.blue;
                                pourDetector.bearColorType = BearColorType.Blue;
                                break;
                            case (false, false, true, false):
                                Debug.Log("Yellow");
                                bearMaterial.color = new Color(1.0f, 1.0f, 0.0f);
                                liquidMaterial.SetColor("_Tint", new Color(1,1,0));
                                currentStream.SetLineColor(Color.yellow);
                                color = Color.yellow;
                                pourDetector.bearColorType = BearColorType.Yellow;
                                break;
                            case (false, false, false, true):
                                Debug.Log("White");
                                bearMaterial.color = new Color(1.0f, 1.0f, 1.0f);
                                liquidMaterial.SetColor("_Tint", new Color(1,1,1));
                                currentStream.SetLineColor(Color.white);
                                color = Color.white;
                                pourDetector.bearColorType = BearColorType.White;
                                break;
                        }
                        break;
                }
                
            }
        }
        void ResetLeverPosition()
        {
            //leverTransform.localRotation = Quaternion.Euler(0,-180,0);
            xrKnob.value = 1f;
        }
        private Stream CreateStream()
        {
            Debug.Log("CreateSteam!");
            GameObject streamObject = Instantiate(streamPrefabColor, origin.position, Quaternion.identity, transform);
            isPrefabInstantiated = true;
            StartCoroutine(pourDetector.PlusLiquidHeight());
            return streamObject.GetComponent<Stream>();
        }

        private void StartPour()
        {
            //Debug.Log("TryPour, Value : " + xrKnob.value);
            if (!isPouring && (xrKnob.value <=0 || xrKnob.value >=2) && !isPrefabInstantiated) // 여기에서 회전값 일정량 이상 되면 붓도록&&    
            {
                isPouring = true;
                pourCheck = true;

                currentStream = CreateStream();
                currentStream.Begin();
                StartCoroutine(EndPourAfterDelay(5.0f));

                

                /*
                XRKnob xRKnob = new XRKnob();

                if (xRKnob.m_Value >= 15.0f)
                {
                    isPouring = true;
                    pourCheck = true;

                    currentStream = CreateStream();
                    currentStream.Begin();
                    StartCoroutine(EndPourAfterDelay(5.0f));
                }
                */
            }
        }

        private IEnumerator EndPourAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            ResetLeverPosition();
            
            if (currentStream != null)
            {
                currentStream.End();
                //currentStream = null;
            }

            Debug.Log("EndPour routine End");
            isPouring = false;
            isPrefabInstantiated = false;
        }
    }
}
