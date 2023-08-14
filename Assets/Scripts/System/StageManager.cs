using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public TableEventManager tableEventManager;
    public TimerUpdater timerUpdater;
    private WaitForSeconds tableRotationTime;
    
    private void Start()
    {
        tableRotationTime = new WaitForSeconds(5f);
    }

    // 스테이지는 게임이 끝나기 까지 한 바퀴를 의미함.
    // 라운드는 곰돌이 1마리 꾸미는 시간이 종료되면 호출됨. 
    [ContextMenu("StartGame")]
    private void StartGame()
    {
        Debug.Log("Start Game!");
        timerUpdater.ResetAllTimer();
    }
    
    [ContextMenu("Start StageRoutine")]
    public void StartStageRoutine()
    {
        Debug.Log("StageRoutine Start!");
        StartCoroutine(StageRoutine());
    }
    IEnumerator StageRoutine()
    {
        // Table 돌리기 완료되면 다시 타이머 시작
        tableEventManager.RaiseEvent();
        yield return tableRotationTime;
        timerUpdater.ResetAllTimer();
    }
}
