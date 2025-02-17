using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using static EventManager;
using UnityEngine.AI;


public class NPC_CleanMan : NPCFSM
{

    
    public GameObject npcchatbox; //NPC의 메인 채팅 최상위
    private string npc = "Cleaner";
    public Transform GarbagePos; //이동 할 위치
    public Transform richKill;
    private bool GarbageTrue = false;
    private bool isHide = false;
    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.Garbage, StartGarbage);
        EventManager.Subscribe(GameEventType.RichKill, StartRichKill);
        EventManager.Subscribe(GameEventType.RichHide, StartRichHide);
        EventManager.Subscribe(GameEventType.RichNoHide, StartRichNoHide);
    }
    private void StartRichNoHide()
    {
        isHide = false;
    }
    private void StartRichHide()
    {
        isHide = true;
    }
    private void StartGarbage()
    {
        agent.speed = 3f;
        GarbageTrue = true;
        Debug.Log("청소부 개 빡쳐서 달려오는 중");
        agent.SetDestination(GarbagePos.transform.position);
        animator.SetBool("Run", true);
        NPCCollider.enabled = true;
    }
    private void StartRichKill()
    {
        if (GarbageTrue == false)
        agent.SetDestination(richKill.transform.position);
        chat.LoadNPCDialogue(npc, 3);
    }
    private IEnumerator RichFind()
    {

        yield return null;
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
            ChangeState(State.Talk);
            chat.LoadNPCDialogue(npc, 0);
        }
        if (Input.GetKey(KeyCode.F))
        {
            isDead = true;
        }

    }

    protected override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
        if (!isDead && other.CompareTag("Player"))
        {
            // 키 입력을 지속적으로 체크
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                chat.LoadNPCDialogue(npc, 1);
                StopCoroutine(TalkView());
                ScreenshotManager.Instance.CaptureScreenshot();
                Invoke("StopNpc",2f);

            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                chat.LoadNPCDialogue(npc, 2);
                StopCoroutine(TalkView());
                ScreenshotManager.Instance.CaptureScreenshot();
                Invoke("StopNpc", 2f);
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
}

