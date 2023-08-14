using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InjectionPusher : MonoBehaviour
{
    public CreamMaker creamMaker;
    
    public Transform visualTarget;
    public Vector3 localAxis;
    public float resetSpeed = 5f;
    public float followAngleTreshold = 45;
    
    private bool _freeze = false;
    
    private Vector3 _initialLocalPos;
    
    [SerializeField] private XRBaseInteractable interactable;
    private Transform _pokeAttachTransform;
    private Vector3 _offset;
    [SerializeField] private bool isFollowing=false;
    void Start()
    {
        _initialLocalPos = visualTarget.localPosition;
        
        interactable = GetComponent<XRBaseInteractable>();
        interactable.hoverEntered.AddListener(Follow);
        interactable.hoverExited.AddListener(Reset);
        interactable.selectEntered.AddListener(Freeze);
    }

    public void Follow(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            creamMaker.isPressing = true;
            
            XRPokeInteractor interactor = (XRPokeInteractor)hover.interactorObject;
            isFollowing = true;
            _freeze = false;
            
            _pokeAttachTransform = interactor.attachTransform;
            _offset = visualTarget.position - _pokeAttachTransform.position;

            float pokeAngle = Vector3.Angle(_offset, visualTarget.TransformDirection(localAxis));

            if (pokeAngle > followAngleTreshold)
            {
                isFollowing = false;
                _freeze = true;
            }
        }
    }

    public void Reset(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            creamMaker.isPressing = false;
            
            isFollowing = false;
            _freeze = false;
        }
    }

    public void Freeze(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            _freeze = true;
        }   
    }
    void Update()
    {
        if (_freeze) return;
        
        if (isFollowing)
        {
            Vector3 localTargetPosition = visualTarget.InverseTransformPoint(_pokeAttachTransform.position + _offset);
            Vector3 constrainedLocalTargetPosition = Vector3.Project(localTargetPosition, localAxis);

            visualTarget.position = visualTarget.TransformPoint(constrainedLocalTargetPosition);
        }
        else
        {
            visualTarget.localPosition = Vector3.Lerp(visualTarget.localPosition, _initialLocalPos, Time.deltaTime * resetSpeed);
        }
    }

    // Trigger 말고 Hover Enter 되면 생성 코루틴 돌림
}
