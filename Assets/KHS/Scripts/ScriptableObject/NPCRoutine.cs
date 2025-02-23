using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "NPCRoutine", menuName = "Scriptable Objects/NPCRoutine")]
public class NPCRoutine : ScriptableObject
{
    public List<RoutineAction> actions;
}

[System.Serializable]
public class RoutineAction
{
    public RoutineActionType actionType;
    
    public Vector3 targetPosition;
    public float waitTime;
    public EventManager.GameEventType gameEvent;
}

public enum RoutineActionType
{
    Move,
    Wait,
    Run,
    Talk,
    Taking,
    Smoking,
    Tell
}