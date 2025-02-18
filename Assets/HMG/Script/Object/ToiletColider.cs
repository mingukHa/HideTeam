using UnityEngine;

public class ToiletColider : MonoBehaviour
{
    public NPCRichMan nPCRichMan;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            nPCRichMan.isToilet = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            nPCRichMan.isToilet = false;
        }
    }
}
