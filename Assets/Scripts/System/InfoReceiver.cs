using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoReceiver : MonoBehaviour
{
    private static InfoReceiver _instance;
    public static InfoReceiver Instance
    {
        get
        {
            Init();
            return _instance;
        }
        
    }
    private static void Init()
    {
        if(_instance != null)
        {
            return;
        }
        else
        {
            _instance = GameObject.FindObjectOfType<InfoReceiver>();

            if (_instance == null)
            {
                GameObject obj = new GameObject(typeof(InfoReceiver).Name);
                _instance = obj.AddComponent<InfoReceiver>();   
            }
        }
    }

    [SerializeField] private PlayerHandController _handController;
    private void Start()
    {
        _handController = GetComponent<PlayerHandController>();
    }
    // Info Receiver를 거쳐서 PlayerHandController호출하는 이유 : 전체 Item 개수 관리하기 위함.
    public void GrabLeftHand(GlobalObjects objectInfo)
    {
        //Debug.Log("Grab LeftHand objectInfo.Name "+objectInfo.GUID+ ", "+objectInfo.Name);
        _handController.GrabLeftHand(objectInfo);
        // 아이템 개수 관리 메서드 호출
    }
    public void GrabRightHand(GlobalObjects objectInfo)
    {
        //Debug.Log("Grab RightHand objectInfo.Name "+objectInfo.GUID+ ", "+objectInfo.Name);
        _handController.GrabRightHand(objectInfo);
    }
    public void DropLeftHand(GlobalObjects objectInfo)
    {
        //Debug.Log("Drop LeftHand objectInfo.Name "+objectInfo.GUID+ ", "+objectInfo.Name);
        _handController.DropLeftHand(objectInfo);
    }
    public void DropRightHand(GlobalObjects objectInfo)
    {
        //Debug.Log("Drop RightHand objectInfo.Name "+objectInfo.GUID+ ", "+objectInfo.Name);
        _handController.DropRightHand(objectInfo);
    }
    // 잡고 떨어뜨린 물체가 knife, forcing bag, pen, 틀에 따라 다르게 작용함.
}
