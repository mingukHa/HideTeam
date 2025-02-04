using UnityEngine;

public interface IFaction
{
    string GetFaction();
    bool isSuspiciousArea(Vector3 position);
}

