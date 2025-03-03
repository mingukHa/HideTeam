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
    private void StartPolicTlak()
    {
        StartCoroutine(moutline.EventOutLine());
        agent.SetDestination(PolicPos.gameObject.transform.position);
        ChangeState(State.Run);
        StartCoroutine(CheckArrival());
    }
}

