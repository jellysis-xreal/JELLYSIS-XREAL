using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerUpdater : MonoBehaviour
{
    [SerializeField] private CircluarTimer[] timers;
    private bool _timerIsOver;
    
    
    [ContextMenu("Reset All Timers")]
    public void ResetAllTimer()
    {
        for (int i = 0; i < timers.Length; i++)
        {
            timers[i].ResetTimer();
        }
    }

    public void StopAllTimer()
    {
        for (int i = 0; i < timers.Length; i++)
        {
            timers[i].StopTimer();
        }
    }
}
