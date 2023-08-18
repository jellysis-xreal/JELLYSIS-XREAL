using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnumTypes;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Serialization;

public class StageManager : NetworkBehaviour
{
    [Header("Stage Setting")]
    public int MaxPlayer = 4;
    public int DecorateRound = 4;
    public DecorateType PlayerType1 = DecorateType.PutCream;
    public DecorateType PlayerType2 = DecorateType.Draw;
    private StageState curState = StageState.InLobby;
    private StageState beforeState;
    
    // CMS: state 초기화 - only server can edit this variable
    [SerializeField]
    public NetworkVariable<StageState> curState_Multi = new NetworkVariable<StageState>(StageState.InLobby, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<StageState> beforeState_Multi = new NetworkVariable<StageState>(StageState.InLobby, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    
    private bool IsStateUpdate = false;
    private bool CanStartStage = false;
    private bool SetFirst = false;

    [Header("Player Setting")] 
    [SerializeField] private Transform[] transforms;
    [SerializeField] private List<GameObject> players = new List<GameObject>();
    
    public TableEventManager tableEventManager;
    public TimerUpdater timerUpdater;
    private WaitForSeconds tableRotationTime;
    private BearManager bearManager;
    private int _currentRound = 1;
    
    [SerializeField] public NetworkObject injection, whisk, creamBowl;
    private ulong clientId = 0;
    
    private void Start()
    {
        //before_state = state;
        //if (IsServer) beforeState_Multi.Value = before_state;
        //tableRotationTime = new WaitForSeconds(5f);

        //bearManager = GameManager.Bear;
        
        //state = StageState.BeforeStageStart;
        //if (IsServer) curState_Multi.Value = state;
    }
    
    // CMS: network variable 변화한 값 업데이트
    public override void OnNetworkSpawn()
    {
        beforeState_Multi.OnValueChanged += OnBeforeStateValueChanged;
        curState_Multi.OnValueChanged += OnCurrentStateValueChanged;
    }

    private void OnBeforeStateValueChanged(StageState previousValue, StageState newValue)
    {
        // Debug.Log("[TEST] <Before changed> ---- before: " + previousValue + ", after: " + newValue);
    }
    
    private void OnCurrentStateValueChanged(StageState previousValue, StageState newValue)
    {
        // state는 바로바로 동기화 됨
        // 변화하면 수행하고 싶은 함수 넣으면 됨 ex.
        // Debug.Log("[TEST] <Current changed> ---- before: " + previousValue + ", after: " + newValue);
        // CanStartStage = true;
        // curState_Multi.Value 정의한 자료형이 나와서 그걸로 접근하면 됩니다

        if (curState_Multi.Value != beforeState_Multi.Value)
        {
            beforeState = curState;
            if (IsServer) beforeState_Multi.Value = beforeState;
            //HandleState(state);
            HandleState(curState_Multi.Value);
        }
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
                // curState = StageState.StageStart;
                // // CMS
                // if(IsServer) curState_Multi.Value = StageState.StageStart; // -> 클라이언트도 자동으로 바뀐다!

                // TODO: 여기 spawn code 수정
                /*
                if (IsServer)
                {
                    // // // UnityEditor.TransformWorldPlacementJSON:{"position":{"x":8.310199737548829,"y":0.39800000190734866,"z":-2.5239999294281008},"rotation":{"x":0.5,"y":0.5,"z":-0.5,"w":0.5},"scale":{"x":3.1062018871307375,"y":3.1062018871307375,"z":3.1062018871307375}}
                    // Vector3 spawnPoint = new Vector3(8.31f, 0.40f, -2.52f);
                    // NetworkObject _injection = Instantiate(injectionPrefab, spawnPoint, Quaternion.identity);
                    // _injection.GetComponent<NetworkObject>().Spawn(true); //.SpawnWithOwnership(clientId)
                    injection.ChangeOwnership(clientId);
                    whisk.ChangeOwnership(clientId);
                    creamBowl.ChangeOwnership(clientId);
                }
                */
                injection.ChangeOwnership(clientId);
                whisk.ChangeOwnership(clientId);
                creamBowl.ChangeOwnership(clientId);
                CanStartStage = true;
            }
            else
                Debug.Log(" [TEST] <-----------아직 플레이어가 모두 입장하지 않았습니다------------>");

            yield return new WaitForSeconds(1.0f);
        }
    }

