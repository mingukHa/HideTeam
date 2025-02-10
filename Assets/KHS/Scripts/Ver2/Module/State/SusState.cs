using UnityEngine;

public class SusState : MonoBehaviour
{
    [SerializeField]
    private bool isSuspicious = false;

    public bool IsSuspicious
    {
        get { return isSuspicious; }
        set { isSuspicious = value; }
    }
}
