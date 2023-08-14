using System.Collections;
using System.Collections.Generic;
using EnumTypes;
using UnityEngine;
using Unity.Netcode;

public class GameNetworkManager : NetworkBehaviour
{
    //[SyncVar(hook = nameof(OnGameStateChange))]
    private StageState _stageState = StageState.BeforeStageStart;
    
    public StageState StageState
    {
        get { return _stageState; }
        set { _stageState = value; }
    }
    
    
    private void OnGameStateChange(StageState newState)
    {
        // Handle the gameState change here (update UI, trigger actions, etc.).
    }
}
