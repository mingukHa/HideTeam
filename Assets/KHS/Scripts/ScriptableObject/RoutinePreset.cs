using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoutinePreset", menuName = "Scriptable Objects/RoutinePreset")]
public class RoutinePreset : ScriptableObject
{
    public List<Vector3> waypoints = new List<Vector3>();
}
