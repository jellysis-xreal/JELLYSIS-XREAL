using System;
using System.Collections;
using System.Collections.Generic;
using EnumTypes;
// using Unity.Multiplayer.Tools.NetStatsReporting;
using UnityEngine;

public class BearManager : MonoBehaviour
{
    public List<GameObject> PlayerBears;
    //public List<GameObject> AutoBears;

    public List<GameObject> GuestBears;
    public List<GameObject> AnswerBears;

    public List<Material> BaseMaterials;
    public List<Texture2DArray> BaseColorList;

    // TODO: 각 Bear List 셋업 (미리 등록 또는 Tag 검색)
    public void Init()
    {
        // Clear List
        GuestBears.Clear();
        AnswerBears.Clear();
        
        SetBearsList();         // Set List
        SetAnswerBearsItem();   // Set Answer Deco Item
        SetPlayerList();        // Set Player
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
    }
    
    /// <summary>
    /// 꾸밀곰의 정면에 있는 Player를 확인하고, 그 순서대로 List Up
    /// </summary>
    private void SetPlayerList()
    {
        PlayerBears.Clear();
        foreach (var guest in GuestBears)
        {
            guest.GetComponent<GuestBear>().CheckPairPlayer();
        }
        Invoke("SetPlayerList_", 3.0f);
    }

    private void SetPlayerList_()
    {
        foreach (var guest in GuestBears)
        {
            PlayerBears.Add(guest.GetComponent<GuestBear>().GetPairPlayer());
        }
    }

    
    
    public void Test()
    {
        // Guest Bear color 변경 테스트
        // GuestBears[0].GetComponent<GuestBear>().ChangeBaseColor(BaseColorList[(int)BearColorType.Red]);

        foreach (var bear in GuestBears)
        {
            bear.GetComponent<GuestBear>().CutAndShape();
            GameObject answer = bear.GetComponent<GuestBear>().AnswerBear;
            bear.GetComponent<GuestBear>().ChangeBaseColor(BaseColorList[(int)answer.GetComponent<AnswerBear>().baseColor]);
        }
    }
}
