using UnityEngine;

public class PlayerWalkSound : MonoBehaviour
{
    public AudioSource walkSound;
    public void Walk()
    {
        walkSound.Play(); // �߼Ҹ� ���
    }

}
