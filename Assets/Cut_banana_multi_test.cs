using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Cut_banana_multi_test : NetworkBehaviour
{
    // public GameObject origin_banana;
    // public GameObject banana;
    // public AudioSource cut_banana;
    // public NetworkSyncObject NSO_DecoObject;
    // public NetworkSyncObject NSO_origin_bananaPiece;
    // public NetworkSyncObject NSO_new_bananaPiece;
    // public NetworkObject origin_banana;
    // public NetworkObject origin_bananaPiece;
    // public NetworkObject new_bananaPiece;
    public AudioSource cut_banana;


// 과일을 자를 때 생기는 변화에 관한 스크립트
    [SerializeField] private NetworkSyncObject bananaPieceObject;
    [SerializeField] private NetworkSyncObject originalBananaObject;

    // [SerializeField] private NetworkObject parentTransform; // 다른 network object를 부모로 할 수 있음 -> NetworkObject.TryRemoveParent

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 20 )
        {
            // knife 감지 되면 해당 knife의 owner로 바꾸기
            if ((IsClient && !IsOwner)) originalBananaObject.RequestOwnership(originalBananaObject.transform.GetComponent<NetworkObject>());
            Debug.Log(other.name+" trigger enter!");
            cut_banana.Play();
            bananaPieceObject.RequestRemoveParent(bananaPieceObject.transform.GetComponent<NetworkObject>(), originalBananaObject.transform.GetComponent<NetworkObject>());
            if ((IsClient && IsOwner) && !IsServer) originalBananaObject.RequestRemoveOwnership(originalBananaObject.transform.GetComponent<NetworkObject>());
        }
    }



    // public bool isCut = false;
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     if (!isCut)
    //     {
    //         NSO_new_bananaPiece.RequestOwnership(new_bananaPiece);
    //         NSO_new_bananaPiece.SetActiveNetworkObject(false);
    //         Debug.Log("[TEST] piece false");
    //         isCut = true;
    //     } else {
    //         // NSO_new_bananaPiece.RequestOwnership(new_bananaPiece);
    //         // NSO_new_bananaPiece.SetActiveNetworkObject(true);
    //     }
    // }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.layer == 20)
    //     {
    //         Debug.Log("[TEST] banana and knife triggered! ");
    //         isCut = true;
    //         cut_banana.Play();
    //         NSO_new_bananaPiece.RequestOwnership(new_bananaPiece);
    //         NSO_new_bananaPiece.SetActiveNetworkObject(true);
    //         Debug.Log("[TEST] banana set activated ");
    //         NSO_DecoObject.RequestRemoveParent(new_bananaPiece, origin_banana);
    //         Debug.Log("[TEST] banana parent removed ");
    //         NSO_origin_bananaPiece.RequestOwnership(origin_bananaPiece);
    //         // NSO_origin_bananaPiece.SetActiveNetworkObject(false);
    //     }
    // }
}
