using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkPlayerRpcCall : NetworkBehaviour
{
    public void OnSelectGrabbable(SelectEnterEventArgs eventArgs)
    {
        Debug.Log("[TEST] Grabbed!");
        if (IsClient && IsOwner)
        {
            NetworkObject networkObjectSelected = eventArgs.interactableObject.transform.GetComponent<NetworkObject>();
            Debug.Log("[TEST] Client selected the "+ networkObjectSelected);
            if (networkObjectSelected != null)
                RequestGrabbableOwnershipServerRpc(OwnerClientId, networkObjectSelected);
        }
    }

    public void OnSelectExitGrabbable(SelectExitEventArgs eventArgs)
    {
        Debug.Log("[TEST] Release!!");
        if (IsClient && IsOwner)
        {
            NetworkObject networkObjectSelected = eventArgs.interactableObject.transform.GetComponent<NetworkObject>();
            Debug.Log("[TEST] Client released the "+ networkObjectSelected);
            if (networkObjectSelected != null)
                RequestRetrunGrabbableOwnershipServerRpc(OwnerClientId, networkObjectSelected);
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