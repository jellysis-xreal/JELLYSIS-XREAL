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

    // rigidBody 초기화
    [SerializeField]
    private Rigidbody rigidBody;

    // activation 초기화
    [SerializeField]
    public NetworkVariable<bool> isActive = new NetworkVariable<bool> (true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    // use gravity & is kinematic 초기화
    [SerializeField]
    public NetworkVariable<bool> useGravity = new NetworkVariable<bool> (true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private bool coroutineCheck = false;

    public void Start() {
        if (gameObjectToSetActive != null)
        {
            gameObjectToSetActive.SetActive(isActive.Value);
        }
        if (rigidBody != null)
        {
            rigidBody.useGravity = useGravity.Value;
            rigidBody.isKinematic = !useGravity.Value;
        }
        localPlayer = NetworkManager.LocalClient.PlayerObject;
        PlayerClientID = NetworkManager.Singleton.LocalClientId; //LocalId;
        Debug.Log("[TEST] Start player Id: " + PlayerClientID);
    }

    private void printInfo() {
        Debug.Log("[TEST] IsClient, IsServer, IsOwner: " + IsClient + ", " + IsServer + ", " + IsOwner);
    }
    

    // network variable 변화한 값 업데이트
    public override void OnNetworkSpawn()
    {
        isActive.OnValueChanged += OnActiveValueChanged;
        useGravity.OnValueChanged += UseGravityValueChanged;
    }

    // Gravity update
    private void UseGravityValueChanged(bool previousValue, bool newValue)
    {
        Debug.Log("[TEST] ID: " + rigidBody + " before, after: " + previousValue + ", "+ newValue);
        rigidBody.useGravity = newValue;
        rigidBody.isKinematic = !newValue;
        Debug.Log("[TEST] ID: " + rigidBody + " useGravity: " + rigidBody.useGravity);
    }
    
    // private void Update() {
    //     if (rigidBody != null)
    //     {
    //         rigidBody.useGravity = useGravity.Value;
    //         rigidBody.isKinematic = !useGravity.Value;
    //     }
    // }

    // Wait for owner sync ...
    public void UseGravityNetworkObject(bool _useGravity)
    {
        StartCoroutine(UseGravity_WaitForIt(_useGravity));
    }

    public IEnumerator UseGravity_WaitForIt(bool _useGravity)
    {
        if (_useGravity != useGravity.Value) {
            Debug.Log("[TEST] UseGravity_WaitForIt. _useGravity, useGravity.Value, rigidBody.useGravity: " + _useGravity + ", " + useGravity.Value + ", " + rigidBody.useGravity);

            while(!coroutineCheck) {
                Debug.Log("[TEST] UseGravity_WaitForIt. Start Coroutine ...");
                yield return new WaitForSecondsRealtime(0.2f);
                coroutineCheck = true;
            }
            Debug.Log("[TEST] UseGravity_WaitForIt. Complete Coroutine!");

            if (IsOwner)
            {
                // Debug.Log("[TEST] " + rigidBody + " set useGravity before: " + rigidBody.useGravity + " after: " + _useGravity);
                useGravity.Value = _useGravity;
                Debug.Log("[TEST] UseGravity_WaitForIt. This player does have ownership! before, after: " + rigidBody.useGravity + " " + _useGravity);
            }
            else
            {
                Debug.Log("[TEST] UseGravity_WaitForIt. This player doesn't have ownership!");
            }
            coroutineCheck = false;
        }
    }

    // Activation update
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
            yield return new WaitForSecondsRealtime(0.2f);
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
            Debug.Log("[TEST] RequestOwnership. This is Server");
        }
        else if ((IsClient && !IsOwner))
        {
            PlayerClientID = NetworkManager.Singleton.LocalClientId; // LocalId;
            localPlayer = NetworkManager.LocalClient.PlayerObject;
            Debug.Log("[TEST] RequestOwnership. ID: " + PlayerClientID+ " Client grabbed the "+ networkObjectSelected);
            if (networkObjectSelected != null)
                Debug.Log("[TEST] RequestOwnership ... in progress" );
                localPlayer.GetComponent<NetworkPlayerRpcCall>().RequestGrabbableOwnershipServerRpc(PlayerClientID, networkObjectSelected); // 이거 고쳐야하나...
        } else {
            Debug.Log("[TEST] RequestOwnership. Error Rq Ownership ---- ");
            printInfo();
        }
    }

    // [ServerRpc]
    public void RequestRemoveOwnership(NetworkObject networkObjectSelected) {
        if (IsServer) {
            Debug.Log("[TEST] RequestRemoveOwnership. This is Server");
        }
        else if ((IsClient && IsOwner))
        {
            PlayerClientID = NetworkManager.Singleton.LocalClientId; // LocalId;
            localPlayer = NetworkManager.LocalClient.PlayerObject;
            Debug.Log("[TEST] RequestRemoveOwnership. ID: " + PlayerClientID+ " Client dropped the "+ networkObjectSelected);
            if (networkObjectSelected != null)
                localPlayer.GetComponent<NetworkPlayerRpcCall>().RequestRetrunGrabbableOwnershipServerRpc(PlayerClientID, networkObjectSelected);
        } else {
            Debug.Log("[TEST] RequestRemoveOwnership. Error Rq Ownership ---- ");
            printInfo();
        }
    }


    public void RequestRemoveParent(NetworkObject childObject, NetworkObject parentObject) {
        if (IsServer) {
            Debug.Log("[TEST] RequestRemoveParent. This is Server");
            // Debug.Log("[TEST] TryRemoveParent = " + childObject.TryRemoveParent(parentObject));
        }
        if ((IsClient))
        {
            PlayerClientID = NetworkManager.Singleton.LocalClientId; //LocalId;
            localPlayer = NetworkManager.LocalClient.PlayerObject;
            Debug.Log("[TEST] RequestRemoveParent. ID: " + PlayerClientID + " Client wants to remove parent "+ parentObject);
            if (childObject == parentObject) 
                Debug.Log("[TEST] RequestRemoveParent. parentObject = childObject = " + parentObject);
            else if (parentObject != null)
                localPlayer.GetComponent<NetworkPlayerRpcCall>().RequestRemoveParentServerRpc(childObject, parentObject);
        } else {
            Debug.Log("[TEST] RequestRemoveParent. Error Rq Ownership ---- ");
            printInfo();
        }
    }

    public void RequestSetParent(NetworkObject childObject, NetworkObject parentObject) {
        if (IsServer) {
            Debug.Log("[TEST] RequestSetParent. This is Server");
            // Debug.Log("[TEST] TrySetParent = " + childObject.TrySetParent(parentObject));
        }
        if ((IsClient))
        {
            PlayerClientID = NetworkManager.Singleton.LocalClientId; //LocalId;
            localPlayer = NetworkManager.LocalClient.PlayerObject;
            Debug.Log("[TEST] RequestSetParent. ID: " + PlayerClientID + " Client wants to set parent "+ parentObject);
            if (childObject == parentObject) 
                Debug.Log("[TEST] RequestSetParent. parentObject = childObject = " + parentObject);
            else if (parentObject != null)
                localPlayer.GetComponent<NetworkPlayerRpcCall>().RequestSetParentServerRpc(childObject, parentObject);
        } else {
            Debug.Log("[TEST] RequestSetParent. Error Rq Ownership ---- ");
            printInfo();
        }
    }

    public void RequestUseGravity(NetworkObject networkObjectSelected, bool _useGravity) {
        if (IsServer) {
            Debug.Log("[TEST] RequestUseGravity. This is Server");
            // useGravity.Value = _useGravity;
        }
        if ((IsClient))
        {
            PlayerClientID = NetworkManager.Singleton.LocalClientId; //LocalId;
            localPlayer = NetworkManager.LocalClient.PlayerObject;
            Debug.Log("[TEST] RequestUseGravity. ID: " + PlayerClientID+ " Client set the gravity " + networkObjectSelected + " " + _useGravity);
            if (networkObjectSelected != null)
                localPlayer.GetComponent<NetworkPlayerRpcCall>().RequestUseGravityServerRpc(networkObjectSelected, _useGravity);
        } else {
            Debug.Log("[TEST] RequestUseGravity. Error Rq Ownership ---- ");
            printInfo();
        }
    }

    public void RequestSetActive(NetworkObject networkObjectSelected, bool _isActive) {
        if (IsServer) {
            Debug.Log("[TEST] RequestSetActive. This is Server");
        }
        if ((IsClient))
        {
            PlayerClientID = NetworkManager.Singleton.LocalClientId; //LocalId;
            localPlayer = NetworkManager.LocalClient.PlayerObject;
            Debug.Log("[TEST] RequestSetActive. ID: " + PlayerClientID+ " Client set the active " + networkObjectSelected + " " + _isActive);
            if (networkObjectSelected != null)
                localPlayer.GetComponent<NetworkPlayerRpcCall>().RequestSetActiveServerRpc(networkObjectSelected, _isActive);
        } else {
            Debug.Log("[TEST] RequestSetActive. Error Rq Ownership ---- ");
            printInfo();
        }
    }

    public void OnSelectGrabbable(SelectEnterEventArgs eventArgs)
    {
        // localPlayer = NetworkManager.LocalClient.PlayerObject;
        // PlayerClientID = NetworkManager.Singleton.LocalClientId;

        // Debug.Log("[TEST] local player object is " + localPlayer + " and ID: " + PlayerClientID);
        Debug.Log("[TEST] OnSelectGrabbable. Grabbed!!");

        // Debug.Log("[TEST] IsClient: "+ IsClient + " IsOwner: " + IsOwner);
        NetworkObject networkObjectSelected = eventArgs.interactableObject.transform.GetComponent<NetworkObject>();

        if (!IsServer) RequestOwnership(networkObjectSelected);

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
        PlayerClientID = NetworkManager.Singleton.LocalClientId; // LocalId;
        Debug.Log("[TEST] OnSelectExitGrabbable. Release!!");
        NetworkObject networkObjectSelected = eventArgs.interactableObject.transform.GetComponent<NetworkObject>();

        if (!IsServer) RequestRemoveOwnership(networkObjectSelected);
        // if ((IsClient && IsOwner))
        // {
        //     Debug.Log("[TEST] ID: " + PlayerClientID+ " Client dropped the "+ networkObjectSelected);
        //     if (networkObjectSelected != null)
        //         localPlayer.GetComponent<NetworkPlayerRpcCall>().RequestRetrunGrabbableOwnershipServerRpc(PlayerClientID, networkObjectSelected);
        // }
    }
}
