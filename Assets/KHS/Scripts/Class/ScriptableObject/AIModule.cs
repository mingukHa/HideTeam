using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AI Module", menuName = "Scriptable Objects/AI Module")]
public class AIModule : ScriptableObject
{
    public List<Action<NPCController>> behaviors = new List<Action<NPCController>>();

    [SerializeField] private List<string> behaviorNames = new List<string>();

    private static readonly Dictionary<string, Action<NPCController>> BehaviorRegistry = new Dictionary<string, Action<NPCController>>
    {
        { "Detect", npc => npc.Detect() },
        { "Response", npc => npc.ResponseSituation() },
        { "Patrol", npc => { if (npc.data.currentState == AIConState.Patrol) npc.PatrolOrMove(); } },
        { "Chase", npc => { if (npc.data.currentState == AIConState.SuspectDetected) npc.ChaseSuspect(); } },
    };

    public void Initialize()
    {
        behaviors.Clear();
        foreach (string behaviorName in behaviorNames)
        {
            if (BehaviorRegistry.TryGetValue(behaviorName, out Action<NPCController> behaviorAction))
            {
                behaviors.Add(behaviorAction);
            }
        }
    }

    public void ExecuteBehaviors(NPCController npc)
    {
        foreach (var behavior in behaviors)
        {
            behavior.Invoke(npc);
        }
    }
}
