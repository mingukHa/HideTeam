using UnityEngine;

public class PlayerToiletOut : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            EventManager.Trigger(EventManager.GameEventType.PlayerToiletOut);
        }
    }
}
