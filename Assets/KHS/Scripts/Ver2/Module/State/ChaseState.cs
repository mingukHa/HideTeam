using UnityEngine;

public class ChaseState : NPCState
{
    public ChaseState(TNPCController npc) : base(npc) { }

    public override void Enter()
    {
        NPCType npcType = _npcController.npcType;
        if (npcType.GetComponent<DefaultNPC>())
        {
            Debug.Log(_npcController.npcName + " is now CHASING.");
            _npcController.Invoker.AddCommand(new MoveCommand(_npcController));
        }
        if (npcType.GetComponent<GuardNPC>())
        {
            Debug.Log(_npcController.npcName + " is now CHASING.");
            _npcController.Invoker.AddCommand(new MoveCommand(_npcController.GetComponent<GuardController>()));
        }
    }

    public override void Update()
    {
        _npcController.StartCoroutine(JudgeCoroutine(_npcController.HasDetectedTarget() == false, new IdleState(_npcController)));
    }
}
