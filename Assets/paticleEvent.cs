using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using Unity.Netcode;

public class paticleEvent : NetworkBehaviour
{
    private XRPushButton _xrPushButton;
    public NetworkObject button_NO;
    // public NetworkVariable<bool> angry_isPressed = new NetworkVariable<bool> (false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    // public NetworkVariable<bool> cheer_isPressed = new NetworkVariable<bool> (false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    // public NetworkVariable<bool> cool_isPressed = new NetworkVariable<bool> (false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    // public ParticleSystem particle_angry;
    // public ParticleSystem particle_cheer;
    // public ParticleSystem particle_cool;

    public NetworkVariable<bool> isPressed = new NetworkVariable<bool> (false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public ParticleSystem MyParticle;
    public ParticleSystem OtherParticle;

    public bool coroutineCheck = false;

    private void Start()
    {
        _xrPushButton = GetComponent<XRPushButton>();
        MyParticle = GetComponent<ParticleSystem>();
        OtherParticle = GetComponent<ParticleSystem>();
        _xrPushButton.onPress.AddListener(PlayParticle);
    }

    // network variable 변화한 값 업데이트
    public override void OnNetworkSpawn()
    {
        isPressed.OnValueChanged += OnPressedValueChanged;
    }

    public void PlayParticle()
    {
        // request ownership first
        // particle.Play(); // for test
        PlayParticleRequest(true);
        // RequestOwnership(button_NO);
        // isPressed.Value = true;
        // particle.Play();
    }
    
    // Activation update
    private void OnPressedValueChanged(bool previousValue, bool newValue)
    {
        if (newValue) 
        {
            MyParticle.Play();
            OtherParticle.Play();
            Debug.Log("[TEST] ======= play particle =======");
            if (IsOwner) isPressed.Value = false;
        } else {}
        // PlayParticleRequest(true);
    }


    public void RequestOwnership(NetworkObject networkObjectSelected)
    {
        if (IsServer) {
            Debug.Log("[TEST] RequestOwnership. This is Server");
        }
        //else if ((IsClient && !IsOwner))
        else if ((!IsOwner))
        {
            ulong PlayerClientID = NetworkManager.Singleton.LocalClientId; // LocalId;
            NetworkObject localPlayer = NetworkManager.LocalClient.PlayerObject;
            Debug.Log("[TEST] RequestOwnership. ID: " + PlayerClientID+ " Client grabbed the "+ networkObjectSelected);
            if (networkObjectSelected != null)
                Debug.Log("[TEST] RequestOwnership ... in progress" );
                localPlayer.GetComponent<NetworkPlayerRpcCall>().RequestGrabbableOwnershipServerRpc(PlayerClientID, networkObjectSelected); // 이거 고쳐야하나...
        } else {
            Debug.Log("[TEST] RequestOwnership. Error Rq Ownership ---- ");
        }
    }

    // Wait for owner sync ...
    public void PlayParticleRequest(bool _isPressed)
    {
        RequestOwnership(button_NO);
        StartCoroutine(PlayParticle_WaitForIt(_isPressed));
    }


    public IEnumerator PlayParticle_WaitForIt(bool _isPressed)
    {
        while(!coroutineCheck) {
            Debug.Log("[TEST] Start Coroutine ...");
            yield return new WaitForSecondsRealtime(0.2f);
            coroutineCheck = true;
        }
        Debug.Log("[TEST] Complete Coroutine!");

        if (IsServer)
        {
            Debug.Log("[TEST] " + MyParticle + " button pressed" + _isPressed);
            isPressed.Value = true;
        }
        else
        {
            Debug.Log("[TEST] This player doesn't have ownership!");
        }
        coroutineCheck = false;
    }

}
