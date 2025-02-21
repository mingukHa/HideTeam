using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;


[RequireComponent(typeof(NamedNPC))]
public class NamedController : NPCController
{
    public override void Start()
    {
        base.Start();
    }

    public void StartKar()
    {
        stateMachine.ChangeState(new EventWaitState(this));
    }
    public void PlayerEnter()
    {
        stateMachine.ChangeState(new RoutineState(this));
    }
    public void RichmanGone()
    {
        gameObject.tag = "NPCEND";
        stateMachine.ChangeState(new GoneState(this));
    }
    public void RichmanAngry1()
    {
        stateMachine.ChangeState(new Angry1State(this));
    }
    public void RichmanAngry3()
    {
        stateMachine.ChangeState(new Angry3State(this));
    }
    public void CallGang()
    {
        Debug.Log("접근하여 전화내용 출력하기");
    }
    public void GcodeEvent()
    {
        Debug.Log("Gcode 도달하다!");
    }
}
