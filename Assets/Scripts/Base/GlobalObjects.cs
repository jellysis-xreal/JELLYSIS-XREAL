using System;
using System.Collections;
using System.Collections.Generic;
using EnumTypes;
using UnityEngine;

[Serializable]
public class GlobalObjects : MonoBehaviour
{
    public uint GUID;
    public string Name;

    // 어떤 기능에 의해 생성되는 데코 오브젝트인지 속성을 지정합니다
    public DecorateType decoType;

    // public void SetLocalPos(Vector3 pos)
    // {
    //     // 굳이?
    //     // LocalPos = this.transform.localPosition;
    //     LocalPos = pos;
    // }
}
