using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class TriggerArea : XRBaseInteractor
{
    private XRBaseInteractable currentInteractable = null;
    private List<XRBaseInteractable> interactablesInsideTrigger = new List<XRBaseInteractable>();

    private bool redCollided = false;
    private bool blueCollided = false;
    private bool yellowCollided = false;
    private bool whiteCollided = false;

    public Material liquidMaterial;

    private void OnTriggerStay(Collider other)
    {
        SetInteractiable(other);

        if (TryGetInteractable(other, out XRBaseInteractable interactable))
        {
            if (!interactablesInsideTrigger.Contains(interactable))
            {
                interactablesInsideTrigger.Add(interactable);
                Debug.Log("Object entered the trigger area: " + interactable.name);
                CheckCollision();
            }
        }
        
    }


    private void SetInteractiable(Collider other) 
    {
        if(TryGetInteractable(other, out XRBaseInteractable interactable))
        {
            if(currentInteractable == null)
                currentInteractable= interactable;
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
                Debug.Log("Object with Tag2 is inside the trigger area.");
                
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

        switch (redCollided, blueCollided, yellowCollided, whiteCollided)
        {
            case (true, true, false, false):
                Debug.Log("Red + Blue = Purple");
                liquidMaterial.SetColor("_Tint", new Color(1.0f, 0.0f, 1.0f)); // Purple color
                break;
            case (true, false, true, false):
                Debug.Log("Red + Blue = Orange");
                liquidMaterial.SetColor("_Tint", new Color(1.0f, 0.5f, 0.0f)); // Orange color
                break;
            case (true, false, false, true):
                Debug.Log("Red + White = Pink");
                liquidMaterial.SetColor("_Tint", new Color(1.0f, 0.75f, 0.75f)); // Pink color
                break;
            case (false, true, true, false):
                Debug.Log("Blue + Yellow = Green");
                liquidMaterial.SetColor("_Tint", new Color(0.0f, 1.0f, 0.0f)); // Green color
                break;
            case (false, true, false, true):
                Debug.Log("Blue + White = Skyblue");
                liquidMaterial.SetColor("_Tint", new Color(0.0f, 1.0f, 1.0f)); // Sky blue color
                break;
            case (false, false, true, true):
                Debug.Log("Yellow + White = Pastel Green");
                liquidMaterial.SetColor("_Tint", new Color(1.0f, 1.0f, 0.0f)); // Pastel green color
                break;
            default:
                switch (redCollided, blueCollided, yellowCollided, whiteCollided)
                {
                    case (true, false, false, false):
                        Debug.Log("Red");
                        liquidMaterial.SetColor("_Tint", Color.red);
                        break;
                    case (false, true, false, false):
                        Debug.Log("Blue");
                        liquidMaterial.SetColor("_Tint", Color.blue); ;
                        break;
                    case (false, false, true, false):
                        Debug.Log("Yellow");
                        liquidMaterial.SetColor("_Tint", Color.yellow);
                        break;
                    case (false, false, false, true):
                        Debug.Log("White");
                        liquidMaterial.SetColor("_Tint", Color.white);
                        break;
                }
                break;
        }


    }
    
}
