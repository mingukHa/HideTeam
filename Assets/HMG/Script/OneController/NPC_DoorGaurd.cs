using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using static EventManager;
using UnityEngine.AI;


public class NPC_DoorGaurd : NPCFSM
{

    public GameObject npcchatbox; //NPC의 메인 채팅 최상위
    private string npc = "NPC7";

    private void OnEnable()
    {
        //EventManager.Subscribe(GameEventType.OldManHelp, );
        
    }
    
    private void StopNpc()
    {
        StopCoroutine(TalkView());
        transform.rotation = initrotation;
        new WaitForSeconds(2f);
        animator.SetTrigger("Idel");
        select.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        chat = GetComponent<NPCChatTest>();
        agent = GetComponent<NavMeshAgent>();
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

        chat.LoadNPCDialogue("NULL", 0);
    }

    protected override void OnTriggerEnter(Collider other)
    {

        if (!isDead && other.CompareTag("Player"))
        {
            chat.LoadNPCDialogue(npc, 0);
        }
    }

    protected override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
   
    }
    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chat.LoadNPCDialogue("NULL", 0);

        }
    }
}

