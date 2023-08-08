using UnityEngine;

public static class Utils
{
    
    /// <summary>
    /// 게임 속도와 물리 시뮬레이션의 정확도를 조절함
    /// Time.timeScale 값에 따라 Time.fixedDeltaTime를 0.02f(기본 물리 시간 간격)에 해당 배율로 조정
    /// </summary>
    /// <param name="timescale"></param>
    public static void SetTimeScale(float timescale)
    {
        Time.timeScale = timescale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
    
    public static float DirectionToAngle(float x, float y)
    {
        float cos = x;
        float sin = y;
        return Mathf.Atan2(sin, cos) * Mathf.Rad2Deg;
    }
}
