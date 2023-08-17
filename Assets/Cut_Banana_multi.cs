using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Cut_Banana_multi : NetworkBehaviour
{
    // public GameObject origin_banana;
    // public GameObject banana;
    // public AudioSource cut_banana;
    public NetworkSyncObject NSO_DecoObject;
    public NetworkSyncObject NSO_origin_bananaPiece;
    public NetworkSyncObject NSO_new_bananaPiece;
    public NetworkObject origin_banana;
    public NetworkObject origin_bananaPiece;
    public NetworkObject new_bananaPiece;
    public AudioSource cut_banana;

    public bool isCut = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCut)
        {
            NSO_new_bananaPiece.RequestOwnership(new_bananaPiece);
            NSO_new_bananaPiece.SetActiveNetworkObject(false);
            Debug.Log("[TEST] piece false");
            isCut = true;
        } else {
            // NSO_new_bananaPiece.RequestOwnership(new_bananaPiece);
            // NSO_new_bananaPiece.SetActiveNetworkObject(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 20)
        {
            Debug.Log("[TEST] banana and knife triggered! ");
            isCut = true;
            cut_banana.Play();
            NSO_new_bananaPiece.RequestOwnership(new_bananaPiece);
            NSO_new_bananaPiece.SetActiveNetworkObject(true);
            Debug.Log("[TEST] banana set activated ");
            NSO_DecoObject.RequestRemoveParent(new_bananaPiece, origin_banana);
            Debug.Log("[TEST] banana parent removed ");
            NSO_origin_bananaPiece.RequestOwnership(origin_bananaPiece);
            // NSO_origin_bananaPiece.SetActiveNetworkObject(false);
        }
    }
}
