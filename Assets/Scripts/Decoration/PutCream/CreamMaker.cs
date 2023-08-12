using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CreamMaker : MonoBehaviour
{
    [Header("Cream")]
    [SerializeField] private GameObject creamPrefab;
    [SerializeField] private float scaleSpeed;
    private Vector3 _remainNewCreamScale;
    [SerializeField] private Transform injectionPusherTransform;
    [SerializeField] private Transform remainCreamTransform;
    [SerializeField] private bool isScalingUp;
    public bool isPressing = false;
    private bool _isTouching = false;
    
    [FormerlySerializedAs("can")] public bool timeOK;
    public float _timer = 0f;
    public float delayTime = 3f;
    
    public GameObject indicator;
    [SerializeField] private GameObject _recentMakedCream;
    [SerializeField] private float creamCapacity = 100f;
    [SerializeField] private float eachCreamMinimum = 10f;
    [SerializeField] private float remainCreamCapacity;
    public InjectScaleController injectScaleController;

    private void Start()
    {
        //remainCreamCapacity = creamCapacity;
        _remainNewCreamScale = remainCreamTransform.localScale;
    }

    void Update()
    {
        // 손에 물체가 있으면 현제 가리키는 위치를 보여줌. 이건 양쪽 컨트롤러가 가리키는 위치에 따라 달라짐.
        // 주사기에 용량이 있다면 CreamRenderer 보여줌.
        if (true)
        {
            ShowDetectedArea();
        }
        
        // 버튼이 눌러지는 세기에 따라서 생크림 나오는 속도 다르게 함. 이건 나중에
        // '버튼이 눌리는 동안'로 조건 변경
        if (isScalingUp && _recentMakedCream != null && 
            isPressing && _isTouching)
        {
            ScaleUp();
        }
        SetRemainCreamScale();
    }

    private void SetRemainCreamScale()
    {
        // 최소 스케일 0, 최대 스케일 : 처음 스케일
        // injec_Cream 스케일 ->  남아있는 용량 VS 주사기 위치에 맞춰 이동
        // 일단 주사기 위치에 맞춰 스케일 조절
        // push localPosition.y 최대 0.07 최소 0.01에 따라 injec_Cream Scale.y를 최대 3에서 0으로
        float percentage = injectionPusherTransform.localPosition.y / (0.07f - 0.01f);
        if (percentage > 1 || percentage <0) return;
            _remainNewCreamScale.y = (3f - 0f) * percentage;
        remainCreamTransform.localScale = _remainNewCreamScale;
    }
    // 주사기 앞부분 콜라이더 
    public void ShowDetectedArea()
    {
        Ray ray = new Ray(transform.position - transform.up *0.05f, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log("1" + hit.transform.name);
            if (hit.transform.gameObject.layer == 11) // Basic bear 오브젝트 레이어 11
            {
                //Debug.Log("2");
                _recentMakedCream = null;
                indicator.SetActive(true);
                indicator.transform.position = hit.point;
                indicator.transform.forward = hit.normal;
                // '버튼이 눌리면 1번'로 조건 변경
                if(isPressing && _recentMakedCream == null && _isTouching)
                {
                    if (remainCreamCapacity > eachCreamMinimum)
                    {
                        SetCreamNormalVector(hit);
                    }
                }
                // '버튼이 눌릴 때'로 조건 변경
                if (!isPressing)
                {
                    isScalingUp = false;
                }
            }
            else if (hit.transform.tag == "Cream") // Cream 감지, Cream Scale Up!
            {
                // material 빨갛게 알려줌.?
                _recentMakedCream = hit.transform.gameObject;
                if (isPressing)
                    isScalingUp = true;
            }
            else
            {
                _recentMakedCream = null;
                indicator.SetActive(false);
            }
        }else
        {
            //indicator.SetActive(false);
        }
    }
    
    
    public void SetCreamNormalVector(RaycastHit hit)
    {
        Debug.Log("Set Cream Normal Vector");
        _recentMakedCream = Instantiate(creamPrefab, hit.point,Quaternion.identity);
        _recentMakedCream.transform.forward = hit.normal;
        _recentMakedCream.transform.SetParent(hit.transform);
        isScalingUp = true;

        remainCreamCapacity -= 10f;
    }
    public void ScaleUp()
    {
        if (remainCreamCapacity > 0)//TimeCheck())
        {
            Debug.Log("Scale Up");
            float scaleFactor = 1.0f + scaleSpeed * Time.deltaTime;
            _recentMakedCream.transform.localScale *= scaleFactor;
            remainCreamCapacity -= Time.deltaTime;
        }
    }

    bool TimeCheck()
    {
        if (_timer >= delayTime)
        {
            return true;
        }
        return false;
    }
    private void OnDrawGizmos()
    {
        if (true)
        {
            Gizmos.color = Color.red;
            Vector3 startPosition = transform.position - transform.up *0.05f;
            Vector3 endPosition = transform.position  -transform.up * 5f;
            Gizmos.DrawLine(startPosition, endPosition);    
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // preparedCream에 닿으면 충전, 곰돌이에 닿으면 꾸미기 가능
        // Ray Casting할 때 normal Vector값 저장해놓음.
        // 실시간으로 생성 모양 위치를 알고 있기 때문.
        
        if (other.CompareTag("PreparedCream"))
        {
            remainCreamCapacity = creamCapacity;
        }
        // 닿았을 때 생성 가능하도록
        if (other.gameObject.layer == 11)
        {
            _isTouching = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Trigger Exit에 생성 불가능한 상태로 변경
        if (other.gameObject.layer == 11)
        {
            _isTouching = false;
        }
    }
}
