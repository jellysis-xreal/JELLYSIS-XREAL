using System;
using System.Collections;
using System.Collections.Generic;
using EnumTypes;
using UnityEngine;

public class StageController : MonoBehaviour
{

    [Header("Stage Setting")] 
    private StageState state;
    public int DecorateTime = 20;


    [Header("Bool")] 
    private bool IsStateUpdate = false;

    private float timer;

    public void Init()
    {
        state = GameManager.Instance.CurrentState;
        timer = 0.0f;
        
    }
    
    public void BeforeStageStart()
    {
        // TODO: stage 시작 전, 기본 애니메이션 재생
        // GameManager.Bear.Init();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if ( IsStateUpdate && timer > DecorateTime)
        {
            switch (state)
            {
                case StageState.BeforeStageStart:
                    break;

                case StageState.RotateLP:
                    Debug.Log("<-----------[Stage] LP 돌아갑니다------------>");
                    // TODO: 돌아가는 시간 정해야함
                    break;

                case StageState.Decorate:
                    Debug.Log("<-----------[Stage] Let's Decorate------------>");
                    break;

                case StageState.DoPosing:
                    break;
                
                default:
                    IsStateUpdate = false; // Set to false to stop executing
                    break;
            }

            IsStateUpdate = false;
        }
    }
}
