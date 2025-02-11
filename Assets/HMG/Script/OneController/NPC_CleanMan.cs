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
    private int currentWaypointIndex;
    private bool isLookingAround = false;
    private NavMeshAgent agent;
    public Transform GarbagePos;
    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.Garbage, StartGarbage);
    }
    private void StartGarbage()
    {
        Debug.Log("청소부 개 빡쳐서 달려오는 중");
        animator.SetTrigger("Run");
        agent.SetDestination(GarbagePos.transform.position);
    }
    protected override void Start()
    {
        base.Start();
        chat = GetComponent<NPCChatTest>();
        select.SetActive(false);       
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
        if (isDead == false)
        {
            if (other.CompareTag("Player"))
            {
                select.SetActive(true);
                ChangeState(State.Talk);
                chat.LoadNPCDialogue(npc, 0);
                if (Input.GetKeyDown(KeyCode.Keypad1))
                {
                    chat.LoadNPCDialogue(npc, 1);
                    EventManager.Trigger(GameEventType.sweeper);
                }
                if (Input.GetKeyDown(KeyCode.Keypad2))
                {
                    chat.LoadNPCDialogue(npc, 2);
                    EventManager.Trigger(GameEventType.sweeperKill);
                }
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

