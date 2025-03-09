using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using static EventManager;
using UnityEngine.AI;
using TMPro;
using UnityEngine.SocialPlatforms;


public class NPC_CleanMan : NPCFSM
{  
    public GameObject npcchatbox; //NPC의 메인 채팅 최상위
    public Transform GarbagePos; //이동 할 위치
    public Transform richKill; //화장실 앞 위치
    public Transform richKillPos;//화장실 안 위치
    public GameObject PlayerToiletOutPos;//플레이어가 나가면 활성화 되는 콜라이더
    public TextMeshProUGUI npcclean; //대사 강제 초기화 
    
    private string npc = "Cleaner";
    private bool GarbageTrue = false;
    private bool isHide = false;
    private bool checkOneDead = false;
  
    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.Garbage, StartGarbage);
        EventManager.Subscribe(GameEventType.RichKill, StartRichKill);
        EventManager.Subscribe(GameEventType.RichHide, StartRichHide);
        EventManager.Subscribe(GameEventType.RichNoHide, StartRichNoHide);
        EventManager.Subscribe(GameEventType.PlayerToiletOut, RichFinds);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEventType.Garbage, StartGarbage);
        EventManager.Unsubscribe(GameEventType.RichKill, StartRichKill);
        EventManager.Unsubscribe(GameEventType.RichHide, StartRichHide);
        EventManager.Unsubscribe(GameEventType.RichToiletKill, StartRichNoHide);
        EventManager.Unsubscribe(GameEventType.PlayerToiletOut, RichFinds);
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
        if (!isDead && isPlayerNearby)
        {
            // 키 입력을 지속적으로 체크
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                chat.LoadNPCDialogue(npc, 1);
                StopCoroutine(TalkView());
                EventManager.Trigger(GameEventType.CleanManTalk);
                Invoke("StopNpc", 2f);
                StartCoroutine(CleanManIdle());
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                chat.LoadNPCDialogue(npc, 2);
                StopCoroutine(TalkView());
                EventManager.Trigger(GameEventType.CleanManTalk);
                Invoke("StopNpc", 2f);
                StartCoroutine(CleanManIdle());
            }
        }
        if (!checkOneDead)
        {
            if (isDead)
            {
                checkOneDead = true;
                EventManager.Trigger(GameEventType.CleanManDie);
            }
        }
    }
   
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (!isDead && other.CompareTag("Player"))
        {
            ChangeState(State.Talk);
            chat.LoadNPCDialogue(npc, 0);
        }
    }
    private void StartRichNoHide()
    {
        isHide = false;
    }
    private void StartRichHide()
    {
        isHide = true;
        StartCoroutine(moutline.EventOutLine());
    }

    private void StartGarbage() //청소부가 쓰레기장으로 호출되는 부분
    {
        StartCoroutine(moutline.EventOutLine());
        ChangeState(State.Run);
        GarbageTrue = true;
        NPCCollider.enabled = true;
        //Debug.Log("청소부 빡쳐서 달려오는 중");
        //첫 번째 목적지로 이동
        agent.SetDestination(GarbagePos.transform.position);
        StartCoroutine(CheckArrival());

    }
    private void StartRichKill()
    {
        if (GarbageTrue == false)
        {
            StartCoroutine(moutline.EventOutLine());
            agent.SetDestination(richKill.transform.position);
            chat.LoadNPCDialogue(npc, 3);
            PlayerToiletOutPos.SetActive(true);           
            StartCoroutine(CheckArrival());
        }
    }
    
    private void RichFinds()
    {
        StartCoroutine(RichFind());
        StartCoroutine(moutline.EventOutLine());
    }
    private IEnumerator RichFind() //부자가 죽고 난 후 실행되는 부분
    {
        if (GarbageTrue == false) //쓰레기장으로 이동하지 않았다면 실행
        {
            yield return new WaitForSeconds(1);
            ChangeState(State.Walk);
            agent.isStopped = false; //  이동 재개
            agent.SetDestination(richKillPos.position);

            // 목적지 도착 감지 (중복 방지)
            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance || agent.velocity.magnitude > 0.1f)
            {
                yield return null;
            }
            ChangeState(State.Talk);
            if (isHide == false) //시체를 숨기지 못하면
            {
                chat.LoadNPCDialogue(npc, 4);
                yield return new WaitForSeconds(3f);
                EventManager.Trigger(GameEventType.GameOver);
            }
            else //시체를 숨겼다면
            {
                chat.LoadNPCDialogue(npc, 5);
                yield return new WaitForSeconds(3f);
                npcclean.text = "";
                ChangeState(State.Idle);
                EventManager.Trigger(GameEventType.OldManOut);
            }
        }
    }
    private IEnumerator CleanManIdle()
    {
        yield return new WaitForSeconds(2f);
        ChangeState (State.Idle);
    }
}

