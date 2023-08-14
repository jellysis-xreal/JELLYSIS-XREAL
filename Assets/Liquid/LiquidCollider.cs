using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidCollider : MonoBehaviour
{
    public GameObject target;
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
                    Debug.Log("와인 붓는중");
                } else
                {
                    Debug.Log("곰 색 변경하기");
                    //
                }
            }
        }
    }
}
