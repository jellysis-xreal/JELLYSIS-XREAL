using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class ItempPropertyMulti : NetworkBehaviour
{
    [SerializeField] private NetworkObject netPreTrans; 
    [SerializeField] private NetworkSyncObject NSO_DecoObject;
    [SerializeField] private bool isDropped = true;

    [SerializeField] private NetworkObject NO_DecoObject;

    [SerializeField] private FruitTransformer_multi _fruitTransformer;

    [SerializeField] private bool shouldRemoveParent = false;

    void Start()
    {
        NO_DecoObject = gameObject.transform.GetComponent<NetworkObject>();
    }

    private void printInfo() {
        Debug.Log("[TEST] IsClient, IsServer, IsOwner: " + IsClient + ", " + IsServer + ", " + IsOwner);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11 || other.gameObject.layer == 21)
        {
            netPreTrans = other.transform.GetComponent<NetworkObject>();
            Debug.Log("[TEST] 0. OnTriggerEnter!");
            Debug.Log("[TEST] 0. detected Object = " + netPreTrans);

            if (isDropped && other.gameObject.layer == 11)
            {
                Debug.Log("[TEST] 0.1. isDropped! ");
                NSO_DecoObject.RequestUseGravity(NO_DecoObject, false);
                NSO_DecoObject.RequestSetParent(NO_DecoObject, netPreTrans);
            }
            else {
                Debug.Log("[TEST] 0.2. !isDropped ");
                shouldRemoveParent = false;
            }
        }

        if(other.gameObject.layer == 21 && !isDropped)
        {
            netPreTrans = other.transform.GetComponent<NetworkObject>();
            _fruitTransformer = other.transform.GetComponent<FruitTransformer_multi>(); // NetworkObject
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.layer == 11 && !isDropped) || other.gameObject.layer == 21)
        {
            Debug.Log("[TEST] 2. OnTriggerExit!");
            if ((netPreTrans != null && other.gameObject.layer == 11) || (netPreTrans != null && other.gameObject.layer == 21)) {
                Debug.Log("[TEST] 2.1. netPreTrans != null"); // we should remove the netPreTrans
                shouldRemoveParent = true;
            }
        }
    }

    public void ItemSelectEvent()
    {
        Debug.Log("[TEST] 3. ItemSelected!");
        isDropped = false;
        if (_fruitTransformer) _fruitTransformer.DetachFruit(); // 고쳐야함
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
        else if (netPreTrans != null && netPreTrans.gameObject.layer == 21 && !shouldRemoveParent)
        {
            Debug.Log("[TEST] 4.1.3 ItemUnnetPreTrans != null! shouldRemoveParent and fruit");
            if ((IsClient && !IsOwner)) NSO_DecoObject.RequestOwnership(NO_DecoObject);
            NSO_DecoObject.RequestUseGravity(NO_DecoObject, false);
            _fruitTransformer.AttachFruit(NO_DecoObject);
            //if ((IsClient && IsOwner) && !IsServer) NSO_DecoObject.RequestRemoveOwnership(NO_DecoObject);
           // NSO_DecoObject.RequestUseGravity(NO_DecoObject, false);

            Debug.Log("[TEST] 4.1.4 ID: " + NSO_DecoObject.rigidBody + " velocity, angularVelocity: " + NSO_DecoObject.rigidBody.velocity + ", " + NSO_DecoObject.rigidBody.angularVelocity);
            //NSO_DecoObject.RequestSetParent(NO_DecoObject, netPreTrans);

        }
        else if (netPreTrans != null && netPreTrans.gameObject.layer == 21 && shouldRemoveParent)
        {
            Debug.Log("[TEST] 4.1.3 ItemUnnetPreTrans != null! shouldRemoveParent and fruit");
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