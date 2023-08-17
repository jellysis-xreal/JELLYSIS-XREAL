using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using EnumTypes;

public class LiquidCollider : MonoBehaviour
{
    public BearManager bearManager;
    public GameObject target;
    public GuestBear bear;
    [SerializeField] private PourDetector pourdetector;
    public bool isDecorated = false;
    private float currenttime = 0;
    public BearColorType bearColorType;
    public Color color;
    
    public PuddingSpawn puddingSpawn1;
    public PuddingSpawn puddingSpawn2;
    public PuddingSpawn puddingSpawn3;
    public PuddingSpawn puddingSpawn4;
    
    Transform transform1;
    // Start is called before the first frame update
    void Start()
    {
        currenttime = 0;
    }

    // Update is called once per frame

    private bool TryGetGuestBear()
    {
        
        if (transform1.parent.TryGetComponent<GuestBear>(out GuestBear guestBear))
        {
            bear = guestBear;
            return true;
        }
        else
        {
            transform1 = transform1.parent;
            return false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            transform1 = other.transform;
            while (!TryGetGuestBear())
            {
                continue;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == target)
        {
            pourdetector = other.gameObject.GetComponent<PourDetector>();
            if (pourdetector.isPouring)
            {
                currenttime += Time.deltaTime;
                if (currenttime <= 2 && !isDecorated)
                {
                    // 컬러 변경 코드
                    // pourdetector.color 읽어올 수 있음.
                    Debug.Log("Color code : " + pourdetector.color.r + ", " + pourdetector.color.g + ", " + pourdetector.color.b);

                    color = pourdetector.color;
                    bearColorType = pourdetector.bearColorType;
                    Debug.Log(bearColorType);
                    
                    switch (bearColorType)
                    {
                        case (BearColorType.Purple):// Purple color
                            bear.ChangeBaseColor(BearColorType.Purple);
                            break;
                        case (BearColorType.Orange):// Orange color
                            Debug.Log("주황곰 변경");
                            bear.ChangeBaseColor(BearColorType.Orange);
                            break;
                        case (BearColorType.Pink):// Pink color
                            bear.ChangeBaseColor(BearColorType.Pink);
                            break;
                        case (BearColorType.Green):// Green color
                            bear.ChangeBaseColor(BearColorType.Green);
                            break;
                        case (BearColorType.PastelBlue):// Sky blue color
                            bear.ChangeBaseColor(BearColorType.PastelBlue);
                            break;
                        case (BearColorType.PastelYellow):// Pastel yellow color
                            bear.ChangeBaseColor(BearColorType.PastelYellow);
                            break;
                        case (BearColorType.Red):// red color
                            bear.ChangeBaseColor(BearColorType.Red);
                            break;
                        case (BearColorType.Blue):// blue color
                            bear.ChangeBaseColor(BearColorType.Blue);
                            break;
                        case (BearColorType.Yellow):// yellow color 
                            bear.ChangeBaseColor(BearColorType.Yellow);
                            Debug.Log("노랑곰 변경");
                            break;
                        case (BearColorType.White):// white color
                            bear.ChangeBaseColor(BearColorType.White);
                            break;
                    }
                    isDecorated = true;
                    StartCoroutine(ResetPuddingTransform());
                    /*1,0,0 -> red
                    bear.ChangeBaseColor(bearManager.BaseColorList[(int)bearManager.BearColorType.Red]);
                    1,1,1 -> white
                    */

                    // r, g, b에 해당하는 값 읽고 case (1,0,0)이면
                    // 아래와 같이 ChangeBaseColor(빨간색) 호출
                    // bear.ChangeBaseColor(bearManager.BaseColorList[(int)bearManager.BearColorType.Red]);
                    
                }
                else
                {
                    Debug.Log("�� �� �����ϱ�");
                    //
                }
            }

            
        }
    }
    IEnumerator ResetPuddingTransform()
    {
        yield return new WaitForSecondsRealtime(5f);
        currenttime = 0f;
        isDecorated = false;
        // 플레이어가 잡으면 컵을 잡으면 실행
        puddingSpawn1.puddingReset = true;
        puddingSpawn2.puddingReset = true;
        puddingSpawn3.puddingReset = true;
        puddingSpawn4.puddingReset = true;
        StopCoroutine(ResetPuddingTransform());
    }
}


