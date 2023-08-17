using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Cut_Banana_multi : NetworkBehaviour
{
    public NetworkObject origin_banana;
    public NetworkObject origin_bananaPiece;
    public NetworkSyncObject NSO_origin_bananaPiece;
    public NetworkObject new_bananaPiece;
    public AudioSource cut_banana;

    public bool isCut = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 20)
        {
            Debug.Log("[TEST] banana and knife triggered! ");
            isCut = true;
            cut_banana.Play();
            // NSO_new_bananaPiece.RequestOwnership(new_bananaPiece);
            // NSO_new_bananaPiece.SetActiveNetworkObject(true);
            Debug.Log("[TEST] banana set activated ");
            Debug.Log("[TEST] " + new_bananaPiece + " " + origin_banana);
            RequestRemoveParent(new_bananaPiece, origin_banana);
            Debug.Log("[TEST] banana parent removed ");
            RequestOwnership(origin_bananaPiece);
            NSO_origin_bananaPiece.SetActiveNetworkObject(false);
        }
    }


    public void RequestRemoveParent(NetworkObject childObject, NetworkObject parentObject) {
        if (IsServer) {
            Debug.Log("[TEST] RequestRemoveParent. This is Server");
            // Debug.Log("[TEST] TryRemoveParent = " + childObject.TryRemoveParent(parentObject));
        }
        if ((IsClient))
        {
            ulong PlayerClientID = NetworkManager.Singleton.LocalClientId; //LocalId;
            NetworkObject localPlayer = NetworkManager.LocalClient.PlayerObject;
            Debug.Log("[TEST] RequestRemoveParent. ID: " + PlayerClientID + " Client wants to remove parent "+ parentObject);
            if (childObject == parentObject) 
                Debug.Log("[TEST] RequestRemoveParent. parentObject = childObject = " + parentObject);
            else if (parentObject != null){
                Debug.Log("[TEST] " + childObject);
                Debug.Log("[TEST] " + localPlayer);
                Debug.Log("[TEST] " + localPlayer.GetComponent<NetworkPlayerRpcCall>());
                localPlayer.GetComponent<NetworkPlayerRpcCall>().RequestRemoveParentServerRpc(childObject, parentObject);
            }
        } else {
            Debug.Log("[TEST] RequestRemoveParent. Error Rq Ownership ---- ");
        }
    }


    public void RequestOwnership(NetworkObject networkObjectSelected)
    {
        if (IsServer) {
            Debug.Log("[TEST] RequestOwnership. This is Server");
        }
        //else if ((IsClient && !IsOwner))
        else if ((!IsOwner))
        {
            ulong PlayerClientID = NetworkManager.Singleton.LocalClientId; //LocalId;
            NetworkObject localPlayer = NetworkManager.LocalClient.PlayerObject;
            Debug.Log("[TEST] RequestOwnership. ID: " + PlayerClientID+ " Client grabbed the "+ networkObjectSelected);
            if (networkObjectSelected != null)
                Debug.Log("[TEST] RequestOwnership ... in progress" );
                localPlayer.GetComponent<NetworkPlayerRpcCall>().RequestGrabbableOwnershipServerRpc(PlayerClientID, networkObjectSelected); // 이거 고쳐야하나...
        } else {
            Debug.Log("[TEST] RequestOwnership. Error Rq Ownership ---- ");
        }
    }

    // [ServerRpc]
    public void RequestRemoveOwnership(NetworkObject networkObjectSelected) {
        if (IsServer) {
            Debug.Log("[TEST] RequestRemoveOwnership. This is Server");
        }
        else if ((IsClient && IsOwner))
        {
            ulong PlayerClientID = NetworkManager.Singleton.LocalClientId; //LocalId;
            NetworkObject localPlayer = NetworkManager.LocalClient.PlayerObject;
            Debug.Log("[TEST] RequestRemoveOwnership. ID: " + PlayerClientID+ " Client dropped the "+ networkObjectSelected);
            if (networkObjectSelected != null)
                localPlayer.GetComponent<NetworkPlayerRpcCall>().RequestRetrunGrabbableOwnershipServerRpc(PlayerClientID, networkObjectSelected);
        } else {
            Debug.Log("[TEST] RequestRemoveOwnership. Error Rq Ownership ---- ");
        }
    }
}
