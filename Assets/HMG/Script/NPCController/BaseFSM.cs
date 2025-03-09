using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class NPCFSM : MonoBehaviour
{
    public bool isPlayerNearby = false; //��ȭ ���� Ȯ��
    public bool isRagdollActivated = false; // ���׵� Ȱ��ȭ ���� Ȯ�ο�
    public bool isDead = false; //���� ����
    public SphereCollider NPCCollider; //NPC ��ȣ�ۿ� �ݶ��̴�
    public GameObject select; //ĳ���� ��ǳ��    
    
    protected enum State { Idle, Look, Walk, Run, Talk, Dead } //���� ������
    protected State currentState = State.Idle; //�⺻���� Idle
    protected Transform player; //�÷��̾� ��ġ
    protected Animator animator; //�ִϸ�����
    protected Quaternion initrotation; //�⺻ ��ġ
    protected int currentWaypointIndex; //�׺�޽� �迭 �ʱ� ��
    protected NavMeshAgent agent; //�׺�޽�
    protected NPCChatTest chat; //ä�� �ڽ�
    protected Moutline moutline; // �ƿ�����
    protected bool isTalking = false; //��ȭ ����
    protected NPCChatTest NPCChatTest; //NPC��ȭ �ҷ����� ��
    protected Rigidbody[] rigidbodies; //���׵� �޾ƿ��� �κ�
    
    
    protected virtual void Start()
    {
        chat = GetComponent<NPCChatTest>();
        animator = GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        NPCChatTest = GetComponent<NPCChatTest>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        moutline = GetComponent<Moutline>();

        player = GameObject.FindGameObjectWithTag("Player").transform; //�±׷� �÷��̾� �޾ƿ���
        SetRagdollState(false); //���׵� off
        ChangeState(State.Idle); //�⺻���� Idle
        initrotation = transform.rotation; //�� NPC�� ȸ�� �ʱ� ��      
    }
    
    protected virtual void Update()
    {
        HandleState();
        DeadState();
        TalkState();
    }
    //���� ���� ���� 
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
    protected void DeadState() //���� ���� 
    {
        if (isDead && !isRagdollActivated)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // �ִϸ��̼� "Dead"�� ���� ������ Ȯ��
            if (stateInfo.IsName("Dead") && stateInfo.normalizedTime >= 1.0f)
            {
                //Debug.Log(" Dead �ִϸ��̼� ���� - ���׵� Ȱ��ȭ ����");
                ActivateRagdoll(); //  ���׵� ó��
                agent.enabled = true;
            }
        }
    }
    protected void TalkState() //��ȭ ����
    {
        if (!isDead && isPlayerNearby)
        {
            if (Input.GetKeyDown(KeyCode.E) && !isTalking)
            {
                StartCoroutine(TalkView());
                ChangeState(State.Talk); // ��ȭ ���·� ����
            }

            if (Input.GetKey(KeyCode.F) && currentState != State.Dead)
            {
                ChangeState(State.Dead); //���� ���·� ����
                NPCChatTest.enabled = false; //��ȭ ��ũ��Ʈ off
                moutline.enabled = false; //�ƿ����� off
                agent.enabled = false; //�׺�޽� off
                isTalking = false; //��ȭ ���� off
                select.SetActive(false); //UI����
                chat.LoadNPCDialogue("NULL", 0); //��ȭ�� �ʱ�ȭ
                isDead = true;
            }
        }
    }
    protected virtual void ChangeState(State newState)
    {
        currentState = newState; //���ο� ���·� ����

        if (newState == State.Dead) //�׾��� �� ���¸� dead�� �ʱ�ȭ
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
        //������ �����ִ� ���¸� �ʱ�ȭ �ϰ�
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Look");
        animator.ResetTrigger("Walk");
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Talk");
        animator.ResetTrigger("Dead");
        //���ο� ���·� ����
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
        if (isRagdollActivated) return; //  �ߺ� ���� ����
        isDead = true;
        
        animator.enabled = false; //  �ִϸ��̼� ����
        SetRagdollState(true); //  ���� ����

        isRagdollActivated = true; //  �̹� ����Ǿ����� ����

        // �ֻ��� ������Ʈ �����ϰ� ��� �ڽ� ������Ʈ�� �±׿� ���̾� ����
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            if (child == transform) continue; // ��Ʈ ������Ʈ�� ����

            child.gameObject.tag = "Ragdoll";
            child.gameObject.layer = 15;
        }
    }

    private void SetRagdollState(bool state)
    {
        foreach (var rb in rigidbodies) //���׵� ������ �� �޾ƿ���
        {
            rb.isKinematic = !state; // ���� Ȱ��ȭ
        }
    }


    // �� ������ �⺻ �ൿ
    protected virtual void IdleBehavior() { }
    protected virtual void LookBehavior() { }
    protected virtual void WalkBehavior() { }
    protected virtual void RunBehavior() { }
    protected virtual void TalkBehavior(){ }
    protected virtual void DeadBehavior()
    {
        //Debug.Log("���� �����̾� ����");
        chat.LoadNPCDialogue("NULL", 0);
        isDead = true;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!isDead && other.CompareTag("Player"))
        {
            isPlayerNearby = true; // �÷��̾ ���� ���� ������ ����
        }        
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (!isDead && other.CompareTag("Player"))
        {
            StopCoroutine(TalkView()); //�ٽ� ���� ���� �ִ� ��ġ�� ȸ��
            ChangeState(State.Idle); //Idle���·� ����
            isPlayerNearby = false; // ��ȭ���� off
            isTalking = false; // ��ȭ ����
            transform.rotation = initrotation; // ���� �������� ����
            select.SetActive(false); //��ȭ UI off
            chat.LoadNPCDialogue("NULL", 0); //��� �ʱ�ȭ
        }
    } 
    protected IEnumerator TalkView()
    {
        Debug.Log("�ٶ󺸱� �ڷ�ƾ ����");
        if (isTalking || agent.hasPath) yield break; // �̵� ���̸� ���� �� ��
        select.SetActive(true); //��ȭ UI ����
        isTalking = true; //��ȭ���� on
        while (isTalking)
        {
            //Debug.Log("�ٶ󺸱� �ڷ�ƾ�� ���� �۵� ��");
            Vector3 direction = (player.position - transform.position).normalized; //���� �ٶ󺸵��� ���� ���� ����ȭ
            direction.y = 0; //y�� ����
            Quaternion lookRotation = Quaternion.LookRotation(direction); //ȸ���� ���
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); //�ε巴�� ����

            yield return null;
        }
    }
    protected IEnumerator CheckArrival() //NPC�� ���� �ߴ��� �Ǵ��ϴ� �Լ�
    {
            // NPC�� �������� ������ ������ ���
            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance || agent.velocity.magnitude > 0.1f)
            {//  ��� ����� �ϰ� �ִ���,���� �Ÿ��� stoppingDistance���� ū��, �ӵ��� ������ ������ �ʾҴ��� Ȯ��
                yield return null;
            }
            // ���� �� ���ߴ� �ڵ�
            agent.isStopped = true; // �׺���̼� ����
            agent.ResetPath(); // ��� �ʱ�ȭ
            chat.LoadNPCDialogue("Null", 0);
            Debug.Log("NPC�� �������� �����Ͽ� ������ϴ�.");
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


