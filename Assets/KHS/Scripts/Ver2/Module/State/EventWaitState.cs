using System.Collections;
using UnityEngine;

public class EventWaitState : NPCState2
{
    public EventWaitState(NPCController npc) : base(npc)
    {
        
    }
    public override void Enter()
    {
        Debug.Log($"{_npcController.npcName}�� �̺�Ʈ�� �����Ͽ� 3�� ��� ����");
        _npcController.animator.SetTrigger("Look");
        _npcController.StartCoroutine(WaitBeforeMoving());
    }
    private IEnumerator WaitBeforeMoving()
    {
        yield return new WaitForSeconds(3f);
        _npcController.animator.ResetTrigger("Look");
        _npcController.stateMachine.ChangeState(new PatrolState(_npcController));
    }

    public override void Update()
    {
        
    }
}
