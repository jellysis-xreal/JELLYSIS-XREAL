using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkSyncObject : NetworkBehaviour
{
    NetworkObject localPlayer;
    ulong PlayerClientID;

    // activation control 할 gameObject
    [SerializeField]
    public GameObject gameObjectToSetActive;

    // activation 초기화
    [SerializeField]
    private NetworkVariable<bool> isActive = new NetworkVariable<bool> (true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private bool coroutineCheck = false;

    public void Start() {
        if (gameObjectToSetActive != null)
        {
            gameObjectToSetActive.SetActive(isActive.Value);
        }
        localPlayer = NetworkManager.LocalClient.PlayerObject;
        PlayerClientID = NetworkManager.Singleton.LocalClientId;
    }

    // network variable 변화한 값 업데이트
    public override void OnNetworkSpawn()
    {
        isActive.OnValueChanged += OnActiveValueChanged;
    }

    private void OnActiveValueChanged(bool previousValue, bool newValue)
    {
        gameObjectToSetActive.SetActive(isActive.Value);
        Debug.Log("[TEST] ID: " + gameObjectToSetActive + " IsActive: " + gameObjectToSetActive.activeInHierarchy);
    }

    // Wait for owner sync ...
    public void SetActiveNetworkObject(bool _isActive)
    {
        StartCoroutine(SetActive_WaitForIt(_isActive));
    }


    public IEnumerator SetActive_WaitForIt(bool _isActive)
    {
        while(!coroutineCheck) {
            Debug.Log("[TEST] Start Coroutine ...");
            yield return new WaitForSecondsRealtime(0.5f);
            coroutineCheck = true;
        }
        Debug.Log("[TEST] Complete Coroutine!");

        if (IsOwner)
        {
            Debug.Log("[TEST] " + gameObjectToSetActive + " SetActiveNetworkObject isActive: " + _isActive);
            isActive.Value = _isActive;
        }
        else
        {
            Debug.Log("[TEST] This player doesn't have ownership!");
        }
        coroutineCheck = false;
    }

    // Event Funcs: Request Ownership
    // [ServerRpc]
    public void RequestOwnership(NetworkObject networkObjectSelected)
    {
        if (IsServer) {
            Debug.Log("[TEST] This is Server");
        }
        else if ((IsClient && !IsOwner))
        {
            Debug.Log("[TEST] ID: " + PlayerClientID+ " Client grabbed the "+ networkObjectSelected);
            if (networkObjectSelected != null)
                localPlayer.GetComponent<NetworkPlayerRpcCall>().RequestGrabbableOwnershipServerRpc(PlayerClientID, networkObjectSelected); // 이거 고쳐야하나...
        }
    }

    // [ServerRpc]
    public void RequestRemoveOwnership(NetworkObject networkObjectSelected) {
        if ((IsClient && IsOwner))
        {
            Debug.Log("[TEST] ID: " + PlayerClientID+ " Client dropped the "+ networkObjectSelected);
            if (networkObjectSelected != null)
                localPlayer.GetComponent<NetworkPlayerRpcCall>().RequestRetrunGrabbableOwnershipServerRpc(PlayerClientID, networkObjectSelected);
        }
    }

    public void OnSelectGrabbable(SelectEnterEventArgs eventArgs)
    {
        // localPlayer = NetworkManager.LocalClient.PlayerObject;
        // PlayerClientID = NetworkManager.Singleton.LocalClientId;

        // Debug.Log("[TEST] local player object is " + localPlayer + " and ID: " + PlayerClientID);
        Debug.Log("[TEST] Grabbed!!");

        // Debug.Log("[TEST] IsClient: "+ IsClient + " IsOwner: " + IsOwner);
        NetworkObject networkObjectSelected = eventArgs.interactableObject.transform.GetComponent<NetworkObject>();

        RequestOwnership(networkObjectSelected);

        // if (IsServer) {
        //     Debug.Log("[TEST] This is Server");
        // }
        // else if ((IsClient && !IsOwner))
        // {
        //     Debug.Log("[TEST] ID: " + PlayerClientID+ " Client grabbed the "+ networkObjectSelected);
        //     if (networkObjectSelected != null)
        //         localPlayer.GetComponent<NetworkPlayerRpcCall>().RequestGrabbableOwnershipServerRpc(PlayerClientID, networkObjectSelected); // 이거 고쳐야하나...
        // }
    }

    public void OnSelectExitGrabbable(SelectExitEventArgs eventArgs)
    {
        localPlayer = NetworkManager.LocalClient.PlayerObject;
        PlayerClientID = NetworkManager.Singleton.LocalClientId;
        Debug.Log("[TEST] Release!!");
        NetworkObject networkObjectSelected = eventArgs.interactableObject.transform.GetComponent<NetworkObject>();

        RequestRemoveOwnership(networkObjectSelected);
        // if ((IsClient && IsOwner))
        // {
        //     Debug.Log("[TEST] ID: " + PlayerClientID+ " Client dropped the "+ networkObjectSelected);
        //     if (networkObjectSelected != null)
        //         localPlayer.GetComponent<NetworkPlayerRpcCall>().RequestRetrunGrabbableOwnershipServerRpc(PlayerClientID, networkObjectSelected);
        // }
    }
}
