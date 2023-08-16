using System;
using System.Collections;
using System.Collections.Generic;
using EnumTypes;
// using Unity.Multiplayer.Tools.NetStatsReporting;
using UnityEngine;

public class BearManager : MonoBehaviour
{
    public List<GameObject> PlayerBears= new List<GameObject>(4);
    //public List<GameObject> AutoBears;

    public List<GameObject> GuestBears;
    public List<GameObject> AnswerBears;

    public List<Material> BaseMaterials;
    public List<Texture2DArray> BaseColorList;

    public bool ShouldCheckPair = true;
    
    // TODO: 각 Bear List 셋업 (미리 등록 또는 Tag 검색)
    public void Init()
    {
        // Clear List
        GuestBears.Clear();
        AnswerBears.Clear();
        
        SetBearsList();         // Set List
        SetAnswerBearsItem();   // Set Answer Deco Item
        UpdatePairPlayer();
        // Debug.Log("[TEST] Initalize Bear Manager");
        // SetPlayerList();        // Set Player
    }

    private void SetBearsList()
    {
        // TODO: Set Player Bear List
        
        
        // Find Guest Bear List
        GameObject guest = GameObject.Find("Guest");
        for (int num = 0; num < guest.transform.childCount; num++)
        {
            GuestBears.Add(guest.transform.GetChild(num).gameObject);
        }
        
        // Find Answer Bear List
        GameObject answer = GameObject.Find("AnswerBears");
        for (int num = 0; num < answer.transform.childCount; num++)
        {
            AnswerBears.Add(answer.transform.GetChild(num).gameObject);
        }
        
        // Set Guest-Answer Pair
        for (int num = 0; num < guest.transform.childCount; num++)
        {
            GuestBears[num].GetComponent<GuestBear>().AnswerBear = AnswerBears[num];
        }
    }

    /// <summary>
    /// 정답곰의 Deco Item List를 세팅합니다
    /// </summary>
    private void SetAnswerBearsItem()
    {
        if (AnswerBears.Count == 0) Debug.Log("에러(LogError) : 정답곰을 Scene 내에 세팅해주세요");

        foreach (var bear in AnswerBears)
        {
            bear.GetComponent<AnswerBear>().InitAnswer();
        }
        
        foreach (var bear in GuestBears)
        {
            bear.GetComponent<GuestBear>().SetDecorationList();
        }
    }

    /// <summary>
    /// 꾸밀곰의 정면에 있는 Player를 확인하고, 그 순서대로 List Up
    /// </summary>
    public IEnumerator SetPairPlayerList()
    {
        int pair = 0;
        while (ShouldCheckPair)
        {
            PlayerBears.Clear();
            for (int i = 0; i < 4; i++)
            {
                GameObject temp = GuestBears[i].GetComponent<GuestBear>().GetPairPlayer();
                if (temp != null)
                {
                    PlayerBears.Add(temp);
                    pair++;
                }
            }
            yield return new WaitForSeconds(0.2f);
            
            if (pair < 4)
                ShouldCheckPair = true;

            else
            {
                ShouldCheckPair = false;
            }

        }
    }

    public void SetPlayerList_()
    {
        PlayerBears.Clear();
        for (int i=0; i < 4; i++)
        {
            GuestBear tempBear = GuestBears[i].GetComponent<GuestBear>();
            PlayerBears.Add(tempBear.GetPairPlayer());
        }
    }
    
    public void UpdatePairPlayer()
    {
        foreach (var guest in GuestBears)
        {
            // Debug.Log(guest.name + "의 UpdatePairPlayer() 진행함 -----> ");
            guest.GetComponent<GuestBear>().CheckPairPlayer();
        }
    }

    public void AutoDecorate()
    {
        //Debug.Log("[TEST] Start Auto decorate");
        //yield return StartCoroutine(SetPairPlayerList());
        //yield return new WaitForSeconds(3.0f);

        //SetPlayerList_();

        foreach (var bear in GuestBears)
        {
            GuestBear bearComponent = bear.GetComponent<GuestBear>();

            if (bearComponent.GetPairBearType() == BearType.PlayerBear) 
                continue; // Pair가 Player일 경우는 Auto를 수행하지 않음

            switch (bearComponent.GetPairPlayerType())
            {
                case DecorateType.PutCream:
                    bearComponent.PutCream();
                    break;

                case DecorateType.Draw:
                    bearComponent.Draw();
                    break;

                case DecorateType.CutAndShape:
                    bearComponent.CutAndShape();
                    break;

                case DecorateType.ChangeColor:
                    GameObject answer = bearComponent.AnswerBear;
                    bearComponent.ChangeBaseColor(answer.GetComponent<AnswerBear>().baseColor);
                    bearComponent.DoPlusDecoration();
                    break;

                case DecorateType.Basic:
                    break;
            }
        }
    }
    
    public void Test()
    {
        // Guest Bear color 변경 테스트
        // GuestBears[0].GetComponent<GuestBear>().ChangeBaseColor(BaseColorList[(int)BearColorType.Red]);

        // foreach (var bear in GuestBears)
        // {
        //     bear.GetComponent<GuestBear>().CutAndShape();
        //     GameObject answer = bear.GetComponent<GuestBear>().AnswerBear;
        //     bear.GetComponent<GuestBear>().ChangeBaseColor(BaseColorList[(int)answer.GetComponent<AnswerBear>().baseColor]);
        // }
    }
}
