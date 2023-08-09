using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkSpawnObject : NetworkBehaviour
{
    ulong PlayerClientID = 0;

    public void OnSelectGrabbable(SelectEnterEventArgs eventArgs)
    {
        PlayerClientID = NetworkManager.Singleton.LocalClientId;
        Debug.Log("[TEST] Grabbed!");
        if ((IsClient && !IsOwner))
        {
            NetworkObject networkObjectSelected = eventArgs.interactableObject.transform.GetComponent<NetworkObject>();
            Debug.Log("[TEST] ID: " + PlayerClientID+ " Client selected the "+ networkObjectSelected);
            if (networkObjectSelected != null)
                RequestGrabbableOwnershipServerRpc(PlayerClientID, networkObjectSelected);
            Debug.Log("[TEST] This Object's owner ID: " + OwnerClientId);
        }
        if (IsServer) {
            Debug.Log("[TEST] Server");
        }
    }

    public void OnSelectExitGrabbable(SelectExitEventArgs eventArgs)
    {
        PlayerClientID = NetworkManager.Singleton.LocalClientId;
        Debug.Log("[TEST] Release!!");
        if ((IsClient && !IsOwner))
        {
            NetworkObject networkObjectSelected = eventArgs.interactableObject.transform.GetComponent<NetworkObject>();
            Debug.Log("[TEST] ID: " + PlayerClientID+ " Client released the "+ networkObjectSelected);
            if (networkObjectSelected != null)
                RequestRetrunGrabbableOwnershipServerRpc(PlayerClientID, networkObjectSelected);
            Debug.Log("[TEST] This Object's owner ID: " + OwnerClientId);
        }
    }

    [ServerRpc]
    public void RequestGrabbableOwnershipServerRpc(ulong newOwnerClientId, NetworkObjectReference networkObjectReference)
    {
        Debug.Log("[TEST] Got client entering requests id: " + newOwnerClientId);
        if (networkObjectReference.TryGet(out NetworkObject networkObject))
        {
            networkObject.ChangeOwnership(newOwnerClientId);
        }
    }

    [ServerRpc]
    public void RequestRetrunGrabbableOwnershipServerRpc(ulong newOwnerClientId, NetworkObjectReference networkObjectReference)
    {
        Debug.Log("[TEST] Got client releasing requests id: " + newOwnerClientId);
        if (networkObjectReference.TryGet(out NetworkObject networkObject))
        {
            networkObject.RemoveOwnership();
        }
    }
}
