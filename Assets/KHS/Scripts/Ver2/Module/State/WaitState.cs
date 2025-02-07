using UnityEngine;

public class WaitState : NPCState
{
    public WaitState(TNPCController npc) : base(npc) { }

    public override void Enter()
    {
        NPCType npcType = _npcController.npcType;
        
        if (npcType.GetComponent<NamedNPC>())
        {
            Debug.Log(_npcController.npcName + " is now WAITING.");
            _npcController.Invoker.AddCommand(new RoutineCommand(_npcController.GetComponent<NamedNPCController>()));
        }
    }

    public override void Update()
    {
        _npcController.StartCoroutine(JudgeCoroutine(!_npcController.GetComponent<NamedNPCController>().Routine(), new IdleState(_npcController)));
    }
}
