using EnumTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTest : MonoBehaviour
{
    private bool isRotating = false;
    private float rotationSpeed = 30f; // ȸ�� �ӵ� (��/��)

    private BearManager bearManager; // BearManager ��ü

    // ȸ�� ���� �Լ�
    public void StartRotation()
    {
        isRotating = true;

        bearManager = FindObjectOfType<BearManager>(); // BearManager ��ü ã��

        if (bearManager != null)
        {
            List<GameObject> guestBears = bearManager.GuestBears; // GuestBears ����Ʈ ����
            List<Texture2DArray> baseColorList = bearManager.BaseColorList; // BaseColorList ����Ʈ ����
            guestBears[0].GetComponent<GuestBear>().ChangeBaseColor(BearColorType.Red);
            Debug.Log("���� �� ����");
        }
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
