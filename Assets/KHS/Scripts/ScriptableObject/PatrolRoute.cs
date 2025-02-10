using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPatrolRoute", menuName = "Scriptable Objects/PatrolRoute")]
public class PatrolRoute : ScriptableObject
{
    public List<Vector3> waypoints;
}
