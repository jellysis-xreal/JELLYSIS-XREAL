using System;
using System.Collections.Generic;
using EnumTypes;
using StructsType;
using Unity.VisualScripting;
using UnityEngine;

public class AnswerBear : GlobalBears
{
    // Answer Card에 등록된 Correct Bear Prefab
    
    // 해당 Bear에게 꾸며진 Decorate를 확인하고, 정보를 넘겨주어야함
    // [0814] 해당 정보 GuestBear 자체에 저장해두는 것으로 바뀌었습니다
    //public List<DecoItemData> decoItemDataList = new List<DecoItemData>();

    private void Awake()
    {
        BearType = BearType.AnswerBear;
        originParent = transform.Find("Armature");
        
        //decoItemDataList.Clear();
    }

    // public List<DecoItemData> GetDecoItemList()
    // {
    //     //return decoItemDataList;
    // }
    
    /// <summary>
    /// 정답곰의 기본적인 세팅 이후, 비활성화 됩니다
    /// </summary>
    public void InitAnswer()
    {
        Debug.Log("<<-------Init AnswerBear------->>");
        // 1. PutCream 및 CutAndShape 관련 데코
        // SetDecorationList();
        
        // 2. 바탕 색은 BearColorType으로 표현됨
        
        // TODO: 3. Draw 관련 Texture를 저장해두자
        
        // Scene내에서 보이지 않도록 처리
        gameObject.SetActive(false);
    }

    
    /// <summary>
    /// Deco Item의 Dictionary를 생성함
    /// [0814] 해당 함수 GuestBear로 옮겨졌습니다
    /// </summary>
    /*public void SetDecorationList()
    {
        Transform[] childrens = originParent.GetComponentsInChildren<Transform>();

        foreach (Transform child in childrens)
        {
            if (child.CompareTag("Deco"))
            {
                // 각 Deco는 Bear의 Armature Hierarchy 하위에 존재
                //Debug.Log("Find Answer Decoration : "+ child.name);
                
                // TODO: Deco Item으로부터 Type을 알아야함 따라서 각 프리팹에 타입을 정의해야함 (임시로 해둠)
                DecoItemData item = new DecoItemData(child.gameObject, DecorateType.CutAndShape);
                
                //decoItemDataList.Add(item);
                decoItemNum++;
            }
        }
    }*/
}
