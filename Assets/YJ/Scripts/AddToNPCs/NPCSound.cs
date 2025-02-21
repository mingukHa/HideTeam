using UnityEngine;

public class NPCSound : MonoBehaviour
{
    private SoundManager soundManager; // 오디오소스 컴포넌트

    private void Start()
    {
        soundManager = GetComponent<SoundManager>();
    }

    public void Walk()
    {
        SoundManager.instance.SFXPlay("Walk_SFX", this.gameObject);
    }
}
