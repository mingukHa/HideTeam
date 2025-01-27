using UnityEngine;

public interface IState //인터페이스 스크립트
{
    void EnterState(NomalNPC npc);
    void UpdateState(NomalNPC npc);
    void ExitState(NomalNPC npc);
}
