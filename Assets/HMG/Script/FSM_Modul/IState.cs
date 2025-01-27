using UnityEngine;

public interface IState //�������̽� ��ũ��Ʈ
{
    void EnterState(NPC npc);
    void UpdateState(NPC npc);
    void ExitState(NPC npc);
}
