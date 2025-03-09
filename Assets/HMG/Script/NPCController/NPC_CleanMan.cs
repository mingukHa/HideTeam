using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using static EventManager;
using UnityEngine.AI;
using TMPro;
using UnityEngine.SocialPlatforms;


public class NPC_CleanMan : NPCFSM
{  
    public GameObject npcchatbox; //NPC�� ���� ä�� �ֻ���
    public Transform GarbagePos; //�̵� �� ��ġ
    public Transform richKill; //ȭ��� �� ��ġ
    public Transform richKillPos;//ȭ��� �� ��ġ
    public GameObject PlayerToiletOutPos;//�÷��̾ ������ Ȱ��ȭ �Ǵ� �ݶ��̴�
    public TextMeshProUGUI npcclean; //��� ���� �ʱ�ȭ 
    
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
            // Ű �Է��� ���������� üũ
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

    private void StartGarbage() //û�Һΰ� ������������ ȣ��Ǵ� �κ�
    {
        StartCoroutine(moutline.EventOutLine());
        ChangeState(State.Run);
        GarbageTrue = true;
        NPCCollider.enabled = true;
        //Debug.Log("û�Һ� ���ļ� �޷����� ��");
        //ù ��° �������� �̵�
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
    private IEnumerator RichFind() //���ڰ� �װ� �� �� ����Ǵ� �κ�
    {
        if (GarbageTrue == false) //������������ �̵����� �ʾҴٸ� ����
        {
            yield return new WaitForSeconds(1);
            ChangeState(State.Walk);
            agent.isStopped = false; //  �̵� �簳
            agent.SetDestination(richKillPos.position);

            // ������ ���� ���� (�ߺ� ����)
            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance || agent.velocity.magnitude > 0.1f)
            {
                yield return null;
            }
            ChangeState(State.Talk);
            if (isHide == false) //��ü�� ������ ���ϸ�
            {
                chat.LoadNPCDialogue(npc, 4);
                yield return new WaitForSeconds(3f);
                EventManager.Trigger(GameEventType.GameOver);
            }
            else //��ü�� ����ٸ�
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

