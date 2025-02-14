using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TellerNPC))]
public class TellerController : NPCController
{
    public int interactType = 0;

    public override void Start()
    {
        base.Start();
    }

    public void Interact()
    {
        stateMachine.ChangeState(new InteractState(this));
    }
}
