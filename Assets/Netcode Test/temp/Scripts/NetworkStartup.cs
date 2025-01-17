using Unity.Netcode;
using UnityEngine;

public class NetworkStartup : MonoBehaviour
{
    void Start()
    {
        if (NetworkManagerUI.Instance.InitializeAsHost)
        {
            Debug.Log("[TEST] Start as a Host");
            Debug.Log(NetworkManagerUI.Instance.InitializeAsHost);
            NetworkManager.Singleton.StartHost();
            Debug.Log("[TEST] Start local client ID " + NetworkManager.Singleton.LocalClientId);
        }
        else
        {
            Debug.Log("[TEST] Start as a Client");
            Debug.Log(NetworkManagerUI.Instance.InitializeAsHost);
            NetworkManager.Singleton.StartClient();
            Debug.Log("[TEST] Start local client ID " + NetworkManager.Singleton.LocalClientId);
        }
    }
}