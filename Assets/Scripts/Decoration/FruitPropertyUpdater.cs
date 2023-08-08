using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitPropertyUpdater : MonoBehaviour
{
    // 과일을 자를 때 생기는 변화에 관한 스크립트
    [SerializeField] private GameObject fruitPieceObject;
    [SerializeField] private int hp = 5;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 20 )
        {
            Debug.Log(other.name+" trigger enter!");
            hp -= 1;
            if (hp == 0)
            {
                // 상위 오브젝트의 컴포넌트 지워야됨 생성되면서 원본과 자른 과일이 충돌함.
                fruitPieceObject.transform.SetParent(null);
                gameObject.SetActive(false);
                fruitPieceObject.SetActive(true);
            }
        }
    }
}
