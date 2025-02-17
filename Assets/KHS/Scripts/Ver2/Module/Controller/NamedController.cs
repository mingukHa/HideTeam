using System.Collections;
using System.Collections.Generic;
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
        stateMachine.ChangeState(new GoneState(this));
    }
    public void RichmanAngry1()
    {
        stateMachine.ChangeState(new Angry1State(this));
    }
}
