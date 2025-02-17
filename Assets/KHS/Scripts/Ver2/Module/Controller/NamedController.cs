using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(NamedNPC))]
public class NamedController : NPCController
{
    public List<string> AngryEvent1Dialogue;
    public List<string> AngryEvent2Dialogue;

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
}
