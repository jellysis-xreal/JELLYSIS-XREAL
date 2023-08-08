using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoSender : MonoBehaviour
{
    [SerializeField] public GlobalObjects myInfo;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider col;
    [SerializeField] private bool isTriggering;
    void Start()
    {
        //myInfo = GetComponent<GlobalObjects>();
        rb = GetComponent<Rigidbody>();
        /*if (myInfo == null)
        {
            myInfo = GetComponentInChildren<GlobalObjects>();   
        }*/
        col = GetComponentInChildren<Collider>();
        Init();
    }

    private void Update()
    {
        //Debug.Log(transform.position);
    }

    void Init()
    {
        rb.useGravity = true;
        //col.isTrigger = false;
    }

    void SetIsGrabbed()
    {
        rb.useGravity = false;
        //col.isTrigger = true;
    }

    void SetDrop()
    {
        if (isTriggering)
        {
            // nothing
        }
        else
        {
            rb.useGravity = true;
            //col.isTrigger = false;
        }
    }
    // 처음 상태 gravity o, kinematic x, triggerx  
    // 잡고 있는 상태 gravity x, kinematic x, is Trigger o
    // Trigger됐는데 놓았을 때
    public void ChangeShape(GlobalObjects objectInfo)
    {
        //Debug.Log(objectInfo.Name +" 으로 바꿀래!");
        myInfo = objectInfo;
    }
    public void GrabLeftHandInfoToReceiver()
    {        
        InfoReceiver.Instance.GrabLeftHand(myInfo);
        SetIsGrabbed();
        //col.isTrigger = true;
    }
    public void GrabRightHandInfoToReceiver()
    {        
        InfoReceiver.Instance.GrabRightHand(myInfo);
        SetIsGrabbed();
        //col.isTrigger = true;
    }
    public void DropLeftHandInfoToReceiver()
    {       
        InfoReceiver.Instance.DropLeftHand(myInfo);
        SetDrop();
        //col.isTrigger = false;
    }
    public void DropRightHandInfoToReceiver()
    {        
        InfoReceiver.Instance.DropRightHand(myInfo);
        SetDrop();
        //col.isTrigger = false;
        // 오브젝트를 TriggerStay 상태에서 Drop을 해버린다.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            /*Transform originalParent = transform.parent;
            Vector3 originalLocalPosition = transform.localPosition;
            Quaternion originalLocalRotation = transform.localRotation;
            Vector3 originalLocalScale = transform.localScale;

// 부모 변경
            //transform.SetParent(newParent);

// 위치, 회전, 스케일 보정
            transform.localPosition = originalLocalPosition;
            transform.localRotation = originalLocalRotation;
            transform.localScale = originalLocalScale;*/

            Debug.Log("trigger Enter "+other.name);
            //rb.isKinematic = true;
            /*rb.angularVelocity = Vector3.zero;            
            rb.velocity = Vector3.zero;*/
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            Debug.Log(transform.position);
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            Debug.Log(other.name + " trigger Exit");
            //isTriggering = false;
            //rb.isKinematic = false;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
            transform.SetParent(null); // Trigger Exit되면 종속관계 해제
        }
    }*/
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 11) // && myInfo.GUID < 100)
        {
            Debug.Log(other.name + " trigger Stay");
            //isTriggering = true;            rb.isKinematic = true;
            //rb.isKinematic = true;
            rb.useGravity = false;
            //transform.SetParent(other.transform); // Trigger된 오브젝트의 자식으로 놔둠.
            //Debug.Log(transform.localPosition);
            //Debug.Log(transform.position);
            //rb.angularVelocity = Vector3.zero;            
            //rb.velocity = Vector3.zero;
        }
    }
    
    
    
    void DrawGizmoAttachable()
    {
        // 붙일 수 있을 경우 아이템 테두리에 윤곽선 보임
    }
}
