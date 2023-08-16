using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidCollider : MonoBehaviour
{
    public BearManager bearManager;
    public GameObject target;
    public GuestBear bear;
    private PourDetector pourdetector;
    private float currenttime = 0;

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
                    Debug.Log(pourdetector.color.r + " " + pourdetector.color.g + " " + pourdetector.color.b);

                    color = pourdetector.color;
                    switch (pourdetector.color.r, pourdetector.color.g, pourdetector.color.b)
                    {
                        case (1.0f, 0.0f, 1.0f):// Purple color
                            //bear.ChangeBaseColor(bearManager.BaseColorList[(int)bearManager.BearColorType.Purple]);
                            break;
                        case (1.0f, 0.5f, 0.0f):// Orange color
                            //bear.ChangeBaseColor(bearManager.BaseColorList[(int)bearManager.BearColorType.Orange]);
                            break;
                        case (1.0f, 0.75f, 0.75f):// Pink color
                            //bear.ChangeBaseColor(bearManager.BaseColorList[(int)bearManager.BearColorType.Pink]);
                            break;
                        case (0.0f, 1.0f, 0.0f):// Green color
                            //bear.ChangeBaseColor(bearManager.BaseColorList[(int)bearManager.BearColorType.Green]);
                            break;
                        case (0.0f, 1.0f, 1.0f):// Sky blue color
                            //bear.ChangeBaseColor(bearManager.BaseColorList[(int)bearManager.BearColorType.Skyblue]);
                            break;
                        case (1.0f, 1.0f, 0.0f):// Pastel yellow color
                            //bear.ChangeBaseColor(bearManager.BaseColorList[(int)bearManager.BearColorType.Pastelgreen]);
                            break;
                        case (1.0f, 0.0f, 0.0f):// red color
                            //bear.ChangeBaseColor(bearManager.BaseColorList[(int)bearManager.BearColorType.Red]);
                            break;
                        case (0.0f, 0.0f, 1.0f):// blue color
                            //bear.ChangeBaseColor(bearManager.BaseColorList[(int)bearManager.BearColorType.Blue]);
                            break;
                        case (1.0f, 1.0f, 0.5f):// yellow color 
                            //bear.ChangeBaseColor(bearManager.BaseColorList[(int)bearManager.BearColorType.Pastelyellow]);
                            break;
                        case (1.0f, 1.0f, 1.0f):// white color
                            //bear.ChangeBaseColor(bearManager.BaseColorList[(int)bearManager.BearColorType.White]);
                            break;
                    }

                    
                    
                    /*1,0,0 -> red
                    bear.ChangeBaseColor(bearManager.BaseColorList[(int)bearManager.BearColorType.Red]);
                    1,1,1 -> white
                    */
                        
                    // r, g, b에 해당하는 값 읽고 case (1,0,0)이면
                    // 아래와 같이 ChangeBaseColor(빨간색) 호출
                    // bear.ChangeBaseColor(bearManager.BaseColorList[(int)bearManager.BearColorType.Red]);
                    
                } else
                {
                    Debug.Log("�� �� �����ϱ�");
                    //
                }
            }
        }
    }
}
