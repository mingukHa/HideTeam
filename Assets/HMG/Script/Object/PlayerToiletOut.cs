using UnityEngine;

public class PlayerToiletOut : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) //플레이어가 화장실을 나올 때
    {
        if(other.CompareTag("Player"))
        {
            EventManager.Trigger(EventManager.GameEventType.PlayerToiletOut);
        }
    }
}
