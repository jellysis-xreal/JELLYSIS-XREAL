using System.Collections;
using System.Collections.Generic;
using EnumTypes;
using Unity.Netcode;
using UnityEngine;

/* [ Game Manager ]
 * 게임 내에 정의될 모든 객체들을 Child로 갖는 하나의 관리자 클래스를 정의합니다
 * 해당 클래스는 Scene의 가장 루트 GameObject로, 단 하나만 미리 인스턴싱 해두도록 한다
 *
 * Scene 전환에 걸쳐서도 유지되어야 하는 데이터들을 관리함
*/

public class GameManager : NetworkBehaviour
{
    [Header("Field of view")]
    [SerializeField] private GameType CurrentGameMode = GameType.Multi;
    
    public StageState CurrentState;
    
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
             return null;
            }

            return instance;
        }
    }

    #region Managers
    
    [SerializeField] private BearManager  _bear;
    public static BearManager Bear { get { return Instance._bear; } } // MonoBehaviour를 상속 받고 있기 때문에 Scene에서 직접 할당 필요
    
    [SerializeField] private StageController _stage;
    public static StageController Stage { get { return Instance._stage; } }// MonoBehaviour를 상속 받고 있기 때문에 Scene에서 직접 할당 필요
    
    // [SerializeField] private CustomSceneManager _scene;
    // public static CustomSceneManager Scene { get { return Instance._scene; } }// MonoBehaviour를 상속 받고 있기 때문에 Scene에서 직접 할당 필요
    //
    // [SerializeField] private PlayerManager _player = new PlayerManager();
    // public static PlayerManager Player { get { return Instance._player; } }
    //
    // [SerializeField] private SoundManager _sound = new SoundManager();
    // public static SoundManager Sound {get{return Instance._sound; } }

    #endregion

    void Start()
    {
        Init();
        
        Test();
    }

    public void Init()
    {
        if (instance == null)
        {
            GameObject obj = GameObject.Find("Game Manager");

             if (obj == null)
             {
                  //obj = new GameObject { name = "Game Manager" };
                  //obj.AddComponent<GameManager>();
                  obj = Resources.Load<GameObject>("Prefabs/Game Manager");
                  obj.name = "Game Manager";
                  Instantiate(obj);
             }

             DontDestroyOnLoad(obj);
             instance = obj.GetComponent<GameManager>();

             // 하위 Managers Init();
             Stage.Init();
             Bear.Init();
             // Sound.Init();
             // Scene.Init();
             //          
             // Player.Init();
        }
        else
        {
             Debug.LogWarning("GameManager instance isn't null, Destroy GameManager");

             Destroy(this.gameObject);
        }
    }

    private void Test()
    {
         Debug.Log("<<-------TEST------->>");
         // 이 밑으로 진행할 Test 코드를 입력한 후, Start 함수에 가서 Test의 주석 처리를 해제하면 됩니다.
         //_bear.Test();
    }
}
