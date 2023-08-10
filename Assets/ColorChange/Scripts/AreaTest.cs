using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTest : MonoBehaviour
{
    private bool isRotating = false;
    private float rotationSpeed = 30f; // ȸ�� �ӵ� (��/��)

    // ȸ�� ���� �Լ�
    public void StartRotation()
    {
        isRotating = true;
    }

    // ȸ�� ���� �Լ�
    public void StopRotation()
    {
        isRotating = false;
    }

    private void Update()
    {
        if (isRotating)
        {
            // ȸ�� �ӵ��� ���� ������Ʈ ȸ��
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }
}
