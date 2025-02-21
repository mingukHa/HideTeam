using UnityEngine;

public class NPCSound : MonoBehaviour
{
    private SoundManager soundManager; // ������ҽ� ������Ʈ

    private void Start()
    {
        soundManager = GetComponent<SoundManager>();
    }

    public void Walk()
    {
        SoundManager.instance.SFXPlay("Walk_SFX", this.gameObject);
    }
}
