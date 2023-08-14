using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class ItempPropertyMulti : NetworkBehaviour
{
    [SerializeField] private NetworkObject netPreTrans; 
    [SerializeField] private NetworkSyncObject NSO_DecoObject;
    [SerializeField] private bool isDropped = false;

    [SerializeField] private NetworkObject NO_DecoObject;

    // [SerializeField] private ulong OwnerId;

    void Start()
    {
        NO_DecoObject = gameObject.transform.GetComponent<NetworkObject>();
    }

    private void printInfo() {
        Debug.Log("[TEST] IsClient, IsServer, IsOwner: " + IsClient + ", " + IsServer + ", " + IsOwner);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            netPreTrans = other.transform.GetComponent<NetworkObject>();
            Debug.Log("[TEST] 0. OnTriggerEnter!");
            Debug.Log("[TEST] 0. detected Object = " + netPreTrans);

            if (isDropped)
            {
                Debug.Log("[TEST] 0.1. isDropped! ");
                // printInfo();
                // if ((IsClient && !IsOwner) && !IsServer) NSO_DecoObject.RequestOwnership(NO_DecoObject);
                // printInfo();
                // if (IsOwner) NSO_DecoObject.UseGravityNetworkObject(false);
                NSO_DecoObject.RequestUseGravity(NO_DecoObject, false);
                // if (IsOwner) Debug.Log("[TEST] Rq set parent " + netPreTrans);
                NSO_DecoObject.RequestSetParent(NO_DecoObject, netPreTrans);
                // if ((IsClient && IsOwner) && !IsServer) NSO_DecoObject.RequestRemoveOwnership(NO_DecoObject);
            } else {
                Debug.Log("[TEST] 0.2. !isDropped ");
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11 && !isDropped)
        {
            Debug.Log("[TEST] 2. OnTriggerExit!");
            if (netPreTrans != null) {
                Debug.Log("[TEST] 2.1. netPreTrans != null"); // we should remove the netPreTrans
                NSO_DecoObject.RequestRemoveParent(NO_DecoObject, netPreTrans);

                // printInfo();
                // if ((IsClient && !IsOwner) && !IsServer) NSO_DecoObject.RequestOwnership(NO_DecoObject);
                // printInfo();
                // // if (IsOwner) NSO_DecoObject.UseGravityNetworkObject(true);
                // if (IsOwner) Debug.Log("[TEST] 2.2. Rq remove parent " + NO_DecoObject.TryRemoveParent(netPreTrans));
                // if ((IsClient && IsOwner) && !IsServer) NSO_DecoObject.RequestRemoveOwnership(NO_DecoObject);

                netPreTrans = null;
            }
        }
    }

    public void ItemSelectEvent()
    {
        Debug.Log("[TEST] 3. ItemSelected!");
        isDropped = false;
        printInfo();
        // if ((IsClient && !IsOwner) && !IsServer) NSO_DecoObject.RequestOwnership(NO_DecoObject);
        printInfo();
        // if (IsOwner) NSO_DecoObject.UseGravityNetworkObject(false);
        // NSO_DecoObject.RequestUseGravity(NO_DecoObject, false);

        // printInfo();
        // if ((IsClient && !IsOwner) && !IsServer) NSO_DecoObject.RequestOwnership(NO_DecoObject);
        // printInfo();
    }

    public void ItemUnSelectEvent()
    {
        Debug.Log("[TEST] 4. ItemUnSelected!");
        isDropped = true;
        if (netPreTrans != null && netPreTrans.gameObject.layer == 11)
        {
            Debug.Log("[TEST] 4.1. ItemUnnetPreTrans != null!");
            
            // printInfo();
            // if ((IsClient && !IsOwner) && !IsServer) NSO_DecoObject.RequestOwnership(NO_DecoObject);
            // printInfo();
            NSO_DecoObject.RequestUseGravity(NO_DecoObject, false);
            // if (IsOwner) NSO_DecoObject.UseGravityNetworkObject(false);
            // if (IsOwner) Debug.Log("[TEST] 4.2. Rq set parent " + netPreTrans);
            NSO_DecoObject.RequestSetParent(NO_DecoObject, netPreTrans);
            // if ((IsClient && IsOwner) && !IsServer) NSO_DecoObject.RequestRemoveOwnership(NO_DecoObject);

            // if ((IsClient && IsOwner) && !IsServer) NSO_DecoObject.RequestRemoveOwnership(NO_DecoObject);
        } else {
            Debug.Log("[TEST] 4.2. ItemUnnetPreTrans == null!");

            // if ((IsClient && !IsOwner) && !IsServer) NSO_DecoObject.RequestOwnership(NO_DecoObject);
            // if (IsOwner) Debug.Log("[TEST] 4.3. Rq remove parent " + netPreTrans);
            // NSO_DecoObject.RequestRemoveParent(NO_DecoObject, netPreTrans);
            // if (IsOwner) NSO_DecoObject.UseGravityNetworkObject(true);
            NSO_DecoObject.RequestUseGravity(NO_DecoObject, true);
            // if ((IsClient && IsOwner) && !IsServer) NSO_DecoObject.RequestRemoveOwnership(NO_DecoObject);
        }
    }
}