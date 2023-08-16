using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class paticleEvent : MonoBehaviour
{
    private XRPushButton _xrPushButton;
    public bool isPressed = false;
    public ParticleSystem particle;
    private void Start()
    {
        _xrPushButton = GetComponent<XRPushButton>();
        particle = GetComponent<ParticleSystem>();
        _xrPushButton.onPress.AddListener(PlayParticle);
    }

    void PlayParticle()
    {
        isPressed = true;
        particle.Play();
        
    }
}
