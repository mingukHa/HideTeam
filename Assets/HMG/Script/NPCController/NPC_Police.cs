using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using static EventManager;
using UnityEngine.AI;
using TMPro;


public class NPC_Polic : NPCFSM
{

    public GameObject npcchatbox; //NPC의 메인 채팅 최상위
    public GameObject PolicPos; //이동 할 위치    
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
    private IEnumerator StartEndingStop() //게임 오버 부분의 함수
    {
        //플레이어 정지 기능 추가해야함
        EventManager.Trigger(GameEventType.EndingStop);
        EventManager.Trigger(GameEventType.TellerTalk);
        StartCoroutine(TalkView());        
        chat.LoadNPCDialogue(npc, 0);
        ChangeState(State.Talk);
        yield return new WaitForSeconds(5f);
        EventManager.Trigger(GameEventType.GameOver);
    }
    private void StartPolicTlak() //플레이어가 자동차를 발로 차면 달려 가는 함수
    {
        StartCoroutine(moutline.EventOutLine());
        agent.SetDestination(PolicPos.gameObject.transform.position);
        NPCCollider.radius = 0.01f;
        ChangeState(State.Run);
        StartCoroutine(CheckArrivals()); 
    }
    protected IEnumerator CheckArrivals() //NPC가 도착 했는지 판단하는 함수
    {
        // NPC가 목적지에 도착할 때까지 대기
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance || agent.velocity.magnitude > 0.1f)
        {//  경로 계산을 하고 있는지,남은 거리가 stoppingDistance보다 큰지, 속도가 완전히 멈추지 않았는지 확인
            yield return null;
        }
        // 도착 후 멈추는 코드
        agent.isStopped = true; // 네비게이션 멈춤
        agent.ResetPath(); // 경로 초기화
        chat.LoadNPCDialogue("Null", 0);
        ChangeState(State.Idle);
    }
}

