using EnumTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTest : MonoBehaviour
{
    private bool isRotating = false;
    private float rotationSpeed = 30f; // 회전 속도 (도/초)

    private BearManager bearManager; // BearManager 객체

    // 회전 시작 함수
    public void StartRotation()
    {
        isRotating = true;

        bearManager = FindObjectOfType<BearManager>(); // BearManager 객체 찾기

        if (bearManager != null)
        {
            List<GameObject> guestBears = bearManager.GuestBears; // GuestBears 리스트 참조
            List<Texture2DArray> baseColorList = bearManager.BaseColorList; // BaseColorList 리스트 참조
            guestBears[0].GetComponent<GuestBear>().ChangeBaseColor(baseColorList[(int)BearColorType.Red]);
            Debug.Log("빨강 곰 변경");
        }
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
