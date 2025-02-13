using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using static EventManager;
using UnityEngine.AI;


public class NPC_CleanMan : NPCFSM
{

    
    public GameObject npcchatbox; //NPC�� ���� ä�� �ֻ���
    private string npc = "NPC4";
    public Transform GarbagePos; //�̵� �� ��ġ
    public Transform richKill;
    private bool GarbageTrue = false;
    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.Garbage, StartGarbage);
        EventManager.Subscribe(GameEventType.RichKill, StartRichKill);
    }
    private void StartGarbage()
    {
        GarbageTrue = true;
        Debug.Log("û�Һ� �� ���ļ� �޷����� ��");
        agent.SetDestination(GarbagePos.transform.position);
        animator.SetBool("Run", true);
        NPCCollider.enabled = true;
    }
    private void StartRichKill()
    {
        if (GarbageTrue == false)
        agent.SetDestination(richKill.transform.position);
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
        if (Input.GetKeyDown(KeyCode.F))
        {
            isDead = true;
        }

    }

    protected override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
        if (!isDead && other.CompareTag("Player"))
        {
            // Ű �Է��� ���������� üũ
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                chat.LoadNPCDialogue(npc, 1);
                StopCoroutine(TalkView());
                Invoke("StopNpc",2f);

            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                chat.LoadNPCDialogue(npc, 2);
                StopCoroutine(TalkView());
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

