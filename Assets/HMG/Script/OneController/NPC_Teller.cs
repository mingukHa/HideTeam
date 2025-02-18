using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using static EventManager;
using UnityEngine.AI;


public class NPC_Teller : NPCFSM
{

    public GameObject npcchatbox; //NPC�� ���� ä�� �ֻ���
    private string npc = "NPC6";

    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.OldManHelp, OldManTalk);
        EventManager.Subscribe(GameEventType.OldManoutside, OldManTalk);
    }
    private void OnDisble()
    {
        EventManager.Unsubscribe(GameEventType.OldManHelp, OldManTalk);
        EventManager.Unsubscribe(GameEventType.OldManoutside, OldManTalk);
    }
    private void OldManTalk()
    {
        //�ҹ����� �� �ɸ� �����ϴ� ����
    }
    private void StopNpc()
    {
        StopCoroutine(TalkView());
        transform.rotation = initrotation;
        new WaitForSeconds(2f);
        NPCCollider.radius = 0.01f;
        animator.SetTrigger("Idle");
        select.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        chat = GetComponent<NPCChatTest>();
        agent = GetComponent<NavMeshAgent>();
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

        chat.LoadNPCDialogue("NULL", 0);
    }

    protected override void OnTriggerEnter(Collider other)
    {

        if (!isDead && other.CompareTag("Player"))
        {
            chat.LoadNPCDialogue(npc, 0);
        }
    }

    protected override void OnTriggerStay(Collider other)
    {
        if (!isDead && other.CompareTag("Player"))
        {
            // Ű �Է��� ���������� üũ
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                chat.LoadNPCDialogue(npc, 1);
                EventManager.Trigger(GameEventType.TellerTalk); //�ڷ��� �� �ϸ� ������ �̺�Ʈ
                returnManager.StartCoroutine(returnManager.SaveAllNPCData(3f));
                StopCoroutine(TalkView());
                Invoke("StopNpc", 2f);

            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                chat.LoadNPCDialogue(npc, 2);
                EventManager.Trigger(GameEventType.TellerTalk); //�ڷ��� �� �ϸ� ������ �̺�Ʈ
                returnManager.StartCoroutine(returnManager.SaveAllNPCData(3f));
                StopCoroutine(TalkView());
                Invoke("StopNpc", 2f);

            }
            if (Input.GetKey(KeyCode.F))
            {
                isDead = true;
            }
        }
    }
    private void ReturnOldMan()
    {
        if (!isDead)
        {
            animator.SetTrigger("Idle");
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

