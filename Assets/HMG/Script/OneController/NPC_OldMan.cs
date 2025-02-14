using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using static EventManager;
using UnityEngine.AI;
using UnityEngine.InputSystem.iOS;
using UnityEngine.UIElements;
using TMPro;


public class NPC_OldMan : NPCFSM
{
    
    public GameObject npcchatbox; //NPC�� ���� ä�� �ֻ���
    private string npc = "NPC3";
    public Transform OldManPos; //�̵� �� ��ġ
    public Transform NewManPos;
    public TextMeshPro TextChange;
    private Transform OldPos;
    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.TellerTalk, OldmanMove);
    }
    private void OldmanMove()
    {
        OldPos = NewManPos;
        TextChange.text = "1.ī���ͷ� �ȳ��Ѵ�";
        Debug.Log($"������ ��ġ ���� {OldPos}");
    }
    private void StopNpc()
    {
        StopCoroutine(TalkView());
        transform.rotation = initrotation;
        new WaitForSeconds(2f);
        NPCCollider.radius = 0.01f;
       // animator.SetTrigger("Idel");
        select.SetActive(false);
    }
    
    protected override void Start()
    {
        base.Start();
        chat = GetComponent<NPCChatTest>();    
        agent = GetComponent<NavMeshAgent>();
        OldPos = OldManPos;
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
                EventManager.Trigger(GameEventType.OldManHelp);
                returnManager.StartCoroutine(returnManager.SaveAllNPCData(3f));
                StopCoroutine(TalkView());
                Invoke("StopNpc", 2f);
                Invoke("ReturnOldMan", 2f);

            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                chat.LoadNPCDialogue(npc, 2);
                EventManager.Trigger(GameEventType.OldManoutside);
                returnManager.StartCoroutine(returnManager.SaveAllNPCData(3f));
                StopCoroutine(TalkView());
                Invoke("StopNpc", 2f);
                Invoke("ReturnOldMan", 2f);
                            
            }
            if (Input.GetKey(KeyCode.F))
            {
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
    private void ReturnOldMan()
    {
        if (!isDead)
        {
            animator.SetTrigger("Walk");
            agent.SetDestination(OldPos.position);
            StartCoroutine(CheckArrival());
        }
    }
    private IEnumerator CheckArrival()
    {
        // ��ǥ ������ ������ ������ ���
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        // ���� �� ���ߴ� �ڵ�
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.isStopped = true; // �׺���̼� ����
            agent.ResetPath(); // ��� �ʱ�ȭ
            animator.SetTrigger("Idle"); // Idle �ִϸ��̼����� ����
            Debug.Log("NPC�� �������� �����Ͽ� ������ϴ�.");
        }
    }

}

