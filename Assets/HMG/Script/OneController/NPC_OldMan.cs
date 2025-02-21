using UnityEngine;
using System.Collections;
using static EventManager;
using UnityEngine.AI;
using TMPro;

public class NPC_OldMan : NPCFSM
{
    public GameObject npcchatbox;
    private string npc = "OldMan";
    public Transform OldManPos;
    public Transform NewManPos;
    public TextMeshPro TextChange;
    private Transform OldPos;
    private bool isWalk = false;
    private bool isPlayerNearby = false; // 플레이어가 범위 내에 있는지 여부

    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.TellerTalk, OldmanMove);
        EventManager.Subscribe(GameEventType.OldManOut, OldmanOut);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEventType.TellerTalk, OldmanMove);
        EventManager.Unsubscribe(GameEventType.OldManOut, OldmanOut);
    }
    private void OldmanOut()
    {
        ChangeState(State.Walk);
        agent.SetDestination(OldManPos.position);
    }
    private void OldmanMove()
    {
        EventManager.Trigger(GameEventType.OldManGotoTeller);
        OldPos = NewManPos;
        TextChange.text = "1.카운터로 안내한다\n 2.무시한다\n 3.제압한다";
        Debug.Log($"늙은이 위치 변경 {OldPos}");
    }
    private void StopNpc()
    {
        StopCoroutine(TalkView());
        transform.rotation = initrotation;
        new WaitForSeconds(2f);
        NPCCollider.radius = 0.01f;
        select.SetActive(false);
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
                EventManager.Trigger(GameEventType.OldManHelp);
                returnManager.StartCoroutine(returnManager.SaveAllNPCData(3f));
                ScreenshotManager.Instance.CaptureScreenshot();
                Invoke("StopNpc", 2f);
                Invoke("ReturnOldMan", 2f);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                chat.LoadNPCDialogue(npc, 7);
                EventManager.Trigger(GameEventType.OldManoutside);
                returnManager.StartCoroutine(returnManager.SaveAllNPCData(3f));
                ScreenshotManager.Instance.CaptureScreenshot();
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
            chat.LoadNPCDialogue(npc, 0);
            new WaitForSeconds(1.5f);
            chat.LoadNPCDialogue(npc, 1);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
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
        }
    }

    private IEnumerator CheckArrival()
    {
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance || agent.velocity.magnitude > 0.1f)
        {
            yield return null;
        }

        agent.isStopped = true;
        agent.ResetPath();
        ChangeState(State.Talk);
        isWalk = false;
        Debug.Log("NPC가 목적지에 도착하여 멈췄습니다.");
    }
}