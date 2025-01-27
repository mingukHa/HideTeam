using UnityEngine;

public interface IState //인터페이스 스크립트
{
    void EnterState(NPC npc);
    void UpdateState(NPC npc);
    void ExitState(NPC npc);
}
