using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{

    // [SerializeField] private Button serverBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    
    [SerializeField] public string DefaultMainMenu = "Multi_MainScene";

    public static NetworkManagerUI Instance;

    public bool InitializeAsHost { get; set; }

    private void Awake(){
        // serverBtn.onClick.AddListener(() => {
        //     // InitializeAsHost = true;
        //     SceneManager.LoadScene(DefaultMainMenu);
        //     // NetworkManager.Singleton.StartServer();
        // });
        hostBtn.onClick.AddListener(() => {
            InitializeAsHost = true;
            SceneManager.LoadScene(DefaultMainMenu);
            // NetworkManager.Singleton.StartHost();
        });
        clientBtn.onClick.AddListener(() => {
            InitializeAsHost = false;
            SceneManager.LoadScene(DefaultMainMenu);
            // NetworkManager.Singleton.StartClient();
        });
    }

    void Start() {
        if (Instance != null) {
            Destroy(this);
        }
        else {
            Instance = this;
        }
        
        DontDestroyOnLoad(this);
    }
}
