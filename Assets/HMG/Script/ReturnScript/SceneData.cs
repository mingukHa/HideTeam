using UnityEngine;
//데이터 전용 클래스
public class SceneData
{    
    //NPC의 대한 정보들
    public Vector3 NpcPosition { get; set; }
    public Quaternion NpcRotation { get; set; }
    public string NpcAnimation { get; set; }
    public float NpcDuration { get; set; }
    //플레이어의 대한 정보들
    public Vector3 PlayerPosition { get; set; }
    public Quaternion PlayerRotation { get; set; }
    public string PlayerAnimation { get; set; }
    public float PlayerDuration { get; set; }

    public SceneData(Vector3 position, Vector3 playerposition, Quaternion rotation,
        Quaternion playerrotation, string animation, string playeranimation, float duration,float playerduration)
    {
        NpcPosition = position;
        NpcRotation = rotation;
        NpcAnimation = animation;
        NpcDuration = duration;

        PlayerPosition = playerposition;
        PlayerRotation = playerrotation;
        PlayerAnimation = playeranimation;
        PlayerDuration = playerduration;
    }
}
