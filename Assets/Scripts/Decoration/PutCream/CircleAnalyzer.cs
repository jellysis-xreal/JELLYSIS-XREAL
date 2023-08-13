using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAnalyzer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float maxErrorDistance = 0.05f;
    [SerializeField] private bool isTriggeringBowl = false;
    [SerializeField] private bool isCorrectWhipping = false;
    public List<Vector3> circlePoints = new List<Vector3>();
    private bool _onCalculate = false;
    private WaitForSeconds _waitTime;

    [SerializeField] private ScaleTest scaleTest;
    private Coroutine whippringCoroutine;
    void Start()
    {
        _waitTime = new WaitForSeconds(0.5f);
    }
    // 지금 문제 점과 점 사이의 거리가 너무 작아 0으로 값이 나옴. 오차 크게 하고 
    // 그려진 원이 실제 원과 유사한지 판단하는 메서드
    private bool IsApproximateCircle(List<Vector3> points)
    {
        Debug.Log(points.Count);
        // 간단하게 점들의 거리를 비교해서 판단., 최근 그려진 것들만 판단   크기 1인 상태로 들어오자마자 
        for (int i = points.Count-1; i >= 0 ; i--)
        {
            if (i == 0)
            {
                Debug.Log("return false");
                return false;
            }
            // 실제 컨트롤러에 적용시 Position 값이 소수점단위로 차이가 나서 일정량 크기 키움.
            Debug.Log("@i is "+i);
            float distance = Vector3.Distance(points[i], points[i - 1]) * 100;
            if (distance < 1f || Mathf.Abs(distance - 1.0f) > maxErrorDistance)
            {
                return false;
            }
            break;
        }
        return true;
    }

    IEnumerator AddPoint()
    {
        while (true)
        {
            circlePoints.Add(new Vector3(transform.position.x, transform.position.y, transform.position.z));
            Debug.Log("Add point");
            /*// 그려진 원의 궤적을 라인 렌더러를 통해 그림
            lineRenderer.positionCount = circlePoints.Count;
            lineRenderer.SetPositions(circlePoints.ToArray());*/     
            isCorrectWhipping = IsApproximateCircle(circlePoints) && isTriggeringBowl;
            if(isCorrectWhipping) scaleTest.ScaleUp();
            else if(scaleTest != null) scaleTest.ScaleDown();
            yield return _waitTime;
        }
    }

    public void WhipSelectEvent()
    {
        // 잡은 순간부터 코루틴 돌림. 코루틴 안에서 실시간으로 볼과 트리거 중인가 체크해야 할까
        StartCoroutine(AddPoint());
    }

    public void WhipUnselectEvent()
    {
        // 놓으면 코루틴 종료
        circlePoints.Clear();
        StopCoroutine(AddPoint());
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CreamBowl"))
        {
            isTriggeringBowl = true;
            scaleTest = other.GetComponent<ScaleTest>();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CreamBowl"))
        {
            isTriggeringBowl = false;
            scaleTest.ScaleDown();
            scaleTest = null;
        }
    }
}
