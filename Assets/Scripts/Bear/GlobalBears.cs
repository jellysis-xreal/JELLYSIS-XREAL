using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GlobalBears : MonoBehaviour
{
   //public GameManager.BearColorType colorType;
   public int decoItemNum = 1;
   public Dictionary<uint, List<Vector3>> decoItemTable = new Dictionary<uint, List<Vector3>>();

   public Transform originParent;

   private void Awake()
   {
      originParent = this.transform;
   }

   //+--------- 공통 Function ---------+//
   /// <summary>
   /// Bear에게 꾸며져 있는 Item의 Dictionary를 Return
   /// </summary>
   public Dictionary<uint, List<Vector3>> GetItemTable()
   {
      return decoItemTable;
   }

   public void Decorate(uint item, Vector3 localPos)
   {
      // 이미 해당 GUID의 ItemList가 존재
      if (decoItemTable.TryGetValue(item, out var value))
      {
         value.Add(localPos);
      }
      else
      {
         List<Vector3> itemsPos = new List<Vector3>();
         itemsPos.Add(localPos);
         decoItemTable.Add(item, itemsPos);
      }
      
      decoItemNum++;
      
   }
}


