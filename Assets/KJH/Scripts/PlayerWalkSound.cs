using UnityEngine;

public class PlayerWalkSound : MonoBehaviour
{
    public AudioSource walkSound;
    public void Walk()
    {
        walkSound.Play(); // 발소리 재생
    }

}
