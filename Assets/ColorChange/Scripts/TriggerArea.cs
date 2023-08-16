using System;
using System.Collections;
using System.Collections.Generic;
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

        public Material liquidMaterial;
        public GameObject streamPrefabColor;
        private bool isPrefabInstantiated = false;
        public Stream currentStream = null;
        public Transform origin = null;

        public bool pourCheck = false;
        [SerializeField]private bool isPouring = false;

        public static event Action<Color> OnColorDetected;

        public XRKnob xrKnob;

        private void OnTriggerStay(Collider other)
        {
            SetInteractiable(other);

            if (TryGetInteractable(other, out XRBaseInteractable interactable))
            {
                if (!interactablesInsideTrigger.Contains(interactable))
                {
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
                    Debug.Log("Object with Tag1 is inside the trigger area.");

                }
                if (tag == "Blue")
                {
                    blueCollided = true;
                    //Debug.Log("Object with Tag2 is inside the trigger area.");

                }
                if (tag == "Yellow")
                {
                    yellowCollided = true;
                    Debug.Log("Object with Tag3 is inside the trigger area.");

                }
                if (tag == "White")
                {
                    whiteCollided = true;
                    Debug.Log("Object with Tag2 is inside the trigger area.");
                }

            }
            if (pourCheck)
            {

                StartPour();

                switch (redCollided, blueCollided, yellowCollided, whiteCollided)
                {
                    case (true, true, false, false):
                        Debug.Log("Red + Blue = Purple");
                        liquidMaterial.SetColor("_Tint", new Color(1.0f, 0.0f, 1.0f)); // Purple color
                        currentStream.SetLineColor(new Color(1.0f, 0.0f, 1.0f));
                        break;
                    case (true, false, true, false):
                        Debug.Log("Red + Blue = Orange");
                        liquidMaterial.SetColor("_Tint", new Color(1.0f, 0.5f, 0.0f));// Orange color
                        currentStream.SetLineColor(new Color(1.0f, 0.5f, 0.0f));
                        break;
                    case (true, false, false, true):
                        Debug.Log("Red + White = Pink");
                        liquidMaterial.SetColor("_Tint", new Color(1.0f, 0.75f, 0.75f)); // Pink color
                        currentStream.SetLineColor(new Color(1.0f, 0.75f, 0.75f));
                        break;
                    case (false, true, true, false):
                        Debug.Log("Blue + Yellow = Green");
                        liquidMaterial.SetColor("_Tint", new Color(0.0f, 1.0f, 0.0f)); // Green color
                        currentStream.SetLineColor(new Color(0.0f, 1.0f, 0.0f));
                        break;
                    case (false, true, false, true):
                        Debug.Log("Blue + White = Skyblue");
                        liquidMaterial.SetColor("_Tint", new Color(0.0f, 1.0f, 1.0f)); // Sky blue color
                        currentStream.SetLineColor(new Color(0.0f, 1.0f, 1.0f));
                        break;
                    case (false, false, true, true):
                        Debug.Log("Yellow + White = Pastel Green");
                        liquidMaterial.SetColor("_Tint", new Color(1.0f, 1.0f, 0.0f)); // Pastel green color
                        currentStream.SetLineColor(new Color(1.0f, 1.0f, 0.0f));
                        break;

                    default:
                        switch (redCollided, blueCollided, yellowCollided, whiteCollided)
                        {
                            case (true, false, false, false):
                                Debug.Log("Red");
                                liquidMaterial.SetColor("_Tint", Color.red);
                                currentStream.SetLineColor(Color.red);
                                break;
                            case (false, true, false, false):
                                //Debug.Log("Blue");
                                liquidMaterial.SetColor("_Tint", Color.blue);
                                currentStream.SetLineColor(Color.blue);
                                break;
                            case (false, false, true, false):
                                Debug.Log("Yellow");
                                liquidMaterial.SetColor("_Tint", Color.yellow);
                                currentStream.SetLineColor(Color.yellow);
                                break;
                            case (false, false, false, true):
                                Debug.Log("White");
                                liquidMaterial.SetColor("_Tint", Color.white);
                                currentStream.SetLineColor(Color.white);
                                break;
                        }
                        break;
                }


            }


        }
        void ResetLeverPosition()
        {
            // 새로 생성, 기존에 있던 거 삭제됨.
        }
        private Stream CreateStream()
        {
            Debug.Log("CreateSteam!");
            GameObject streamObject = Instantiate(streamPrefabColor, origin.position, Quaternion.identity, transform);
            
            return streamObject.GetComponent<Stream>();
        }

        private void StartPour()
        {
            Debug.Log("TryPour, Value : " + xrKnob.value);
            if (!isPouring && (xrKnob.value <=0 || xrKnob.value >=2) && !isPrefabInstantiated) // 여기에서 회전값 일정량 이상 되면 붓도록&&    
            {
                isPouring = true;
                pourCheck = true;

                currentStream = CreateStream();
                currentStream.Begin();
                StartCoroutine(EndPourAfterDelay(5.0f));
                isPrefabInstantiated = true;

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

            if (currentStream != null)
            {
                currentStream.End();
                currentStream = null;
            }

            isPouring = false;
        }
    }
}
