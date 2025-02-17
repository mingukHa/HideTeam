using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using static EventManager;
using UnityEngine.AI;


public class NPC_DoorGaurd : NPCFSM
{

    
    public GameObject npcchatbox; //NPC의 메인 채팅 최상위
    private string npc = "NPC1";
    public SphereCollider sphereCollider;
    private void OnEnable()
    {

        EventManager.Subscribe(GameEventType.NPCKill, StartNPCKill);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEventType.NPCKill, StartNPCKill);
    }

    private void StartNPCKill()
    {
        
        ChangeState(State.Run);
        agent.SetDestination(player.position);
        sphereCollider.radius = 3.5f;
        StartCoroutine(CheckArrival());

    }

    private IEnumerator CheckArrival()
    {
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null; 
        }

        Debug.Log("NPC가 도착했습니다!");
        StartCoroutine(TalkView());
        animator.SetTrigger("Talk");
        chat.LoadNPCDialogue(npc, 1);
        yield return new WaitForSeconds(2f);
        EventManager.Trigger(GameEventType.GameOver);
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
        if (Input.GetKeyDown(KeyCode.F))
        {
            chat.LoadNPCDialogue(npc, 1);
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

