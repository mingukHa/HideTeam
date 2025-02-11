using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class NPCFSM : MonoBehaviour
{
    protected enum State { Idle, Look, Walk, Run, Talk, Dead }
    protected State currentState = State.Idle;
    private Transform player;
    protected Animator animator;
    private Rigidbody[] rigidbodies;
    public bool isDead = false; //죽음 상태
    private bool isTalking = false; //대화 상태
    //private bool isText = false; //죽으면 채팅 끄기 
    private bool isRagdollActivated = false; // 레그돌 활성화 여부 확인용
    private Quaternion initrotation;
    private NPCChatTest NPCChatTest;
    public SphereCollider BoxCollider;
    private NavMeshAgent agent;
    public PatrolRoute patrolRoute;
    private int currentWaypointIndex;
    protected virtual void Start()
    {
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
        agent.SetDestination(patrolRoute.waypoints[currentWaypointIndex]);
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
        }        //// F 키로 Dead 상태 전환
        //if (Input.GetKeyDown(KeyCode.F) && currentState != State.Dead)
        //{
        //    ChangeState(State.Dead); // Dead 상태로 전환
        //}

        //// Dead 모션이 끝났는지 확인
        //if (currentState == State.Dead && !isRagdollActivated)
        //{
        //    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //    if (stateInfo.IsName("Dead") && stateInfo.normalizedTime >= 1.0f) // Dead 모션이 끝났을 때
        //    {
        //        ActivateRagdoll(); // 레그돌 활성화
        //        isRagdollActivated = true; // 레그돌이 활성화되었음을 표시
        //    }
        //}


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

    // 레그돌 활성화
    private void ActivateRagdoll()
    {
        animator.enabled = false; // 애니메이터 비활성화
        SetRagdollState(true);    // 레그돌 활성화
    }

    // 레그돌 상태 설정
    private void SetRagdollState(bool state)
    {
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = !state; // 물리 활성화
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
        isDead = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isDead == false)
        {
            if (other.CompareTag("Player"))
               {
               Debug.Log("바라보기 코루틴 시작");
               StartCoroutine(TalkView());
                }
        }
    }
    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // F 키로 Dead 상태 전환
            if (Input.GetKey(KeyCode.F) && currentState != State.Dead)
            {
                ChangeState(State.Dead); // Dead 상태로 전환
                NPCChatTest.enabled = false;
                isTalking = false;
                
            }
            // Dead 모션이 끝났는지 확인
            if (currentState == State.Dead && !isRagdollActivated)
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("Dead") && stateInfo.normalizedTime >= 1.0f) // Dead 모션이 끝났을 때
                {
                    ActivateRagdoll(); // 레그돌 활성화
                    isRagdollActivated = true; // 레그돌이 활성화되었음을 표시
                    
                    //StopCoroutine(TalkView());
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            StopCoroutine(TalkView());
            transform.rotation = initrotation;
            isTalking = false;
            Debug.Log("바라보기 코루틴 종료");
        }

    }
    private IEnumerator TalkView()
    {
        if (isTalking) yield break; 

        isTalking = true; 

        while (isTalking == true)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            yield return null; 
        }
        
    }

}
