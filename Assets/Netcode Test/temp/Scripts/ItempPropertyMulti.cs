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

    private bool shouldRemoveParent = false;

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
                NSO_DecoObject.RequestUseGravity(NO_DecoObject, false);
                NSO_DecoObject.RequestSetParent(NO_DecoObject, netPreTrans);
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
                shouldRemoveParent = true;
            }
        }
    }

    public void ItemSelectEvent()
    {
        Debug.Log("[TEST] 3. ItemSelected!");
        isDropped = false;
        // printInfo();
    }

    public void ItemUnSelectEvent()
    {
        Debug.Log("[TEST] 4. ItemUnSelected!");
        isDropped = true;
        if (netPreTrans != null && netPreTrans.gameObject.layer == 11 && !shouldRemoveParent)
        {
            Debug.Log("[TEST] 4.1.1 ItemUnnetPreTrans != null!");
            NSO_DecoObject.RequestUseGravity(NO_DecoObject, false);
            NSO_DecoObject.RequestSetParent(NO_DecoObject, netPreTrans);
        }
        else if (netPreTrans != null && netPreTrans.gameObject.layer == 11 && shouldRemoveParent)
        {
            Debug.Log("[TEST] 4.1.2 ItemUnnetPreTrans != null! shouldRemoveParent");
            NSO_DecoObject.RequestRemoveParent(NO_DecoObject, netPreTrans);
            NSO_DecoObject.RequestUseGravity(NO_DecoObject, true);
            netPreTrans = null;
            shouldRemoveParent = false;
        }
        else {
            Debug.Log("[TEST] 4.2. ItemUnnetPreTrans == null!");
            NSO_DecoObject.RequestUseGravity(NO_DecoObject, true);
        }
    }
}