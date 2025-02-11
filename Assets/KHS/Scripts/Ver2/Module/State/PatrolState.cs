using System.Collections;
using UnityEngine;

public class PatrolState : NPCState2
{
    private float patrolTime = 10f; // 3�� (180��)
    private float elapsedTime = 0f;

    public PatrolState(NPCController npc) : base(npc) { }

    public override void Enter()
    {
        Debug.Log($"{_npcController.npcName}�� 3�� ���� ������ ����.");
        _npcController.animator.SetTrigger("Run");
        _npcController.StartCoroutine(PatrolRoutine());
    }

    private IEnumerator PatrolRoutine()
    {
        _npcController.routineInvoker.RoutineChange(1);
        while (elapsedTime < patrolTime)
        {
            _npcController.routineInvoker.ExcuteRoutine(); // ���� ��ƾ ����
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        _npcController.animator.ResetTrigger("Run");
        _npcController.stateMachine.ChangeState(new RoutineState(_npcController)); // ���� ��ƾ���� ����
    }

    public override void Update()
    {

    }
}
