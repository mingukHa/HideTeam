using System.Collections.Generic;
using UnityEngine;
using static EventManager;

[RequireComponent(typeof(RoutineInvoker)),RequireComponent(typeof(NamedNPC2))]
public class NamedController : NPCController
{
    public List<bool> eventFlags = new List<bool>();

    public override void Start()
    {
        base.Start();
    }
    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.Carkick, StartKar);
    }

    private void Update()
    {

    }
    public void StartKar()
    {
        stateMachine.ChangeState(new EventWaitState(this));
    }
    public override bool Response()
    {
        return false;
    }
}
