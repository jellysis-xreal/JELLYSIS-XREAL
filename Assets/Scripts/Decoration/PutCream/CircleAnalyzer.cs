using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAnalyzer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float maxErrorDistance = 0.05f;

    public List<Vector3> circlePoints = new List<Vector3>();

    private bool _onCalculate = false;
    private WaitForSeconds _waitTime;
    void Start()
    {
        _waitTime = new WaitForSeconds(0.5f);
        StartCoroutine(AddPoint());
    }
    // 지금 문제 점과 점 사이의 거리가 너무 작아 0으로 값이 나옴. 오차 크게 하고 
    // 그려진 원이 실제 원과 유사한지 판단하는 메서드
    private bool IsApproximateCircle(List<Vector3> points)
    {
        // 간단하게 점들의 거리를 비교해서 판단., 최근 그려진 것들만 판단
        for (int i = points.Count- 1; i > 0 ; i--)
        {
            // 실제 컨트롤러에 적용시 Position 값이 소수점단위로 차이가 나서 일정량 크기 키움.
            double distance = (double)Vector3.Distance(points[i], points[i - 1]) * 100;            
            if (Mathf.Abs((float)distance - 1.0f) > maxErrorDistance)
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
            
            // 그려진 원의 궤적을 라인 렌더러를 통해 그림
            lineRenderer.positionCount = circlePoints.Count;
            lineRenderer.SetPositions(circlePoints.ToArray());
            
            bool isCircle = IsApproximateCircle(circlePoints);
            Debug.Log("Is Circle : " + isCircle);
            yield return _waitTime;
        }
    }
}
