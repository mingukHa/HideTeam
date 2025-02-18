using UnityEngine;
using static EventManager;

public class DebugMode : MonoBehaviour
{
    //private bool eventTriggered = false;

    //private void OnEnable()
    //{
    //    EventManager.Subscribe(GameEventType.SomeEvent, () => {
    //        eventTriggered = true;
    //        Invoke("ResetGizmo", 2f); // 2초 후 초기화
    //    });
    //}

    //private void OnDisable()
    //{
    //    EventManager.Unsubscribe(GameEventType.SomeEvent, () => {
    //        eventTriggered = false;
    //    });
    //}

    //private void ResetGizmo()
    //{
    //    eventTriggered = false;
    //}

    //private void OnDrawGizmos()
    //{
    //    if (eventTriggered)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawSphere(transform.position, 1f);
    //    }
    //}
}

