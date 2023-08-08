using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalObjects : MonoBehaviour
{
    public uint GUID;
    public string Name;
    public Vector3 LocalPos;
    //public GameManager.BearType UserType;
    
    public void SetLocalPos(Vector3 pos)
    {
        // 굳이?
        // LocalPos = this.transform.localPosition;
        LocalPos = pos;
    }
}
