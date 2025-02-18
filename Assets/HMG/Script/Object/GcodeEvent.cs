using UnityEngine;

public class GcodeEvent : MonoBehaviour
{
    private void OnEnable()
    {
       // EventManager.Subscribe(EventManager.GameEventType.Gcode, StartGcode);
    }
    private void OnDisable()
    {
       // EventManager.Unsubscribe(EventManager.GameEventType.Gcode, StartGcode);
    }
    private void StartGcode()
    {
        box.enabled = false;
        doorman.enabled = false;
    }

    public BoxCollider box;
    public NPC_DoorGaurd doorman;
    


}
