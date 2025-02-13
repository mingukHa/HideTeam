using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class NPCFSM : MonoBehaviour
{
    protected enum State { Idle, Look, Walk, Run, Talk, Dead }
    protected State currentState = State.Idle;
    private Transform player; //�÷��̾� ��ġ
    protected Animator animator;
    private Rigidbody[] rigidbodies; //���׵� �޾ƿ��� �κ�
    public bool isDead = false; //���� ����
    private bool isTalking = false; //��ȭ ����
    //private bool isText = false; //������ ä�� ���� 
    private bool isRagdollActivated = false; // ���׵� Ȱ��ȭ ���� Ȯ�ο�
    protected Quaternion initrotation; //�⺻ ��ġ
    private NPCChatTest NPCChatTest; //NPC��ȭ �ҷ����� ��
    public SphereCollider NPCCollider; //NPC ��ȣ�ۿ� �ݶ��̴�
    protected int currentWaypointIndex; //�׺�޽� �迭 �ʱ� ��
    protected NavMeshAgent agent; //�׺�޽�
    [SerializeField]
    protected GameObject select; //ĳ���� ��ǳ��
    private bool isPlayerNearby = false;
    public ReturnManager returnManager;
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        NPCChatTest = GetComponent<NPCChatTest>();
        // ���׵� �ʱ� ��Ȱ��ȭ
        SetRagdollState(false);
        ChangeState(State.Idle);
        initrotation = transform.rotation;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        
        agent.autoBraking = false;

    }

    protected virtual void Update()
    {
        // ���� ������Ʈ
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

    protected virtual void ChangeState(State newState)
    {
        currentState = newState;

        if (newState == State.Dead)
        {
            isRagdollActivated = false; // Dead ���� �ʱ�ȭ
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

    // ���׵� Ȱ��ȭ
    private void ActivateRagdoll()
    {
        animator.enabled = false; // �ִϸ����� ��Ȱ��ȭ
        SetRagdollState(true);    // ���׵� Ȱ��ȭ

        //���⿡ NPC�� ���׵� ���°� �Ǹ� �±׸� NPC -> Ragdoll�� �ٲ�� ��� �־��ּ���(��� ���� Bone�� �ϰ� ���� �ǵ���)
        //From ����
    }

    // ���׵� ���� ����
    private void SetRagdollState(bool state)
    {
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = !state; // ���� Ȱ��ȭ
        }
    }

    // �� ������ �⺻ �ൿ
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
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!isDead && other.CompareTag("Player"))
        {
            Debug.Log("�÷��̾� ���� �� ����");
            isPlayerNearby = true; // �÷��̾ ���� ���� ������ ����
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (!isDead && other.CompareTag("Player"))
        {
            StopCoroutine(TalkView());
            Debug.Log("�÷��̾� ���� ������ ����");
            ChangeState(State.Idle);
            isPlayerNearby = false; // ������ ����� �ʱ�ȭ
            isTalking = false; // ��ȭ ����
            transform.rotation = initrotation; // ���� �������� ����
            select.SetActive(false);
        }
    }
    protected virtual void OnTriggerStay(Collider other)
    {
        if (!isDead && other.CompareTag("Player"))
        {
            // E Ű�� ������ ���� NPC�� �÷��̾ �ٶ󺸸� ��ȭ ����
            if (Input.GetKeyDown(KeyCode.E) && !isTalking)
            {
                Debug.Log("NPC�� �÷��̾ �ٶ󺸸� ��ȭ ����");
                StartCoroutine(TalkView());
                ChangeState(State.Talk); // ��ȭ ���·� ����
            }

            // F Ű�� ������ �� Dead ���� ��ȯ
            if (Input.GetKey(KeyCode.F) && currentState != State.Dead)
            {
                ChangeState(State.Dead);
                NPCChatTest.enabled = false;
                isTalking = false;
            }

            // Dead ����� �������� Ȯ��
            if (currentState == State.Dead && !isRagdollActivated)
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("Dead") && stateInfo.normalizedTime >= 1.0f)
                {
                    ActivateRagdoll();
                    isRagdollActivated = true;
                }
            }
        }
    }
   
    protected IEnumerator TalkView()
    {
        if (isTalking || agent.hasPath) yield break; // �̵� ���̸� ���� �� ��
        select.SetActive(true);
        isTalking = true;
        while (isTalking)
        {
            Debug.Log("�ٶ󺸱� �ڷ�ƾ�� ���� �۵� ��");
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            yield return null;
        }
    }

}
