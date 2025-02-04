using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCStats : MonoBehaviour
{
    public bool isStunned = false;
    private List<Material> bodyMat;

    private void Awake()
    {
        bodyMat = GetComponentInChildren<MeshRenderer>().materials.ToList();
    }

    public void StunAnimPlay()
    {
        isStunned = true;
        foreach (Material mat in bodyMat)
        {
            mat.EnableKeyword("_EMISSION");
        }
    }
    public void EscapeStun()
    {
        isStunned = false;
        foreach(Material mat in bodyMat)
        {
            mat.DisableKeyword("_EMISSION");
        }
    }
}
