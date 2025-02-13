using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using static EventManager;
using UnityEngine.AI;

public class NPC_CleanMan : NPCFSM
{
    [SerializeField] private GameObject select;
    private NPCChatTest chat;
    public GameObject npcchatbox;
    private string npc = "NPC4";
    
    private bool isLookingAround = false;
    
    public GameObject GarbagePos;
    private SphereCollider sphere;
    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.Garbage, StartGarbage);
    }
    private void StartGarbage()
    {
        Debug.Log("청소부 개 빡쳐서 달려오는 중");
        agent.SetDestination(GarbagePos.transform.position);
        animator.SetBool("Run", true);
        sphere.enabled = true;
    }
    protected override void Start()
    {
        base.Start();
        select.SetActive(false);
        chat = GetComponent<NPCChatTest>();
        select.SetActive(false);
        agent = GetComponent<NavMeshAgent>();
        sphere = GetComponent<SphereCollider>();
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
        npcchatbox.SetActive(false);
        chat.LoadNPCDialogue("NULL", 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isDead && other.CompareTag("Player"))
        {
            select.SetActive(true);
            ChangeState(State.Talk);
            chat.LoadNPCDialogue(npc, 0);
        }
    }
    protected override void OnTriggerStay(Collider other)
    {
        if (!isDead && other.CompareTag("Player"))
        {
            // 키 입력을 지속적으로 체크
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                chat.LoadNPCDialogue(npc, 1);
                EventManager.Trigger(GameEventType.sweeper);
            }
                if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                chat.LoadNPCDialogue(npc, 2);
                EventManager.Trigger(GameEventType.sweeperKill);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeState(State.Idle);
            select.SetActive(false);
            chat.LoadNPCDialogue("NULL", 0);
        }
    }
}

