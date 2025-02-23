using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using static EventManager;
using UnityEngine.AI;
using TMPro;


public class NPC_Polic : NPCFSM
{

    public GameObject npcchatbox; //NPC의 메인 채팅 최상위
    private string npc = "NPC5";
    public GameObject PolicPos; //이동 할 위치    
    private bool isPolicTlak = false;
    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.Carkick, StartPolicTlak);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEventType.Carkick, StartPolicTlak);
    }

    private void StartPolicTlak()
    {
        StartCoroutine(moutline.EventOutLine());
        agent.SetDestination(PolicPos.gameObject.transform.position);
        ChangeState(State.Run);

        // 목표 지점 도착 확인을 위한 코루틴 실행
        StartCoroutine(CheckArrival());
    }

    private IEnumerator CheckArrival()
    {
        // NPC가 이동하는 동안 반복 체크
        while (agent.pathPending || agent.remainingDistance > 0.1f)
        {
            yield return null;
        }

        // NPC가 도착했을 때 처리
        agent.isStopped = true;
        ChangeState(State.Idle);

        NPCCollider.enabled = false;        
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
        base.Update();
        if (Input.GetKey(KeyCode.F))
        {
            //EventManager.Trigger(GameEventType.NPCKill);
            isDead = true;
        }
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
            chat.LoadNPCDialogue(npc, 0);
        }

    }
    protected override void OnTriggerStay(Collider other)
    {
        
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chat.LoadNPCDialogue("NULL", 0);
        }

    }
}

