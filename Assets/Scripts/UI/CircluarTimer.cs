using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CircluarTimer : MonoBehaviour
{
    public Slider roundslider;
    public bool isStarted = false;
    [SerializeField] private float recentRemainTime;
    [SerializeField] private StageManager stageManager;
    // Start is called before the first frame update
    void Start()
    {
        /*roundslider.maxValue =GameManager.Bear.playTimeEachBear;
        roundTime = GameManager.Bear.playTimeEachBear;*/
        roundslider.maxValue = 30f;
        recentRemainTime = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarted)
        {
            UpdateTime();
        }
    }

    public void ReadyToNextBear()
    {
        // Host에 준비됐다고 알림
    }
    public void UpdateTime()
    {
        recentRemainTime -= Time.deltaTime;
        roundslider.value = recentRemainTime;
        if (recentRemainTime <= 0)
        {
            isStarted = false;
            if(stageManager != null) stageManager.StartStageRoutine();
        }
    }
    [ContextMenu("ResetTimer")]
    public void ResetTimer()
    {
        // Host -> 각 오브젝트에 ResetTimer 호출하도록
        recentRemainTime = 30f;
        isStarted = true;
    }
}
