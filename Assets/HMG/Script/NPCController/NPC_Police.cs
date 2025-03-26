using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using static EventManager;
using UnityEngine.AI;
using TMPro;


public class NPC_Polic : NPCFSM
{

    public GameObject npcchatbox; //NPC�� ���� ä�� �ֻ���
    public GameObject PolicPos; //�̵� �� ��ġ    
    private string npc = "NPC5";
    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.Carkick, StartPolicTlak);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEventType.Carkick, StartPolicTlak);
    }

    
    protected override void Start()
    {
        base.Start();
        select.SetActive(false);
        chat = GetComponent<NPCChatTest>();

        agent = GetComponent<NavMeshAgent>();
        NPCCollider = GetComponent<SphereCollider>();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (!isDead && other.CompareTag("Player"))
        {
            StartCoroutine(StartEndingStop());
        }

    }
    private IEnumerator StartEndingStop() //���� ���� �κ��� �Լ�
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
    private void StartPolicTlak() //�÷��̾ �ڵ����� �߷� ���� �޷� ���� �Լ�
    {
        StartCoroutine(moutline.EventOutLine());
        agent.SetDestination(PolicPos.gameObject.transform.position);
        NPCCollider.radius = 0.01f;
        ChangeState(State.Run);
        StartCoroutine(CheckArrivals()); 
    }
    protected IEnumerator CheckArrivals() //NPC�� ���� �ߴ��� �Ǵ��ϴ� �Լ�
    {
        // NPC�� �������� ������ ������ ���
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance || agent.velocity.magnitude > 0.1f)
        {//  ��� ����� �ϰ� �ִ���,���� �Ÿ��� stoppingDistance���� ū��, �ӵ��� ������ ������ �ʾҴ��� Ȯ��
            yield return null;
        }
        // ���� �� ���ߴ� �ڵ�
        agent.isStopped = true; // �׺���̼� ����
        agent.ResetPath(); // ��� �ʱ�ȭ
        chat.LoadNPCDialogue("Null", 0);
        ChangeState(State.Idle);
    }
}

