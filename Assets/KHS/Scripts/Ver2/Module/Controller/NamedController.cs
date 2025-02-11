using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RoutineInvoker)),RequireComponent(typeof(NamedNPC2))]
public class NamedController : NPCController
{
    public override void Start()
    {
        base.Start();
    }

    public override bool Response()
    {
        return false;
    }
}
