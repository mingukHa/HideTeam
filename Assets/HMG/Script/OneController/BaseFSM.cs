using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class NPCFSM : MonoBehaviour
{
    protected enum State { Idle, Look, Walk, Run, Talk, Dead }
    protected State currentState = State.Idle;
    protected Transform player; //플레이어 위치
    protected Animator animator;
    private Rigidbody[] rigidbodies; //레그돌 받아오는 부분
    public bool isDead = false; //죽음 상태
    private bool isTalking = false; //대화 상태
    //private bool isText = false; //죽으면 채팅 끄기 
    public bool isRagdollActivated = false; // 레그돌 활성화 여부 확인용 // PlayerController에서 사용하기 위해 public으로 수정
    protected Quaternion initrotation; //기본 위치
    private NPCChatTest NPCChatTest; //NPC대화 불러오는 곳
    public SphereCollider NPCCollider; //NPC 상호작용 콜라이더
    protected int currentWaypointIndex; //네브메쉬 배열 초기 값
    protected NavMeshAgent agent; //네브메쉬
    [SerializeField]
    protected GameObject select; //캐릭터 말풍선
    private bool isPlayerNearby = false;
    public ReturnManagerinit returnManager;
    protected NPCChatTest chat;
    protected Moutline moutline;
    protected virtual void Start()
    {
        chat = GetComponent<NPCChatTest>();
        animator = GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        NPCChatTest = GetComponent<NPCChatTest>();
        // 레그돌 초기 비활성화
        SetRagdollState(false);
        ChangeState(State.Idle);
        initrotation = transform.rotation;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        moutline = GetComponent<Moutline>();
        agent.autoBraking = false;

    }

    protected virtual void Update()
    {
        // 상태 업데이트
        switch (currentState)
        {
            case State.Idle:
                IdleBehavior();
                break;
            case State.Look:
                LookBehavior();
                break;
            case State.Walk:
                WalkBehavior();
                break;
            case State.Run:
                RunBehavior();
                break;
            case State.Talk:
                TalkBehavior();
                break;
            case State.Dead:
                DeadBehavior();
                break;
        }
        if (isDead && !isRagdollActivated)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // 애니메이션 "Dead"가 실행 중인지 확인
            if (stateInfo.IsName("Dead") && stateInfo.normalizedTime >= 1.0f)
            {
                //Debug.Log(" Dead 애니메이션 종료 - 레그돌 활성화 실행");
                ActivateRagdoll(); //  레그돌 처리
            }
        }
    }

    protected virtual void ChangeState(State newState)
    {
        currentState = newState;

        if (newState == State.Dead)
        {
            isRagdollActivated = false; // Dead 상태 초기화
            animator.ResetTrigger("Idle");
            animator.ResetTrigger("Look");
            animator.ResetTrigger("Walk");
            animator.ResetTrigger("Run");
            animator.ResetTrigger("Talk");
            animator.ResetTrigger("Dead");
            animator.SetTrigger("Dead");
            return;
        }

        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Look");
        animator.ResetTrigger("Walk");
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Talk");
        animator.ResetTrigger("Dead");

        switch (newState)
        {
            case State.Idle:
                animator.SetTrigger("Idle");
                break;
            case State.Look:
                animator.SetTrigger("Look");
                break;
            case State.Walk:
                animator.SetTrigger("Walk");
                break;
            case State.Run:
                animator.SetTrigger("Run");
                break;
            case State.Talk:
                animator.SetTrigger("Talk");
                break;
        }
    }

    protected void ActivateRagdoll()
    {
        if (isRagdollActivated) return; //  중복 실행 방지
        isDead = true;
        animator.enabled = false; //  애니메이션 정지
        SetRagdollState(true); //  물리 적용

        isRagdollActivated = true; //  이미 실행되었음을 저장

        //해당 오브젝트의 자식까지 전부 태그를 Ragdoll로 바꿈
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            child.gameObject.tag = "Ragdoll";
        }
    }

    private void SetRagdollState(bool state)
    {
        //Debug.Log($" 레그돌 상태 변경: {(state ? "활성화" : "비활성화")}");
        foreach (var rb in rigidbodies)
        {
            //gameObject.tag = "Ragdoll";
            rb.isKinematic = !state; //  Rigidbody 물리 활성화
        }
    }

    // 각 상태의 기본 행동
    protected virtual void IdleBehavior() { }
    protected virtual void LookBehavior() { }
    protected virtual void WalkBehavior() { }
    protected virtual void RunBehavior() { }
    protected virtual void TalkBehavior()
    {
        
    }
    protected virtual void DeadBehavior()
    {
        //Debug.Log("데드 비헤이어 실행");
        isDead = true;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!isDead && other.CompareTag("Player"))
        {
            //Debug.Log("플레이어 범위 내 진입");
            isPlayerNearby = true; // 플레이어가 범위 내에 있음을 저장
        }
        //if (other.CompareTag("NPC") || other.CompareTag("Player"))
        //{
        //    animator.SetTrigger("Hit");
        //}
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (!isDead && other.CompareTag("Player"))
        {
            StopCoroutine(TalkView());
            //Debug.Log("플레이어 범위 밖으로 나감");
            ChangeState(State.Idle);
            isPlayerNearby = false; // 범위를 벗어나면 초기화
            isTalking = false; // 대화 종료
            transform.rotation = initrotation; // 원래 방향으로 복귀
            select.SetActive(false);
        }
    }
    protected virtual void OnTriggerStay(Collider other)
    {
        if (!isDead && other.CompareTag("Player"))
        {
            // E 키를 눌렀을 때만 NPC가 플레이어를 바라보며 대화 시작
            if (Input.GetKeyDown(KeyCode.E) && !isTalking)
            {
                //Debug.Log("NPC가 플레이어를 바라보며 대화 시작");
                StartCoroutine(TalkView());
                ChangeState(State.Talk); // 대화 상태로 변경
            }

            // F 키를 눌렀을 때 Dead 상태 전환
            if (Input.GetKey(KeyCode.F) && currentState != State.Dead)
            {
                ChangeState(State.Dead);
                NPCChatTest.enabled = false;
                moutline.enabled = false;
                EventManager.Trigger(EventManager.GameEventType.NPCKill);
                isTalking = false;
                select.SetActive(false);
                chat.LoadNPCDialogue("NULL", 0);
            }

            // Dead 모션이 끝났는지 확인
            if (currentState == State.Dead && !isRagdollActivated)
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("Dead") && stateInfo.normalizedTime >= 1.0f)
                {
                    //Debug.Log("NPC 죽음");
                    
                    ActivateRagdoll();
                    isRagdollActivated = true;
                }
            }
        }
    }
   
    protected IEnumerator TalkView()
    {
        if (isTalking || agent.hasPath) yield break; // 이동 중이면 실행 안 함
        select.SetActive(true);
        isTalking = true;
        while (isTalking)
        {
            //Debug.Log("바라보기 코루틴이 정상 작동 중");
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            yield return null;
        }
    }

}
