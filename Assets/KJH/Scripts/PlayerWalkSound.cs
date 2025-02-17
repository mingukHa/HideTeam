using UnityEngine;

public class PlayerWalkSound : MonoBehaviour
{
    public void Walk()
    {
        SoundManager.instance.SFXPlay("Walk_SFX", this.gameObject);
    }

}
