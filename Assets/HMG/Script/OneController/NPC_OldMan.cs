using Unity.VisualScripting;
using UnityEngine;

public class NPC_OldMan : NPCFSM
{
    private NPCChatTest chat;
    private string npc = "NPC3";
    protected override void Start()
    {
        base.Start();
        chat = GetComponent<NPCChatTest>();

    }

    protected override void Update()
    {
        base.Update(); 
    }

    protected override void IdleBehavior()
    {
        base.IdleBehavior();
    }

    protected override void LookBehavior()
    {
        base.LookBehavior();
    }

    protected override void WalkBehavior()
    {
        base.WalkBehavior();
    }

    protected override void RunBehavior()
    {
        base.RunBehavior();
    }

    protected override void TalkBehavior()
    {
        base.TalkBehavior();
    }

    protected override void DeadBehavior()
    {
        base.DeadBehavior();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
            ChangeState(State.Talk);
            chat.LoadNPCDialogue(npc, 0);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeState(State.Idle);
        }
    }
}
