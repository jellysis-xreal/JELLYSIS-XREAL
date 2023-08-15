using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkPlayerRpcCall : NetworkBehaviour
{
    // public void OnSelectGrabbable(SelectEnterEventArgs eventArgs)
    // {
    //     Debug.Log("[TEST] Grabbed!");
    //     if (IsClient && IsOwner)
    //     {
    //         NetworkObject networkObjectSelected = eventArgs.interactableObject.transform.GetComponent<NetworkObject>();
    //         Debug.Log("[TEST] Client selected the "+ networkObjectSelected);
    //         if (networkObjectSelected != null)
    //             RequestGrabbableOwnershipServerRpc(OwnerClientId, networkObjectSelected);
    //     }
    // }

    // public void OnSelectExitGrabbable(SelectExitEventArgs eventArgs)
    // {
    //     Debug.Log("[TEST] Release!!");
    //     if (IsClient && IsOwner)
    //     {
    //         NetworkObject networkObjectSelected = eventArgs.interactableObject.transform.GetComponent<NetworkObject>();
    //         Debug.Log("[TEST] Client released the "+ networkObjectSelected);
    //         if (networkObjectSelected != null)
    //             RequestRetrunGrabbableOwnershipServerRpc(OwnerClientId, networkObjectSelected);
    //     }
    // }

    [ServerRpc]
    public void RequestGrabbableOwnershipServerRpc(ulong newOwnerClientId, NetworkObjectReference networkObjectReference)
    {
        Debug.Log("[TEST] Got client entering requests id: " + newOwnerClientId);
        if (networkObjectReference.TryGet(out NetworkObject networkObject))
        {
            networkObject.ChangeOwnership(newOwnerClientId);
            Debug.Log("[TEST] Enter: Now "+ networkObject + " ownerClientID: " + networkObject.OwnerClientId);
        }
    }

    [ServerRpc]
    public void RequestRetrunGrabbableOwnershipServerRpc(ulong newOwnerClientId, NetworkObjectReference networkObjectReference)
    {
        Debug.Log("[TEST] Got client releasing requests id: " + newOwnerClientId);
        if (networkObjectReference.TryGet(out NetworkObject networkObject))
        {
            networkObject.RemoveOwnership();
            Debug.Log("[TEST] Released: Now "+ networkObject + " ownerClientID: " + networkObject.OwnerClientId);
        }
    }

    [ServerRpc]
    public void RequestRemoveParentServerRpc(NetworkObjectReference childObjectReference, NetworkObjectReference parentObjectReference)
    {
        Debug.Log("[TEST] Got client remove parent requests");
        if (childObjectReference.TryGet(out NetworkObject childObject))
        {
            if (parentObjectReference.TryGet(out NetworkObject parentObject))
                childObject.TryRemoveParent(parentObject);
                Debug.Log("[TEST] Removed Parent");
        }
    }
    
    [ServerRpc]
    public void RequestSetParentServerRpc(NetworkObjectReference childObjectReference, NetworkObjectReference parentObjectReference)
    {
        Debug.Log("[TEST] Got client remove parent requests");
        if (childObjectReference.TryGet(out NetworkObject childObject))
        {
            if (parentObjectReference.TryGet(out NetworkObject parentObject))
                childObject.TrySetParent(parentObject);
                Debug.Log("[TEST] Set Parent");
        }
    }

    [ServerRpc]
    public void RequestUseGravityServerRpc(NetworkObjectReference networkObjectReference, bool _useGravity)
    {
        Debug.Log("[TEST] Got client use Gravity requests");
        if (networkObjectReference.TryGet(out NetworkObject networkObject))
        {
            Debug.Log("[TEST] networkObject.GetComponentInChildren<NetworkSyncObject>() = " + networkObject.GetComponentInChildren<NetworkSyncObject>());
            networkObject.GetComponentInChildren<NetworkSyncObject>().useGravity.Value = _useGravity;
            Debug.Log("[TEST] Set Gravity");
        }
    }

    [ServerRpc]
    public void RequestSetActiveServerRpc(NetworkObjectReference networkObjectReference, bool _isActive)
    {
        Debug.Log("[TEST] Got client set Active requests");
        if (networkObjectReference.TryGet(out NetworkObject networkObject))
        {
            Debug.Log("[TEST] networkObject.GetComponentInChildren<NetworkSyncObject>() = " + networkObject.GetComponentInChildren<NetworkSyncObject>());
            networkObject.GetComponentInChildren<NetworkSyncObject>().isActive.Value = _isActive;
            Debug.Log("[TEST] Set Active");
        }
    }

    // [ServerRpc]
    // public void RequestSetActiveServerRpc(ulong newOwnerClientId, NetworkObjectReference gameObjectReference, bool isActive)
    // {
    //     Debug.Log("[TEST] Got client setActive requests id: " + newOwnerClientId);

    //     GameObject(gameObjectReference);//.SetActive(isActive);
    //     Debug.Log("[TEST] SetActive " + isActive);
    // }
}