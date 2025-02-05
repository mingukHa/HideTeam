using UnityEngine;
//������ ���� Ŭ����
public class SceneData
{    
    //NPC�� ���� ������
    public Vector3 NpcPosition { get; set; }
    public Quaternion NpcRotation { get; set; }
    public string NpcAnimation { get; set; }
    //�÷��̾��� ���� ������
    public Vector3 PlayerPosition { get; set; }
    public Quaternion PlayerRotation { get; set; }
    public string PlayerAnimation { get; set; }
    public float Duration { get; set; }

    public SceneData(Vector3 position, Vector3 playerposition, Quaternion rotation,
        Quaternion playerrotation, string animation, string playeranimation, float duration)
    {
        NpcPosition = position;
        NpcRotation = rotation;
        NpcAnimation = animation;
        
        Duration = duration;

        PlayerPosition = playerposition;
        PlayerRotation = playerrotation;
        PlayerAnimation = playeranimation;
    }
}
