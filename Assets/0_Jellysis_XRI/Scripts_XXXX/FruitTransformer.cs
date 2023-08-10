using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FruitTransformer : MonoBehaviour
{
    [SerializeField] private Transform[] placeTransforms;
    [SerializeField] private bool[] isHavingFruit;
    public Transform debugTransform;
    public int numZeroChildCount;
    public int callCount;

    private void Start()
    {
        isHavingFruit = new bool[placeTransforms.Length];
//        Debug.Log(isHavingFruit.Length);
    }

    public void AttachFruit(Transform fruitTransform)
    {
        // 과일 꽂기, placeTransforms수 만큼 반복
        for (int i = 0; i < placeTransforms.Length; i++)
        {
            if (!isHavingFruit[i])
            {
                fruitTransform.SetParent(placeTransforms[i], true);
                isHavingFruit[i] = true;
                OnOffGrabInteract(i);
                Debug.Log("Set Parent!!!");
                /*fruitTransform.localScale =
                    new Vector3(1f / placeTransforms[i].localScale.x, 1f / placeTransforms[i].localScale.y, 1f / placeTransforms[i].localScale.z);*/
                fruitTransform.localPosition = Vector3.zero;
                fruitTransform.localRotation = Quaternion.Euler(45f,0,0);
                return;
            }
        }
    }

    public void DetachFruit()
    {
        // 마지막으로 뽑은 자리를 false
        // 마지막부터 검사, true 반환되면 그거 false로 바꾸고 return 
        for (int i = isHavingFruit.Length -1; i >= 0 ; i--)
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
        if(i==0) return;
        Debug.Log("가져오기!");
        XRGrabInteractable xrGrab = placeTransforms[i - 1].GetChild(0).GetComponent<XRGrabInteractable>();
        xrGrab.enabled = !xrGrab.enabled;
        /*if (placeTransforms[i-1].TryGetComponent<XRGrabInteractable>(out XRGrabInteractable axrGrabInteractable))
        {
            xrGrabInteractable.enabled = !xrGrabInteractable.enabled;
        }*/
    }
}
