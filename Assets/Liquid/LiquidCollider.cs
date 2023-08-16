using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class LiquidCollider : MonoBehaviour
{
    public BearManager bearManager;
    public GameObject target;
    public GuestBear bear;
    [SerializeField] private PourDetector pourdetector;
    private float currenttime = 0;
    public BearColorType bearColorType;
    public Color color;
    // Start is called before the first frame update
    void Start()
    {
        currenttime = 0;
    }

    // Update is called once per frame
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == target)
        {
            pourdetector = other.gameObject.GetComponent<PourDetector>();
            if (pourdetector.isPouring)
            {
                currenttime += Time.deltaTime;
                if (currenttime <= 2)
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
}


