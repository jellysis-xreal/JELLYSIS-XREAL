using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class XRkonbRotation : MonoBehaviour
{
    /*
    public XRKnob xrKnob; // Reference to the XRKnob script on the object

    private int rotationCount = 0;
    private const int requiredRotations = 8;

    private void Start()
    {
        if (xrKnob == null)
        {
            Debug.LogError("XRKnob reference is not set.");
            enabled = false; // Disable this script if XRKnob reference is not set
            return;
        }

        xrKnob.onValueChange.AddListener(OnKnobValueChanged);
    }

    private void OnKnobValueChanged(float value)
    {
        if (Mathf.Abs(value - 1.0f) < 0.001f)
        {
            rotationCount++;
            if (rotationCount >= requiredRotations)
            {
                Debug.Log("액체 시작");
                rotationCount = 0;
            }
        }
    }
    */
}
