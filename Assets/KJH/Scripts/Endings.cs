using UnityEngine;

public class Endings : MonoBehaviour
{
    public DoorController door;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("¹®¿­±â");
            door.OpenDoor();
        }
    }
}
