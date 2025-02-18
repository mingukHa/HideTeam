using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using static EventManager;
using UnityEngine.AI;
using UnityEngine.InputSystem.iOS;
using UnityEngine.UIElements;
using TMPro;


public class NPC_OldMan : NPCFSM
{
    
    public GameObject npcchatbox; //NPC의 메인 채팅 최상위
    private string npc = "OldMan";
    public Transform OldManPos; //이동 할 위치
    public Transform NewManPos;
    public TextMeshPro TextChange;
    private Transform OldPos;
    private bool isWalk = false;
    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.TellerTalk, OldmanMove);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEventType.TellerTalk, OldmanMove);
    }

    private void OldmanMove()
    {
        EventManager.Trigger(GameEventType.OldManGotoTeller);
        OldPos = NewManPos;
        TextChange.text = "1.카운터로 안내한다";
        Debug.Log($"늙은이 위치 변경 {OldPos}");
    }
    private void StopNpc()
    {
        StopCoroutine(TalkView());
        transform.rotation = initrotation;
        new WaitForSeconds(2f);
        NPCCollider.radius = 0.01f;
       // animator.SetTrigger("Idel");
        select.SetActive(false);
    }
    
    protected override void Start()
    {
        base.Start();
        chat = GetComponent<NPCChatTest>();    
        agent = GetComponent<NavMeshAgent>();
        OldPos = OldManPos;
        
    }

    protected override void Update()
    {
        base.Update();
        if (isWalk == true)
        {
            animator.SetTrigger("Walk");
        }


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
            new WaitForSeconds(1.5f);
            chat.LoadNPCDialogue(npc, 1);
        }
    }

    protected override void OnTriggerStay(Collider other)
    {
        if (!isDead && other.CompareTag("Player"))
        {
            // 키 입력을 지속적으로 체크
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                chat.LoadNPCDialogue(npc, 3);
                EventManager.Trigger(GameEventType.OldManHelp);
                returnManager.StartCoroutine(returnManager.SaveAllNPCData(3f));
                StopCoroutine(TalkView());
                ScreenshotManager.Instance.CaptureScreenshot();
                Invoke("StopNpc", 2f);
                Invoke("ReturnOldMan", 2f);

            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                chat.LoadNPCDialogue(npc, 7);
                EventManager.Trigger(GameEventType.OldManoutside);
                returnManager.StartCoroutine(returnManager.SaveAllNPCData(3f));
                StopCoroutine(TalkView());
                ScreenshotManager.Instance.CaptureScreenshot();
                Invoke("StopNpc", 2f);
                Invoke("ReturnOldMan", 2f);
                            
            }
            if (Input.GetKey(KeyCode.F))
            {
                EventManager.Trigger(EventManager.GameEventType.NPCKill);
                isDead = true;
            }
        }
    }
    
    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chat.LoadNPCDialogue("NULL", 0);
            
        }
    }
    private void ReturnOldMan()
    {
        if (!isDead)
        {
            isWalk = true;
            animator.SetTrigger("Walk");
            agent.isStopped = false; // 이동 활성화
            agent.SetDestination(OldPos.position);
            StartCoroutine(CheckArrival());
        }
    }

    private IEnumerator CheckArrival()
    {
        // NPC가 목적지에 도착할 때까지 대기
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance || agent.velocity.magnitude > 0.1f)
        {
            yield return null;
        }

        // 도착 후 멈추는 코드
        agent.isStopped = true; // 네비게이션 멈춤
        agent.ResetPath(); // 경로 초기화
        ChangeState(State.Talk);
        isWalk = false; // 이동 상태 해제

        Debug.Log("NPC가 목적지에 도착하여 멈췄습니다.");
    }


}

