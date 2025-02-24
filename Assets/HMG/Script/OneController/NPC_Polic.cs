using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using static EventManager;
using UnityEngine.AI;
using TMPro;


public class NPC_Polic : NPCFSM
{

    public GameObject npcchatbox; //NPC�� ���� ä�� �ֻ���
    private string npc = "NPC5";
    public GameObject PolicPos; //�̵� �� ��ġ    
    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.Carkick, StartPolicTlak);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEventType.Carkick, StartPolicTlak);
    }

    private void StartPolicTlak()
    {
        StartCoroutine(moutline.EventOutLine());
        agent.SetDestination(PolicPos.gameObject.transform.position);
        ChangeState(State.Run);

        // ��ǥ ���� ���� Ȯ���� ���� �ڷ�ƾ ����
        StartCoroutine(CheckArrival());
    }

    private IEnumerator CheckArrival()
    {
        // NPC�� �̵��ϴ� ���� �ݺ� üũ
        while (agent.pathPending || agent.remainingDistance > 0.1f)
        {
            yield return null;
        }

        // NPC�� �������� �� ó��
        agent.isStopped = true;
        ChangeState(State.Idle);
        StopNpc();
        NPCCollider.enabled = false;        
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

    protected override void OnTriggerEnter(Collider other)
    {
        if (!isDead && other.CompareTag("Player"))
        {
            StartCoroutine(StartEndingStop());
            //�� �κ� �������� �߰�
        }

    }
    protected override void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.F))
            {
                EventManager.Trigger(GameEventType.NPCKill);
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
    private IEnumerator StartEndingStop()
    {
        //�÷��̾� ���� ��� �߰��ؾ���
        EventManager.Trigger(GameEventType.EndingStop);
        EventManager.Trigger(GameEventType.TellerTalk);
        StartCoroutine(TalkView());        
        chat.LoadNPCDialogue(npc, 0);
        ChangeState(State.Talk);
        yield return new WaitForSeconds(5f);
        EventManager.Trigger(GameEventType.GameOver);
    }
}

