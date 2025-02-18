using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using static EventManager;
using UnityEngine.AI;

public class NPC_DoorGaurd : NPCFSM
{
    public GameObject npcchatbox; // NPC의 메인 채팅 최상위
    private string npc = "NPC1";
    public SphereCollider sphereCollider;
    private bool isChasing = false; // 추적 중인지 체크
    private Transform playerTransform; // 플레이어 Transform을 지속적으로 업데이트

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
            // NPC가 도착했는지 확인
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                isChasing = false; // 추적 중지
                agent.isStopped = true; //  NavMeshAgent 멈추기
                agent.ResetPath(); //  경로 초기화 (불필요한 움직임 방지)

                Debug.Log("NPC가 도착했습니다!");
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
