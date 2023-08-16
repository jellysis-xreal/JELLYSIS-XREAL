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
                    Debug.Log(pourdetector.color.r+" "+pourdetector.color.g+" "+pourdetector.color.b);
                    
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
