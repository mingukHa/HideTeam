using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class NPCFSM : MonoBehaviour
{
    public bool isPlayerNearby = false; //대화 여부 확인
    public bool isRagdollActivated = false; // 레그돌 활성화 여부 확인용
    public bool isDead = false; //죽음 상태
    public SphereCollider NPCCollider; //NPC 상호작용 콜라이더
    public GameObject select; //캐릭터 말풍선    
    
    protected enum State { Idle, Look, Walk, Run, Talk, Dead } //상태 열거형
    protected State currentState = State.Idle; //기본값은 Idle
    protected Transform player; //플레이어 위치
    protected Animator animator; //애니메이터
    protected Quaternion initrotation; //기본 위치
    protected int currentWaypointIndex; //네브메쉬 배열 초기 값
    protected NavMeshAgent agent; //네브메쉬
    protected NPCChatTest chat; //채팅 박스
    protected Moutline moutline; // 아웃라인
    protected bool isTalking = false; //대화 상태
    protected NPCChatTest NPCChatTest; //NPC대화 불러오는 곳
    protected Rigidbody[] rigidbodies; //레그돌 받아오는 부분
    
    
    protected virtual void Start()
    {
        chat = GetComponent<NPCChatTest>();
        animator = GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        NPCChatTest = GetComponent<NPCChatTest>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        moutline = GetComponent<Moutline>();

        player = GameObject.FindGameObjectWithTag("Player").transform; //태그로 플레이어 받아오기
        SetRagdollState(false); //레그돌 off
        ChangeState(State.Idle); //기본상태 Idle
        initrotation = transform.rotation; //각 NPC의 회전 초기 값      
    }
    
    protected virtual void Update()
    {
        HandleState();
        DeadState();
        TalkState();
    }
    //현재 상태 정의 
    protected void HandleState()
    {
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
    }
    protected void DeadState() //죽음 상태 
    {
        if (isDead && !isRagdollActivated)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // 애니메이션 "Dead"가 실행 중인지 확인
            if (stateInfo.IsName("Dead") && stateInfo.normalizedTime >= 1.0f)
            {
                //Debug.Log(" Dead 애니메이션 종료 - 레그돌 활성화 실행");
                ActivateRagdoll(); //  레그돌 처리
                agent.enabled = true;
            }
        }
    }
    protected void TalkState() //대화 상태
    {
        if (!isDead && isPlayerNearby)
        {
            if (Input.GetKeyDown(KeyCode.E) && !isTalking)
            {
                StartCoroutine(TalkView());
                ChangeState(State.Talk); // 대화 상태로 변경
            }

            if (Input.GetKey(KeyCode.F) && currentState != State.Dead)
            {
                ChangeState(State.Dead); //죽음 상태로 변경
                NPCChatTest.enabled = false; //대화 스크립트 off
                moutline.enabled = false; //아웃라인 off
                agent.enabled = false; //네브메쉬 off
                isTalking = false; //대화 상태 off
                select.SetActive(false); //UI종료
                chat.LoadNPCDialogue("NULL", 0); //대화를 초기화
                isDead = true;
            }
        }
    }
    protected virtual void ChangeState(State newState)
    {
        currentState = newState; //새로운 상태로 변경

        if (newState == State.Dead) //죽었을 때 상태를 dead로 초기화
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
        //기존에 남아있는 상태를 초기화 하고
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Look");
        animator.ResetTrigger("Walk");
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Talk");
        animator.ResetTrigger("Dead");
        //새로운 상태로 갱신
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

        // 최상위 오브젝트 제외하고 모든 자식 오브젝트의 태그와 레이어 변경
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            if (child == transform) continue; // 루트 오브젝트는 제외

            child.gameObject.tag = "Ragdoll";
            child.gameObject.layer = 15;
        }
    }

    private void SetRagdollState(bool state)
    {
        foreach (var rb in rigidbodies) //레그돌 정보를 다 받아오고
        {
            rb.isKinematic = !state; // 물리 활성화
        }
    }


    // 각 상태의 기본 행동
    protected virtual void IdleBehavior() { }
    protected virtual void LookBehavior() { }
    protected virtual void WalkBehavior() { }
    protected virtual void RunBehavior() { }
    protected virtual void TalkBehavior(){ }
    protected virtual void DeadBehavior()
    {
        //Debug.Log("데드 비헤이어 실행");
        chat.LoadNPCDialogue("NULL", 0);
        isDead = true;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!isDead && other.CompareTag("Player"))
        {
            isPlayerNearby = true; // 플레이어가 범위 내에 있음을 저장
        }        
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (!isDead && other.CompareTag("Player"))
        {
            StopCoroutine(TalkView()); //다시 원래 보고 있던 위치로 회전
            ChangeState(State.Idle); //Idle상태로 변경
            isPlayerNearby = false; // 대화상태 off
            isTalking = false; // 대화 종료
            transform.rotation = initrotation; // 원래 방향으로 복귀
            select.SetActive(false); //대화 UI off
            chat.LoadNPCDialogue("NULL", 0); //대사 초기화
        }
    } 
    protected IEnumerator TalkView()
    {
        Debug.Log("바라보기 코루틴 실행");
        if (isTalking || agent.hasPath) yield break; // 이동 중이면 실행 안 함
        select.SetActive(true); //대화 UI 띄우기
        isTalking = true; //대화상태 on
        while (isTalking)
        {
            //Debug.Log("바라보기 코루틴이 정상 작동 중");
            Vector3 direction = (player.position - transform.position).normalized; //나를 바라보도록 방향 벡터 정규화
            direction.y = 0; //y축 고정
            Quaternion lookRotation = Quaternion.LookRotation(direction); //회전값 계산
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); //부드럽게 보간

            yield return null;
        }
    }
    protected IEnumerator CheckArrival() //NPC가 도착 했는지 판단하는 함수
    {
            // NPC가 목적지에 도착할 때까지 대기
            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance || agent.velocity.magnitude > 0.1f)
            {//  경로 계산을 하고 있는지,남은 거리가 stoppingDistance보다 큰지, 속도가 완전히 멈추지 않았는지 확인
                yield return null;
            }
            // 도착 후 멈추는 코드
            agent.isStopped = true; // 네비게이션 멈춤
            agent.ResetPath(); // 경로 초기화
            chat.LoadNPCDialogue("Null", 0);
            Debug.Log("NPC가 목적지에 도착하여 멈췄습니다.");
    }
    protected void StopNpc()
    {
        StopCoroutine(TalkView());
        chat.LoadNPCDialogue("NULL", 0);
        transform.rotation = initrotation;
        NPCCollider.radius = 0.01f;
        animator.SetTrigger("Idle");
        select.SetActive(false);
    }
}


