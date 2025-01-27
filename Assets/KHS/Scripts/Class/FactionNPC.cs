using System.Collections.Generic;
using UnityEngine;

public class FactionNPC : IFaction
{
    private string faction;
    private List<Vector3> suspiciousAreas;

    public FactionNPC(string faction, List<Vector3> suspiciousAreas)
    {
        this.faction = faction;
        this.suspiciousAreas = suspiciousAreas;
    }

    public string GetFaction() => faction;

    public bool isSuspiciousArea(Vector3 position)
    {
        return suspiciousAreas.Contains(position);
    }
}
