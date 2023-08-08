using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class KnifeHapticTest : MonoBehaviour
{
    public XRBaseController leftController, rightController;
    public float defaultAmplitude = 0.2f;
    public float defaultDuration = 0.5f;

    [ContextMenu("Send Haptics")]
    public void SendHaptics()
    {
        leftController.SendHapticImpulse(defaultAmplitude, defaultDuration);
        rightController.SendHapticImpulse(defaultAmplitude, defaultDuration);
    }
}
