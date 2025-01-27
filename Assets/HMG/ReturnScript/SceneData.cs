using UnityEngine;

public class SceneData : MonoBehaviour
{    
    //NPC의 대한 정보들
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    public string Animation { get; set; }
    public float Duration { get; set; }
    /////////////////////////////////////////////
    public Vector3 PlayerPosition { get; set; }
    public Quaternion PlayerRotation { get; set; }
    public string PlayerAnimation { get; set; }
    public float PlayerDuration { get; set; }

    //Player의 대한 정보들
    public SceneData(Vector3 position, Vector3 playerposition, Quaternion rotation,
        Quaternion playerrotation, string animation, string playeranimation, float duration,float playerduration)
    {
        Position = position;
        Rotation = rotation;
        Animation = animation;
        Duration = duration;

        PlayerPosition = playerposition;
        PlayerRotation = playerrotation;
        PlayerAnimation = playeranimation;
        PlayerDuration = duration;
    }
}
