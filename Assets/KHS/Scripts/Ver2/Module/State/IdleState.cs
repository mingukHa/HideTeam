using UnityEngine;

public class IdleState : NPCState
{
    public IdleState(TNPCController npc) : base(npc) { }

    public override void Enter()
    {
        Debug.Log(_npcController.npcName + " is now IDLE.");
        _npcController.Invoker.AddCommand(new DetectCommand(_npcController));  // Idle ���¿��� Ÿ�� ���� ��� �߰�
    }

    public override void Update()
    {
        _npcController.Invoker.ExecuteCommands();
        if (_npcController.HasDetectedTarget())  // Ÿ���� �����Ǿ�����
        {
            _npcController.stateMachine.ChangeState(new ChaseState(_npcController));  // Chase ���·� ��ȯ
        }
    }
}