using UnityEngine;

public interface IState //�������̽� ��ũ��Ʈ
{
    void EnterState(NomalNPC npc);
    void UpdateState(NomalNPC npc);
    void ExitState(NomalNPC npc);
}
