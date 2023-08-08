using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum HandState
{
    Empty,
    GrabAttachableObject,
    GrabForcingBag,
    GrabKnife,
    GrabBrush,
    GrabMold,
    GrabPen
}

public class PlayerHandController : MonoBehaviour
{
    // 임시 싱글톤, Left, RightHandState를 다른 오브젝트에서 검사해서 메서드 실행하는데 사용
    private static PlayerHandController _instance;
    public static PlayerHandController Instance
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
            _instance = GameObject.FindObjectOfType<PlayerHandController>();

            if (_instance == null)
            {
                GameObject obj = new GameObject(typeof(PlayerHandController).Name);
                _instance = obj.AddComponent<PlayerHandController>();   
            }
        }
    }
    
    public HandState leftHandState;
    public HandState rightHandState;

    #region Forcing Bag
    [Space(10)]
    public float timer = 0f;
    public float delayTime = 3f;
    [SerializeField] private float scaleSpeed;
    //[SerializeField] private bool isScalingUp = false;
    [SerializeField] private GameObject _recentMakedCream;
    #endregion
    void Start()
    {
        leftHandState = HandState.Empty;
        rightHandState = HandState.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Info Sender로 부터 잡았다는 정보를 받으면 잡은 상태 초기화
    // 
    public void GrabLeftHand(GlobalObjects objectInfo)
    {
        UpdateLeftHandState(objectInfo.GUID);
    }
    public void GrabRightHand(GlobalObjects objectInfo)
    {
        UpdateRightHandState(objectInfo.GUID);
    }
    public void DropLeftHand(GlobalObjects objectInfo)
    {
        UpdateLeftHandState(objectInfo.GUID);
    }
    public void DropRightHand(GlobalObjects objectInfo)
    {
        UpdateRightHandState(objectInfo.GUID);
    }
    void UpdateLeftHandState(uint objectGUID)
    {
        if (leftHandState == HandState.Empty)
        {
            // Grab한 오브젝트로 HandState 설정
            if (objectGUID >= 100 && objectGUID <= 109) // 현재 사용할 도구가 100 ~109
            {
                if (objectGUID == 100)
                {
                    leftHandState = HandState.GrabKnife;
                }else if (objectGUID >= 101 && objectGUID <= 103)
                {
                    leftHandState = HandState.GrabMold;
                }else if (objectGUID == 104)
                {
                    leftHandState = HandState.GrabForcingBag;
                }else if (objectGUID == 105)
                {
                    leftHandState = HandState.GrabBrush;
                }else if (objectGUID >= 106 && objectGUID <= 109)
                {
                    leftHandState = HandState.GrabPen;
                }
            }
            else if (objectGUID >= 0 && objectGUID < 100) 
            {
                leftHandState = HandState.GrabAttachableObject;
            }
        }
        else
        {
            //Debug.Log(objectGUID+" : left drop");
            leftHandState = HandState.Empty;
        }
        
    }
    void UpdateRightHandState(uint objectGUID)
    {
        if (rightHandState == HandState.Empty)
        {
            // Grab한 오브젝트로 HandState 설정
            if (objectGUID >= 100 && objectGUID <= 109) // 현재 사용할 도구가 100 ~109
            {
                if (objectGUID == 100)
                {
                    rightHandState = HandState.GrabKnife;
                }else if (objectGUID >= 101 && objectGUID <= 103)
                {
                    rightHandState = HandState.GrabMold;
                }else if (objectGUID == 104)
                {
                    rightHandState = HandState.GrabForcingBag;
                }else if (objectGUID == 105)
                {
                    rightHandState = HandState.GrabBrush;
                }else if (objectGUID >= 106 && objectGUID <= 109)
                {
                    rightHandState = HandState.GrabPen;
                }
            }
            else if (objectGUID >= 0 && objectGUID < 100) 
            {
                rightHandState = HandState.GrabAttachableObject;
            }
        }
        else
        {
            //Debug.Log(objectGUID+" : right drop");
            rightHandState = HandState.Empty;
        }
    }
}
