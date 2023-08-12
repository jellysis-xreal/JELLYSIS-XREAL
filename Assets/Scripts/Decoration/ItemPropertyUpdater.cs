using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPropertyUpdater : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    [SerializeField]private Transform preTransform; // 현재 Trigger 중일 경우에만 true
    [SerializeField] private bool[] isHavingChild;
    //public Transform parentTransform;
    private Vector3 _objectScale;
    [SerializeField] private bool isDropped = false;

    [SerializeField] private FruitTransformer _fruitTransformer;
    void Start()
    {
        _objectScale = transform.localScale;
    }
    
    // TriggerEnter 하는 순간 Scale 작아짐, 작아지면서 Trigger Exit됨. Exit 되는 순간 Scale 커짐.
    // TriggerEnter 하는 순간 가장 최근에 닿은 Collider 기억해놨다가 UnHover했을 때 호출하는 방식
    private void OnTriggerEnter(Collider other)
    {
        // 던져서 붙이기 Trigger Enter했을 때 Scale 감소하는 이슈 발생
        // Inspector window 기준 Scale은 들어갈 오브젝트의 lossyScale / 부모의 오브젝트의 lossyScale 
        if (other.gameObject.layer == 11 && isDropped) // 곰돌이에 직접 닿은 게 아닌 던져서 넣을 경우
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            preTransform = other.transform;
            Vector3 calVec = new Vector3(transform.lossyScale.x / preTransform.lossyScale.x,
                transform.lossyScale.y / preTransform.lossyScale.y,
                transform.lossyScale.z / preTransform.lossyScale.z);
            transform.SetParent(preTransform, true);
            transform.localScale = calVec;
        }

        if (other.gameObject.layer == 21 && !isDropped) // stick이고 잡은 상태로 Trigger 됐을 때, 떨어트려서 꽃는 건 배제함.
        {
            preTransform = other.transform;
            _fruitTransformer = other.GetComponent<FruitTransformer>();
            // 과일을 stick에 꽂았을 때 자리에 배치되도록 한다. 이건 UnSelectEvent에서 처리하는 게 맞을 듯..?
            // other.gameobject.GetComponent<PlaceFruit>().placefruit(); 이렇게? 근데 enter되면 Unselect에서 붙을 수 있도록 하고
            // exit되면 unselect에서 안 붙도록 함.
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer ==11 || other.gameObject.layer == 21)
        {
            preTransform = null;
            _fruitTransformer = null;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
        }
    }
    // 지금은 아이템에 할당된 Lllll 스크립트를 직접 이벤트에 추가한 형태
    // playerHandController? 그걸로 잡은 오브젝트에 대해 접근해서 ItemSelectEvent 실행해야 하고 놓았을 때 그 오브젝트의 ItemUnSelectEvent 실행해야 한다.
    public void ItemSelectEvent()
    {
        isDropped = false;
        // 탕후루에 꽂혔다가 잡혔을 때 FruitTransform.DetachFruit 호출
        if (_fruitTransformer) _fruitTransformer.DetachFruit();
    }
    public void ItemUnSelectEvent() // 손에서 놨을 때 event
    {
        // 현재 컨트롤러로 잡고 있는 상태에서 다른 콜라이더랑 TriggerEnter이 안되는 것 같음. ㄴㄴ Trigger 관련 코드는 Collider가 존재하는 오브젝트에 함께 존재해야 함.
        // _preTransform에 할당됨. 그럼 뭘 해야할까? 직접 잡은 후에 자식으로 안 들어감. 자식으로 들어가야 정답 처리가 가능할 듯
        // 자식으로 안 들어가는 이유는?
        if (preTransform != null && preTransform.gameObject.layer == 11)
        {
            // Client -> Host Call 내가 이제 넘길 거야. 내가 오너쉽가질거야
            // 아직 곰돌이와 Trigger 중이면 그 Collider에 붙임.
            Debug.Log(preTransform.name + "UnSelect");
            transform.SetParent(preTransform);
            
            // Call 이제 바꾼다 useGravity(Network 변수)를 false로 만드는 콜을 날린다.
            // host는 useGravity가 false구나를 알아채고 host가 다시 다른 client에게 해당 콜을 날림.
            // host로 부터 해당 콜을 받으면 아래 코드를 실행하면 됨.
            rb.useGravity = false;
            rb.isKinematic = true;
        }else if (preTransform != null && preTransform.gameObject.layer == 21)
        {
            // 아직 stick과 Trigger 중이면 그 Collider에 붙임.
            // 이 이후에 Call
            _fruitTransformer.AttachFruit(transform);
            rb.useGravity = false;
            rb.isKinematic = true;
            Debug.Log("place Fruit!");
        }
        else
        {
            // 잡은 물체가 곰돌이에서 벗어나 있을 경우 SetParent(null)로 부모 관계 해제
            // 잡은 물체를 곰돌이를 벗어났지만 곰돌이에게 던지는 경우는?
            // 물건을 곰돌이 위에서 잡고 놓았다. 던진다는 bool 형 변수를 만든다? 말이 안됨
            transform.SetParent(null);
            rb.useGravity = true;
            rb.isKinematic = false;
            isDropped = true;
        }
    }
}
