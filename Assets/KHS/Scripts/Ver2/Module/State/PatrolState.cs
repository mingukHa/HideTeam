using System.Collections;
using UnityEngine;

public class PatrolState : NPCState2
{
    private float patrolTime = 10f; // 3분 (180초)
    private float elapsedTime = 0f;

    public PatrolState(NPCController npc) : base(npc) { }

    public override void Enter()
    {
        Debug.Log($"{_npcController.npcName}가 3분 동안 순찰을 시작.");
        _npcController.animator.SetTrigger("Run");
        _npcController.StartCoroutine(PatrolRoutine());
    }

    private IEnumerator PatrolRoutine()
    {
        _npcController.routineInvoker.RoutineChange(1);
        while (elapsedTime < patrolTime)
        {
            _npcController.routineInvoker.ExcuteRoutine(); // 순찰 루틴 실행
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        _npcController.animator.ResetTrigger("Run");
        _npcController.stateMachine.ChangeState(new RoutineState(_npcController)); // 원래 루틴으로 복귀
    }

    public override void Update()
    {

    }
}
