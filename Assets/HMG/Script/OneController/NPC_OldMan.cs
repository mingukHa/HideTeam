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
    private bool isPlayerNearby = false; // �÷��̾ ���� ���� �ִ��� ����

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
        TextChange.text = "1.ī���ͷ� �ȳ��Ѵ�\n 2.�����Ѵ�\n 3.�����Ѵ�";
        Debug.Log($"������ ��ġ ���� {OldPos}");
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

        // �÷��̾ ���� ���� ���� �� Ű �Է� ó��
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
        Debug.Log("NPC�� �������� �����Ͽ� ������ϴ�.");
    }
}