using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTest : MonoBehaviour
{
    private bool isRotating = false;
    private float rotationSpeed = 30f; // 회전 속도 (도/초)

    // 회전 시작 함수
    public void StartRotation()
    {
        isRotating = true;
    }

    // 회전 중지 함수
    public void StopRotation()
    {
        isRotating = false;
    }

    private void Update()
    {
        if (isRotating)
        {
            // 회전 속도에 따라 오브젝트 회전
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }
}
