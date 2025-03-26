using UnityEngine;

public class ToiletColider : MonoBehaviour //NPC가 화장실에 있는가?
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
