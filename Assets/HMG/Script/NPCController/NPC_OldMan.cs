using UnityEngine;
using System.Collections;
using static EventManager;
using UnityEngine.AI;
using TMPro;

public class NPC_OldMan : NPCFSM
{
    public GameObject npcchatbox;
    public Transform OldManPos;
    public Transform NewManPos;
    public TextMeshPro TextChange;
    private string npc = "OldMan";
    private Transform OldPos;
    private bool isWalk = false;

    private bool isHelped = false;


    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.TellerTalk, OldmanMove);
        EventManager.Subscribe(GameEventType.RichKill, OldmanOut);
       
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEventType.TellerTalk, OldmanMove);
        EventManager.Unsubscribe(GameEventType.RichKill, OldmanOut);
    }    
    protected override void Start()
    {
        base.Start();
        chat = GetComponent<NPCChatTest>();
        agent = GetComponent<NavMeshAgent>();
        OldPos = OldManPos;
        agent.avoidancePriority = 50;
    }

    protected override void Update()
    {
        base.Update();
        if (isWalk)
        {
            animator.SetTrigger("Walk");
        }
        // 플레이어가 범위 내에 있을 때 키 입력 처리
        if (isPlayerNearby && !isDead)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                chat.LoadNPCDialogue(npc, 3);
                if (isHelped)
                {
                    EventManager.Trigger(GameEventType.OldManMovingCounter);
                }
                else
                {
                    EventManager.Trigger(GameEventType.OldManHelp);
                }
                
                Invoke("StopNpc", 2f);
                Invoke("ReturnOldMan", 2f);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                chat.LoadNPCDialogue(npc, 7);
                EventManager.Trigger(GameEventType.OldManoutside);
                
                Invoke("StopNpc", 2f);
                Invoke("ReturnOldMan", 2f);
            }
            if (Input.GetKey(KeyCode.F))
            {
                EventManager.Trigger(GameEventType.NPCKill);
                isDead = true;
            }
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (!isDead && other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            chat.LoadNPCDialogue(npc, 1);
        }
    }
    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            chat.LoadNPCDialogue("NULL", 0);
        }
    }

    private void ReturnOldMan()
    {
        if (!isDead)
        {
            isWalk = true;
            animator.SetTrigger("Walk");
            agent.isStopped = false;
            agent.SetDestination(OldPos.position);
            StartCoroutine(CheckArrival());
            StartCoroutine(OldManarrive());

        }
    }
    private IEnumerator OldManarrive()
    {
        // NPC가 목적지에 도착할 때까지 대기
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance || agent.velocity.magnitude > 0.1f)
        {//  경로 계산을 하고 있는지,남은 거리가 stoppingDistance보다 큰지, 속도가 완전히 멈추지 않았는지 확인
            yield return null;
        }
        agent.isStopped = true;   // 네비게이션 정지
        agent.ResetPath();        // 경로 초기화
        agent.velocity = Vector3.zero; // 강제로 속도 0 설정
        isWalk = false;
        animator.ResetTrigger("Walk");
        ChangeState(State.Talk);
    }
    private void OldmanOut()
    {
        ChangeState(State.Walk);
        EventManager.Trigger(GameEventType.OldManOut);
        StartCoroutine(moutline.EventOutLine());
        agent.SetDestination(OldManPos.position);
    }
    private void OldmanMove()
    {
        EventManager.Trigger(GameEventType.OldManGotoTeller);
        isHelped = true;
        StartCoroutine(moutline.EventOutLine());
        OldPos = NewManPos;
        TextChange.text = "1.카운터로 안내한다";
        Debug.Log($"늙은이 위치 변경 {OldPos}");
    }

}