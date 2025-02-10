using UnityEngine;

public class WaitState : NPCState
{
    public WaitState(TNPCController npc) : base(npc) { }

    public override void Enter()
    {
        Debug.Log(_npcController.npcName + " is now WAITING.");
        _npcController.GetComponent<NamedNPCController>().ChangeRoutine(1); // 1번 루틴으로 변경
        _npcController.Invoker.AddCommand(new RoutineCommand(_npcController.GetComponent<NamedNPCController>()));
    }

    public override void Update()
    {
        _npcController.StartCoroutine(JudgeCoroutine(_npcController.GetComponent<NamedNPCController>().RoutineCheck(), new IdleState(_npcController)));
    }
}
