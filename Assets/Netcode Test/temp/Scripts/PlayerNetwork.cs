using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour // Instead of MonoBegaviour
{
    [SerializeField] private Transform spawnedObjectPrefab;
    private Transform spawnedObjectTransform;

    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
        new MyCustomData {
            _int = 56,
            _bool = true,
        }
        , NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    // WritePermission: only owner can change the variable.
    // We can use only value types. ex) int, float, enum, ...

    public override void OnNetworkSpawn() {
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) => {
            Debug.Log(OwnerClientId + "; " + newValue._int + "; " + newValue._bool + "; " + newValue.message);
        };
    }

    public struct MyCustomData : INetworkSerializable {
        public int _int;
        public bool _bool;
        public FixedString128Bytes message; // in collection (using Unity.Collections;)

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    }

    private void Update() {
        // Debug.Log(OwnerClientID + "; randomNumber: " + randomNumber.Value);

        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.T)){
            // only the server can spawn the network object, so use RPC
            spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);

            //// from the Server to the Client
            //// only sent the clientRpc to a specific client (with ID = 1)
            // TestClientRpc(new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> {1}}});
            
            //// from the Client to the Server
            // TestServerRpc(new ServerRpcParams());
            
            //// serverRPC: ref types are available
            // TestServerRpc("Like this video!");
            
            /*
            randomNumber.Value = new MyCustomData {
                _int = 10,
                _bool = false,
                message = "All your base are belong to us!"
            };*/
        }

        if (Input.GetKeyDown(KeyCode.Y)){
            Destroy(spawnedObjectTransform.gameObject);
        }

        Vector3 moveDir = new Vector3(0,0,0);

        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;
        
        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime; 
    }

    [ServerRpc]
    private void TestServerRpc(ServerRpcParams serverRpcParams) { // name must end with serverRPC
        Debug.Log("TestServerRpc " + OwnerClientId + "; " + serverRpcParams.Receive.SenderClientId);
    }
    // private void TestServerRpc(string message) { // name must end with serverRPC
    //     Debug.Log("TestServerRpc " + OwnerClientId + "; " + message);
    // }

    [ClientRpc]
    private void TestClientRpc(ClientRpcParams clientRpcParams) {
        Debug.Log("TestClientRpc");
    }
}
