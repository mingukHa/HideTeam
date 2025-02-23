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
    private string npc = "Cleaner";
    public Transform GarbagePos; //�̵� �� ��ġ
    
    public Transform richKill;
    public Transform richKillPos;
    private bool GarbageTrue = false;
    private bool isHide = false;
    public TextMeshProUGUI npcclean;


    public GameObject PlayerToiletOutPos;
    
    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.Garbage, StartGarbage);
        EventManager.Subscribe(GameEventType.RichKill, StartRichKill);
        EventManager.Subscribe(GameEventType.RichHide, StartRichHide);
        EventManager.Subscribe(GameEventType.RichNoHide, StartRichNoHide);
        EventManager.Subscribe(GameEventType.RichToiletKill, StartRichToiletKill);
        EventManager.Subscribe(GameEventType.PlayerToiletOut, RichFinds);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEventType.Garbage, StartGarbage);
        EventManager.Unsubscribe(GameEventType.RichKill, StartRichKill);
        EventManager.Unsubscribe(GameEventType.RichHide, StartRichHide);
        EventManager.Unsubscribe(GameEventType.RichToiletKill, StartRichNoHide);
        EventManager.Unsubscribe(GameEventType.RichToiletKill, StartRichToiletKill);
        EventManager.Unsubscribe(GameEventType.PlayerToiletOut, RichFinds);
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
                ScreenshotManager.Instance.CaptureScreenshot();
                EventManager.Trigger(GameEventType.CleanManTalk);
                Invoke("StopNpc", 2f);

            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                chat.LoadNPCDialogue(npc, 2);
                StopCoroutine(TalkView());
                ScreenshotManager.Instance.CaptureScreenshot();
                EventManager.Trigger(GameEventType.CleanManTalk);
                Invoke("StopNpc", 2f);
            }
            
            if (Input.GetKey(KeyCode.F))
            {
                EventManager.Trigger(GameEventType.CleanManDie);
                agent.enabled = false;
                isDead = true;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(TalkView());
            }
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
    private void StartRichToiletKill()
    {
        //StartCoroutine(RichFind());
    }
    private void StartGarbage()
    {
        StartCoroutine(moutline.EventOutLine());
        GarbageTrue = true;
        Debug.Log("û�Һ� �� ���ļ� �޷����� ��");

        // ù ��° �������� �̵�
        agent.SetDestination(player.transform.position);

        animator.SetBool("Run", true);
        NPCCollider.enabled = true;

        
    }

    //private IEnumerator MoveToSecondPosition()
    //{
    //    // ù ��° �������� ������ ������ ���
    //    while (agent.pathPending || agent.remainingDistance > 0.5f)
    //    {
    //        yield return null;
    //    }

    //    Debug.Log("ù ��° ������ ����, �� ��° ��ġ�� �̵�");

    //    // �� ��° �������� �̵�
    //    agent.SetDestination(GarbagePos1.transform.position);
    //}
    public void Walk()
    {

    }
    private void StartRichKill()
    {
        if (GarbageTrue == false)
        {
            ScreenshotManager.Instance.CaptureScreenshot();
            StartCoroutine(moutline.EventOutLine());
            agent.SetDestination(richKill.transform.position);
            chat.LoadNPCDialogue(npc, 3);
            PlayerToiletOutPos.SetActive(true);
            StartCoroutine(CheckArrival());
        }
    }
    private IEnumerator CheckArrival()
    {
        if (GarbageTrue == false)
        {
            // NPC�� �������� ������ ������ ���
            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance || agent.velocity.magnitude > 0.1f)
            {
                yield return null;
            }

            // ���� �� ���ߴ� �ڵ�
            agent.isStopped = true; // �׺���̼� ����
            agent.ResetPath(); // ��� �ʱ�ȭ
            ChangeState(State.Talk);
            chat.LoadNPCDialogue("Null", 0);
            Debug.Log("NPC�� �������� �����Ͽ� ������ϴ�.");
        }
    }
    private void RichFinds()
    {
        StartCoroutine (RichFind());
        StartCoroutine(moutline.EventOutLine());
    }
    private IEnumerator RichFind()
    {
        if (GarbageTrue == false)
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
            if (isHide == false)
            {
                chat.LoadNPCDialogue(npc, 4);
                yield return new WaitForSeconds(3f);
                EventManager.Trigger(GameEventType.GameOver);
            }
            else
            {
                chat.LoadNPCDialogue(npc, 5);
                yield return new WaitForSeconds(3f);
                Debug.Log("����������");
                npcclean.text = "";
                Debug.Log("������");
                ChangeState(State.Idle);
                EventManager.Trigger(GameEventType.OldManOut);
            }
        }
    }
    
    private void StopNpc()
    {
        StopCoroutine(TalkView());
        chat.LoadNPCDialogue("NULL", 0);
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
        agent.enabled = false;
        chat.LoadNPCDialogue("NULL", 0);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (!isDead && other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            ChangeState(State.Talk);
            chat.LoadNPCDialogue(npc, 0);
        }
        if (Input.GetKey(KeyCode.F))
        {
            isDead = true;
            chat.LoadNPCDialogue("NULL", 0);
        }

    }

    protected override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
        
        
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
}

