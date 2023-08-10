using System;
using System.Collections;
using System.Collections.Generic;
using EnumTypes;
// using Unity.Multiplayer.Tools.NetStatsReporting;
using UnityEngine;

public class BearManager : MonoBehaviour
{
    public List<GameObject> PlayerBears;
    public List<GameObject> AutoBears;

    public List<GameObject> GuestBears;
    public List<CorrectBear> CorrectBears;
    
    
    public List<Material> BaseMaterials;
    public List<Texture2DArray> BaseColorList;

    
    // TODO: 현재 Pair 상태를 업데이트 하는 함수 필요
    

    public void Test()
    {
        // Guest Bear color 변경 테스트
        GuestBears[0].GetComponent<GuestBear>().ChangeBaseColor(BaseColorList[(int)BearColorType.Red]);
    }
}
