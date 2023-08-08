using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class AnimateHandOnInput : MonoBehaviour
{
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty grabAnimationAction;

    public Animator handAnimator;
    private void Update()
    {
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger", triggerValue);
        
        float grabValue = grabAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Grip", grabValue);
    }
}
