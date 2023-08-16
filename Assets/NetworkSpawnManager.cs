using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class NetworkSpawnManager : NetworkBehaviour
{
    [SerializeField] NetworkObject injectionPrefab, _injection;
    ulong clientId = 1;

    // Start is called before the first frame update
    void Start()
    {
        if (IsServer)
        {
            Vector3 spawnPoint = new Vector3(1f, 1f, 0.75f);
            _injection = Instantiate(injectionPrefab, spawnPoint, Quaternion.identity);
            _injection.GetComponent<NetworkObject>().Spawn(true); //.SpawnWithOwnership(clientId)
            _injection.ChangeOwnership(clientId);
        }
    }

    void Update()
    {
    }
}
