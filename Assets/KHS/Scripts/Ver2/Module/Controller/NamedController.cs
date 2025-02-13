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
}
