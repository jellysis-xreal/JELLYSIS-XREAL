using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Program : MonoBehaviour
{
    private void Start()
    {
        MyClass myObject = new MyClass();
        myObject.MyEvent += MyEventHandler;

// 이벤트 발생
        myObject.RaiseEvent();

    }

    public void MyEventHandler(object sender, EventArgs e)
    {
        Debug.Log("Event was triggered!");
    }
}

public class MyClass
{
    // 이벤트 정의
    public event EventHandler MyEvent;
    
    // 이벤트를 발생시키는 메서드
    public void RaiseEvent()
    {
        OnMyEvent(EventArgs.Empty);
    }

    protected virtual void OnMyEvent(EventArgs e)
    {
        // 이벤트 핸들러가 등록되었는지 확인하고, 등록된 경우 실행
        MyEvent?.Invoke(this,e);
    }
}
public class Guild
{
    private readonly List<string> _members = new List<string>();

    public static event Action<string> NewMemberAdded;
    
    public void AddMember(string memberName)
    {
        _members.Add(memberName);
        
        NewMemberAdded?.Invoke(memberName);
    }
}
public class MemberWelcomer
{
    public void WelcomMember(string memberName)
    {
        Debug.Log("Welcome "+memberName);
    }
}

public class GuildHall
{
    public void AddBedroom(string memberName)
    {
        Debug.Log("A room was added to the guild hall for "+memberName);
    }
}