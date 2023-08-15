using System;
using System.Collections;
using System.Collections.Generic;
using EnumTypes;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("Stage Setting")] public StageState state = StageState.InLobby;
    [SerializeField] private StageState before_state;
    private int MaxPlayer = 4;
    public int DecorateTime = 20;

    [Header("Bool")] private bool IsStateUpdate = false;
    private bool CanStartStage = false;

    [Header("Player")] [SerializeField] private Transform[] transforms;
    [SerializeField] private List<GameObject> players = new List<GameObject>();

    public TableEventManager tableEventManager;
    public TimerUpdater timerUpdater;
    private WaitForSeconds tableRotationTime;
    private BearManager bearManager;
    
    private void Start()
    {
        before_state = state;
        tableRotationTime = new WaitForSeconds(5f);

        bearManager = GameManager.Bear;
        state = StageState.BeforeStageStart;
    }

    private IEnumerator SetTheStageCoroutine()
    {
        yield return new WaitForSeconds(2.0f); // 2초 대기

        while (!CanStartStage)
        {
            GameObject[] find_player = GameObject.FindGameObjectsWithTag("Player");
            foreach (var bear in find_player)
            {
                if (!players.Contains(bear))
                    players.Add(bear);
            }

            // 4명의 Player가 모두 입장하면, 게임을 시작할 수 있습니다
            if (players.Count >= MaxPlayer)
            {
                for (int i = 0; i < MaxPlayer; i++)
                {
                    players[i].transform.position = transforms[i].position;
                    players[i].transform.rotation = transforms[i].rotation;
                }

                state = StageState.StageStart;
                CanStartStage = true;
            }
            else
                Debug.Log("<-----------[TEST] 아직 플레이어가 모두 입장하지 않았습니다------------>");

            yield return new WaitForSeconds(1.0f);
        }
    }

    private void SetPairPlayer()
    {
        bearManager.SetPlayerList_();
    }

    private void StartAutoDeco()
    {
        bearManager.AutoDecorate();
    }
    
    // 스테이지는 게임이 끝나기 까지 한 바퀴를 의미함.
    // 라운드는 곰돌이 1마리 꾸미는 시간이 종료되면 호출됨. 
    [ContextMenu("StartGame")]
    private void StartGame()
    {
        Debug.Log("Start Game!");
        // stage가 시작하면, Decorate time이 주어집니다
        state = StageState.Decorate;

        timerUpdater.ResetAllTimer();
    }

    [ContextMenu("Start StageRoutine")]
    public void StartStageRoutine()
    {
        // Debug.Log("StageRoutine Start!");
        StartCoroutine(StageRoutine());
    }

    IEnumerator StageRoutine()
    {
        // Table 돌리기 완료되면 다시 타이머 시작
        tableEventManager.RaiseEvent();
        state = StageState.RotateLP;
        yield return tableRotationTime;

        // Decorate time이 주어집니다
        state = StageState.Decorate;
        timerUpdater.ResetAllTimer();

    }

    private void HandleState(StageState currentState)
    {
        switch (currentState)
        {
            case StageState.BeforeStageStart:
                Debug.Log("[TEST] <-----게임이 아직 시작하지 않았습니다----->");
                StartCoroutine(SetTheStageCoroutine());
                break;

            case StageState.StageStart:
                Debug.Log("[TEST] <-----게임이 시작됩니다----->");
                bearManager.Init();
                StartGame();
                break;

            case StageState.RotateLP:
                Debug.Log("[TEST] <-----LP 돌아갑니다----->");
                // TODO: Auto 곰돌이 춤추는 애니메이션이라던지?
                break;

            case StageState.Decorate:
                Debug.Log("[TEST] <-----Let's Decorate----->");
                bearManager.UpdatePairPlayer();
                Invoke("SetPairPlayer", 3.0f);
                Invoke("StartAutoDeco", 5.0f);
                //StartCoroutine(bearManager.SetPairPlayerList());
                //StartCoroutine(bearManager.AutoDecorate());
                break;

            case StageState.DoPosing:
                break;
            
            default:
                break;
        }
    }
    
    void Update()
    {
        if (state != before_state)
        {
            before_state = state;
            HandleState(state);
        }
    }
}
