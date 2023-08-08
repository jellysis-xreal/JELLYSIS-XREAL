using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableEventManager : MonoBehaviour
{
    /*#region C#Eventbasic
    public delegate void DoSomething(int num);
    public DoSomething Something;

    /*public TableEventManager()
    {

        //if (Something != null) Something(123); Something?.Invoke(123); 2개는 같다.
        Something += Function;
        Something += FunctionTwo;
        
        Something?.Invoke(456);
    }#1#

    private string FunctionTwo(int num)
    {
        Debug.Log("Function 2 called with value : "+num);
        return "dfgadg";
    }

    private int Function(int num)
    {
        Debug.Log("Function 1 called with value : "+num);
        return num;
    }
    
    public Func<int,string> SomeAction;

    public TableEventManager()
    {
        //SomeAction += Function;
        SomeAction += FunctionTwo;
    }
    #endregion
    */
    
    // define event
    public event EventHandler tableRotationEvent;
    
    // 이벤트를 발생시키는 메서드
    public void RaiseEvent()
    {
        tableEvent(EventArgs.Empty);
    }

    protected virtual void tableEvent(EventArgs e)
    {
        tableRotationEvent?.Invoke(this,e);
    }
}