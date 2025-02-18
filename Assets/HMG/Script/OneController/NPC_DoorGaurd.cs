using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using static EventManager;
using UnityEngine.AI;

public class NPC_DoorGaurd : NPCFSM
{
    public GameObject npcchatbox; // NPC�� ���� ä�� �ֻ���
    private string npc = "NPC1";
    public SphereCollider sphereCollider;
    private bool isChasing = false; // ���� ������ üũ
    private Transform playerTransform; // �÷��̾� Transform�� ���������� ������Ʈ

    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.NPCKill, StartNPCKill);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEventType.NPCKill, StartNPCKill);
    }

    private void StartNPCKill()
    {
        ChangeState(State.Run);
        sphereCollider.radius = 3.5f;        
        StartCoroutine(ChasePlayer());
    }

    private IEnumerator ChasePlayer()
    {
        yield return new WaitForSeconds(2f);
        animator.SetTrigger("Run");
            // NPC�� �����ߴ��� Ȯ��
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                isChasing = false; // ���� ����
                agent.isStopped = true; //  NavMeshAgent ���߱�
                agent.ResetPath(); //  ��� �ʱ�ȭ (���ʿ��� ������ ����)

                Debug.Log("NPC�� �����߽��ϴ�!");
                StartCoroutine(TalkView());
                animator.SetTrigger("Talk");
                chat.LoadNPCDialogue(npc, 1);

                yield return new WaitForSeconds(2f);
                EventManager.Trigger(GameEventType.GameOver);
            }

 
        
    }


    private void StopNpc()
    {
        StopCoroutine(TalkView());
        isChasing = false;
        transform.rotation = initrotation;
        NPCCollider.radius = 0.01f;
        animator.SetTrigger("Idle");
        select.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        select.SetActive(false);
        chat = GetComponent<NPCChatTest>();

        agent = GetComponent<NavMeshAgent>();
        NPCCollider = GetComponent<SphereCollider>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
}
