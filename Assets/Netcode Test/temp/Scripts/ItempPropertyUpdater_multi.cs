using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class ItempPropertyUpdater_ulti : NetworkBehaviour
{
    [SerializeField] private NetworkObject netPreTrans; 
    
    [SerializeField] private bool isDropped = true;

    [SerializeField] private NetworkSyncObject NSO_DecoObject;
    private NetworkObject NO_DecoObject;
    [SerializeField] private ulong OwnerId;

    void Start()
    {
        NO_DecoObject = gameObject.transform.GetComponent<NetworkObject>();
        OwnerId = NO_DecoObject.OwnerClientId;
    }
    private void printInfo() {
        Debug.Log("[TEST] IsClient, IsServer, IsOwner: " + IsClient + ", " + IsOwner + ", " + IsServer);
    }

    private void Update() {
        OwnerId = NO_DecoObject.OwnerClientId;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11 && !isDropped)
        {
            Debug.Log("[TEST] 0. JellyBear detected!");
            netPreTrans = other.transform.GetComponent<NetworkObject>();
            Debug.Log("[TEST] 0. netPreTrans = " + netPreTrans);
        }

        if (other.gameObject.layer == 11 && isDropped)
        {
            Debug.Log("[TEST] 0 여기 나와야 함 1. JellyBear detected!");
            printInfo();
            if ((IsClient && !IsOwner)) {
                Debug.Log("[TEST] 0.1 1. Request Ownership!");
                NSO_DecoObject.RequestOwnership(NO_DecoObject);
                Debug.Log("[TEST] 0.1 1. Got response! OwnerID = " + NO_DecoObject.OwnerClientId +
                             ", PlayerID = " + NetworkManager.Singleton.LocalClientId);
            } else {
                printInfo();
            }
            OwnerId = NO_DecoObject.OwnerClientId;
            if (IsOwner) NSO_DecoObject.UseGravityNetworkObject(false);
            else {
                Debug.Log("[TEST] this player not IsOwner");
            }
            netPreTrans = other.transform.GetComponent<NetworkObject>();
            Debug.Log("[TEST] 1 여기 나와야 함 1. OwnerId= " + OwnerId + " netPreTrans = " + netPreTrans);
            if (IsOwner) {
                Debug.Log("[TEST] 2 여기 나와야 함 1. IsOwner? " + IsOwner + " try setParent? " + NO_DecoObject.TrySetParent(netPreTrans));
                netPreTrans = null;
            }
            if ((IsClient && IsOwner) && !IsServer) NSO_DecoObject.RequestRemoveOwnership(NO_DecoObject);
        }

        if (other.gameObject.layer == 21 && !isDropped)
        {
            Debug.Log("[TEST] 2. Stick Interaction 0");
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11 || other.gameObject.layer == 21)
        {
            Debug.Log("[TEST] 3. JellyBear trigger exit!");
            if (netPreTrans != null) {
                Debug.Log("[TEST] 3.1. netPreTrans != null");
                if ((IsClient && !IsOwner)) { Debug.Log("request ownership"); NSO_DecoObject.RequestOwnership(NO_DecoObject); }
                if (IsOwner) NO_DecoObject.TryRemoveParent(netPreTrans);
                if (IsOwner) {Debug.Log("[TEST] 3.2. "+ netPreTrans); NSO_DecoObject.UseGravityNetworkObject(true);}
                if ((IsClient && IsOwner) && !IsServer) NSO_DecoObject.RequestRemoveOwnership(NO_DecoObject);
            }
        }
    }
    public void ItemSelectEvent()
    {
        isDropped = false;
    }
    public void ItemUnSelectEvent() // 손에서 놨을 때 event
    {
        if (netPreTrans != null && netPreTrans.gameObject.layer == 11)
        {
            if ((IsClient && !IsOwner)) NSO_DecoObject.RequestOwnership(NO_DecoObject);
            Debug.Log(netPreTrans.name + "UnSelect");
            if (IsOwner) NSO_DecoObject.UseGravityNetworkObject(false);
            Debug.Log("[TEST] 4. netPreTrans = " + netPreTrans);
            if (IsOwner) NO_DecoObject.TrySetParent(netPreTrans);
            if ((IsClient && IsOwner) && !IsServer) NSO_DecoObject.RequestRemoveOwnership(NO_DecoObject);
        }else if (netPreTrans != null && netPreTrans.gameObject.layer == 21)
        {
            Debug.Log("[TEST] 5. Stick Interaction");
        }
        else
        {
            if ((IsClient && !IsOwner)) NSO_DecoObject.RequestOwnership(NO_DecoObject);
            Debug.Log("[TEST] 6. netPreTrans = " + netPreTrans);
            if (IsOwner) { NO_DecoObject.TryRemoveParent(netPreTrans); netPreTrans = null; NSO_DecoObject.UseGravityNetworkObject(true);}
            isDropped = true;
            if ((IsClient && IsOwner) && !IsServer) NSO_DecoObject.RequestRemoveOwnership(NO_DecoObject);
        }
    }
}