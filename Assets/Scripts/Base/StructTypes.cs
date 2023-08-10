using System;
using EnumTypes;
using UnityEngine;
using Object = System.Object;

/* [ Struct Types ]
* Global하게 사용되어야 하는 공통 데이터 타입 정의함
*/

namespace StructsType
{
    public struct DecoItemData
    {
        //public int itemNum;
        public GameObject ItemObject;
        public DecorateType ItemType;
        public Transform Parent;
        public Vector3 LocalPosition;
        public Quaternion LocalRotation;

        public DecoItemData(GameObject itemObject, DecorateType type)
        {
            this.ItemObject = itemObject;
            this.ItemType = type;
            this.Parent = itemObject.transform.parent;
            this.LocalPosition = itemObject.transform.localPosition;
            this.LocalRotation = itemObject.transform.localRotation;
        }
    }
}