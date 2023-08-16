using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.Netcode;

public class FruitTransformer_multi : NetworkBehaviour
{
    [SerializeField] private NetworkObject[] placeTransforms_NO;
    [SerializeField] private bool[] isHavingFruit; // sync 안 됨
    public Transform debugTransform;
    public int numZeroChildCount;
    public int callCount;

    private void Start()
    {
        isHavingFruit = new bool[placeTransforms_NO.Length];
        //        Debug.Log(isHavingFruit.Length);
    }

    // find all networkobject inactivate
    // find all networkobject activate

    public void AttachFruit(NetworkObject child_fruitTransform)
    {
        // [Multi] 과일 꽂기, placeTransforms수 만큼 반복
        for (int i = 0; i < placeTransforms_NO.Length; i++)
        {
            if (!isHavingFruit[i])
            {
                RequestSetParent(child_fruitTransform, placeTransforms_NO[i]);
                RequestOwnership(child_fruitTransform);

                isHavingFruit[i] = true;
                OnOffGrabInteract(i);
                Debug.Log("Set Parent!!!");
                
                child_fruitTransform.GetComponent<Transform>().localPosition = Vector3.zero;
                child_fruitTransform.GetComponent<Transform>().localRotation = Quaternion.Euler(45f, 0, 0);
                Debug.Log("[TEST] id:" + child_fruitTransform.GetComponent<Transform>()+ " local pos " + child_fruitTransform.GetComponent<Transform>().localPosition);
                return;
            }
        }
    }

    public void DetachFruit()
    {
        // 마지막으로 뽑은 자리를 false
        // 마지막부터 검사, true 반환되면 그거 false로 바꾸고 return 
        for (int i = isHavingFruit.Length - 1; i >= 0; i--)
        {
            if (isHavingFruit[i])
            {
                isHavingFruit[i] = false;
                OnOffGrabInteract(i);
                return;
            }
        }
    }

    private void OnOffGrabInteract(int i)
    {
        
        // 과일 3개가 꽂혀있을 경우 먼저 꼽힌 과일의 XR Grab Interactable 컴포넌트를 끄기 위함.
        // 과일 꼽았을 때 i-1 grab을 꺼야됨.
        // 과일을 잡았을 때 i-1의 grab을 켜야함. 
        // i-1 번째 과일의 XR Grab Interactable에 접근하기
        if (i == 0) return;
        Debug.Log("가져오기!");
        XRGrabInteractable xrGrab = placeTransforms_NO[i - 1].GetComponent<Transform>().GetChild(0).GetComponent<XRGrabInteractable>();
        xrGrab.enabled = !xrGrab.enabled;
        /*if (placeTransforms[i-1].TryGetComponent<XRGrabInteractable>(out XRGrabInteractable axrGrabInteractable))
        {
            xrGrabInteractable.enabled = !xrGrabInteractable.enabled;
        }*/
    }

    public void RequestSetParent(NetworkObject childObject, NetworkObject parentObject)
    {
        if (IsServer)
        {
            Debug.Log("[TEST] RequestSetParent. This is Server");
            // Debug.Log("[TEST] TrySetParent = " + childObject.TrySetParent(parentObject));
        }
        if ((IsClient))
        {
            ulong PlayerClientID = NetworkManager.Singleton.LocalClientId; //LocalId;
            NetworkObject localPlayer = NetworkManager.LocalClient.PlayerObject;
            Debug.Log("[TEST] RequestSetParent. ID: " + PlayerClientID + " Client wants to set parent " + parentObject);
            if (childObject == parentObject)
                Debug.Log("[TEST] RequestSetParent. parentObject = childObject = " + parentObject);
            else if (parentObject != null)
                localPlayer.GetComponent<NetworkPlayerRpcCall>().RequestSetParentServerRpc(childObject, parentObject);
        }
        else
        {
            Debug.Log("[TEST] RequestSetParent. Error Rq Ownership ---- ");
            //printInfo();
        }
    }

    public void RequestOwnership(NetworkObject networkObjectSelected)
    {
        if (IsServer)
        {
            Debug.Log("[TEST] RequestOwnership. This is Server");
        }
        //else if ((IsClient && !IsOwner))
        else if ((!IsOwner))
        {
            ulong PlayerClientID = NetworkManager.Singleton.LocalClientId; // LocalId;
            NetworkObject localPlayer = NetworkManager.LocalClient.PlayerObject;
            Debug.Log("[TEST] RequestOwnership. ID: " + PlayerClientID + " Client grabbed the " + networkObjectSelected);
            if (networkObjectSelected != null)
                Debug.Log("[TEST] RequestOwnership ... in progress");
            localPlayer.GetComponent<NetworkPlayerRpcCall>().RequestGrabbableOwnershipServerRpc(PlayerClientID, networkObjectSelected); // 이거 고쳐야하나...
        }
        else
        {
            Debug.Log("[TEST] RequestOwnership. Error Rq Ownership ---- ");
        }
    }

    /*
    public void DisableAllNetworkObject(Transform transform)
    {
        //NetworkObject[] networkObjects = new NetworkObject[]

        transform.root.TryGetComponent(out NetworkObject rootTransformNetworkObject);
        rootTransformNetworkObject.enabled = false;

        if (transform.parent != null) // 상위에 부모가 있으면 최상위 오브젝트 밑으로 다끔
        {
            NetworkObject[] networkObjects = transform.root.GetComponentsInChildren<NetworkObject>();
            for (int i = 0; i < networkObjects.Length; i++)
            {
                networkObjects[i].enabled = false;
            }
        }
        else // 상위에 없으면 나 끄고 자식 끄고
        {
            TryGetComponent(out NetworkObject myNetworkObject);
            NetworkObject[] networkObjects = transform.GetComponentsInChildren<NetworkObject>();

            myNetworkObject.enabled = false;
            for (int i = 0; i < networkObjects.Length; i++)
            {
                networkObjects[i].enabled = false;
            }
        }
    }


    public void EnableAllNetworkObject(Transform transform)
    {
        //NetworkObject[] networkObjects = new NetworkObject[]

        transform.root.TryGetComponent(out NetworkObject rootTransformNetworkObject);
        rootTransformNetworkObject.enabled = true;

        if (transform.parent != null) // 상위에 부모가 있으면 최상위 오브젝트 밑으로 다끔
        {
            NetworkObject[] networkObjects = transform.root.GetComponentsInChildren<NetworkObject>();
            for (int i = 0; i < networkObjects.Length; i++)
            {
                networkObjects[i].enabled = true;
            }
        }
        else // 상위에 없으면 나 끄고 자식 끄고
        {
            TryGetComponent(out NetworkObject myNetworkObject);
            NetworkObject[] networkObjects = transform.GetComponentsInChildren<NetworkObject>();

            myNetworkObject.enabled = true;
            for (int i = 0; i < networkObjects.Length; i++)
            {
                networkObjects[i].enabled = true;
            }
        }
    }
    */



}
