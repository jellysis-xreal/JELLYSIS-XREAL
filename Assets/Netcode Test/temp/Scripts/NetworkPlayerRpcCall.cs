
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkPlayerRpcCall : NetworkBehaviour
{
    [ServerRpc]
    public void RequestGrabbableOwnershipServerRpc(ulong newOwnerClientId, NetworkObjectReference networkObjectReference)
    {
        Debug.Log("[TEST] Got client entering requests id: " + newOwnerClientId);
        if (networkObjectReference.TryGet(out NetworkObject networkObject))
        {
            networkObject.ChangeOwnership(newOwnerClientId);
            NetworkObject[] networkObjects = networkObject.GetComponent<Transform>().GetComponentsInChildren<NetworkObject>();

            for (int i = 0; i < networkObjects.Length; i++)
            {
                networkObjects[i].ChangeOwnership(newOwnerClientId);
                Debug.Log("[TEST] Enter: Now " + networkObjects[i] + " ownerClientID: " + networkObjects[i].OwnerClientId);
            }
        }
    }



    [ServerRpc]
    public void RequestRetrunGrabbableOwnershipServerRpc(ulong newOwnerClientId, NetworkObjectReference networkObjectReference)
    {
        Debug.Log("[TEST] Got client releasing requests id: " + newOwnerClientId);
        if (networkObjectReference.TryGet(out NetworkObject networkObject))
        {
            networkObject.RemoveOwnership();
            //networkObject.ChangeOwnership(newOwnerClientId);
            NetworkObject[] networkObjects = networkObject.GetComponent<Transform>().GetComponentsInChildren<NetworkObject>();

            for (int i = 0; i < networkObjects.Length; i++)
            {
                networkObjects[i].RemoveOwnership();
                Debug.Log("[TEST] Enter: Now " + networkObjects[i] + " ownerClientID: " + networkObjects[i].OwnerClientId);
            }
            //Debug.Log("[TEST] Released: Now "+ networkObject + " ownerClientID: " + networkObject.OwnerClientId);
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
                childObject.TrySetParent(parentObject, true);
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
}