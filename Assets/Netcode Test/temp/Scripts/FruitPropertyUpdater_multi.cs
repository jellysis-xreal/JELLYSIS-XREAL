using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class FruitPropertyUpdater_multi : NetworkBehaviour
{
    // 과일을 자를 때 생기는 변화에 관한 스크립트
    [SerializeField] private NetworkSyncObject fruitPieceObject;
    [SerializeField] private NetworkSyncObject fruitObject;
    [SerializeField] private int hp = 1;

    // [SerializeField] private NetworkObject parentTransform; // 다른 network object를 부모로 할 수 있음 -> NetworkObject.TryRemoveParent

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 20 )
        {
            // knife 감지 되면 해당 knife의 owner로 바꾸기
            if ((IsClient && !IsOwner)) fruitObject.RequestOwnership(gameObject.transform.GetComponent<NetworkObject>());
            Debug.Log(other.name+" trigger enter!");
            hp -= 1;
            if (hp <= 0)
            {
                // 상위 오브젝트의 컴포넌트 지워야됨 생성되면서 원본과 자른 과일이 충돌함.
                // fruitPieceObject.transform.SetParent(null);
                // gameObject.SetActive(false);
                // fruitPieceObject.SetActive(true);
                // other.transform.GetComponent<NetworkObject>()
                // fruitObject.RequestSetActive(fruitObject.gameObjectToSetActive, false);
                // fruitPieceObject.RequestSetActive(fruitPieceObject.gameObjectToSetActive, true);
                fruitObject.SetActiveNetworkObject(false);
                fruitPieceObject.SetActiveNetworkObject(true);
            }
            if ((IsClient && IsOwner) && !IsServer) fruitObject.RequestRemoveOwnership(gameObject.transform.GetComponent<NetworkObject>());
        }
    }
}
