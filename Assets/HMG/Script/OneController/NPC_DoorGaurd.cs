using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using static EventManager;
using UnityEngine.AI;


public class NPC_DoorGaurd : NPCFSM
{


    public GameObject npcchatbox; //NPC의 메인 채팅 최상위
    private string npc = "NPC4";

    private void OnEnable()
    {
       // EventManager.Subscribe(GameEventType.Garbage, StartGarbage);
       
    }



    private void StopNpc()
    {
        StopCoroutine(TalkView());
        transform.rotation = initrotation;
        NPCCollider.radius = 0.01f;
        animator.SetTrigger("Idel");
        select.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        select.SetActive(false);
        chat = GetComponent<NPCChatTest>();

        agent = GetComponent<NavMeshAgent>();
        NPCCollider = GetComponent<SphereCollider>();
    }

    protected override void Update()
    {

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
        npcchatbox.SetActive(false);
        chat.LoadNPCDialogue("NULL", 0);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!isDead && other.CompareTag("Player"))
        {
            ChangeState(State.Talk);
            chat.LoadNPCDialogue(npc, 0);
        }
        if (Input.GetKey(KeyCode.F))
        {
            isDead = true;
        }

    }

    //protected override void OnTriggerStay(Collider other)
    //{
    //    base.OnTriggerStay(other);
    //    if (!isDead && other.CompareTag("Player"))
    //    {
            
    //    }
    //}

    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chat.LoadNPCDialogue("NULL", 0);
        }

    }
}

