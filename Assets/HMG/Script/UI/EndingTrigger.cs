using UnityEngine;

public class EndingTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.Trigger(EventManager.GameEventType.Ending);
        }
    }
}