    [ContextMenu("Start the Game")]
    [ServerRpc]
    public void AllPlayersReadyServerRpc()
    {
        SetPlayers();
        
        curState = StageState.StageStart;
        // CMS
        if (IsServer)
        { 
            ClientFunctionClientRpc();
            curState_Multi.Value = StageState.StageStart;
        }
    }

    [ClientRpc]
    void ClientFunctionClientRpc()
    {
        SetPlayers();
        
        Debug.Log("[TEST] <-----게임이 시작됩니다----->");
        bearManager.Init();
        StartGame();
    }

    [ServerRpc]
    public void GameIsEndServerRpc()
    {
        if (IsServer)
        {
            curState = StageState.DoPosing;
            curState_Multi.Value = curState;
            StopCoroutine(StageRoutine());
            timerUpdater.StopAllTimer();
            GameIsEndClientRpc();
        }
    }
    
    [ClientRpc]
    public void GameIsEndClientRpc()
    {
        if (IsClient)
        {
            StopCoroutine(StageRoutine());
            timerUpdater.StopAllTimer();
        }
    }
    
    private void SetPlayers()
    {
        if (IsClient)
        {
            GameObject[] find_player = GameObject.FindGameObjectsWithTag("Player");
            foreach (var bear in find_player)
            {
                if (!players.Contains(bear))
                    players.Add(bear);
            }
        }
        
        foreach (var bear in players)
        {
            PlayerBear temp = bear.GetComponent<PlayerBear>();
            
            if (temp.BearType == BearType.PlayerBear)
            {
                if (bear.GetComponent<NetworkObject>().OwnerClientId == 0)
                    temp.ChangeDecorateType(PlayerType1);
                
                else if (bear.GetComponent<NetworkObject>().OwnerClientId == 1)
                    temp.ChangeDecorateType(PlayerType2);
            }
            
            // 맡은 기능에 따라서 자리가 배정됩니다
            int index = (int)temp.GetDecorateType();
            bear.transform.position = transforms[index].position;
            bear.transform.rotation = transforms[index].rotation;
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
    private void StartGame()
    {
        Debug.Log("[TEST] Start Game!");
        _currentRound = 0;
        timerUpdater.ResetAllTimer();
        
        // stage가 시작하면, Decorate time이 주어집니다
        curState = StageState.Decorate;
        if (IsServer) curState_Multi.Value = curState;
    }
    
    public void StartStageRoutine()
    {
        // Debug.Log("StageRoutine Start!");
        StartCoroutine(StageRoutine());
    }

    IEnumerator StageRoutine()
    {
        // Table 돌리기 완료되면 다시 타이머 시작
        tableEventManager.RaiseEvent();
        curState = StageState.RotateLP;
        if (IsServer) curState_Multi.Value = curState;
        yield return tableRotationTime;

        // Decorate time이 주어집니다
        curState = StageState.Decorate;
        if (IsServer) curState_Multi.Value = curState;
        timerUpdater.ResetAllTimer();

    }

    private void HandleState(StageState currentState)
    {
        Debug.Log("[TEST] HandleState 실행 됨!");
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

                _currentRound++;
                if (_currentRound >= DecorateRound) GameIsEndServerRpc();

                //StartCoroutine(bearManager.SetPairPlayerList());
                //StartCoroutine(bearManager.AutoDecorate());
                break;

            case StageState.DoPosing:
                Debug.Log("[TEST] <-----Game이 종료되었습니다!----->");
                // TODO: Auto 곰돌이 엔딩 포즈
                // TODO: Ending BGM 
                break;
            
            default:
                break;
        }
    }
    
    void Update()
    {
        //if (state != before_state)
        if (!SetFirst)
        {
            beforeState = curState;
            if (IsServer) beforeState_Multi.Value = beforeState;
            
            tableRotationTime = new WaitForSeconds(5f);
            bearManager = GameManager.Bear;
            
            curState = StageState.BeforeStageStart;
            if (IsServer) curState_Multi.Value = curState;

            SetFirst = true;
        }
        
        if (Input.GetKeyDown(KeyCode.J))
        {
            AllPlayersReadyServerRpc();
        }
        // if (curState_Multi.Value != beforeState_Multi.Value)
        // {
        //     beforeState = curState;
        //     if (IsServer) beforeState_Multi.Value = beforeState;
        //     //HandleState(state);
        //     HandleState(curState_Multi.Value);
        // }
    }
}
