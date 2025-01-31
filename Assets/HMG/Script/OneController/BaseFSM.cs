using UnityEngine;

public class NPCFSM : MonoBehaviour
{
    protected enum State { Idle, Look, Walk, Run, Talk, Dead }
    protected State currentState = State.Idle;

    protected Animator animator;
    private Rigidbody[] rigidbodies;
    private bool isRagdollActivated = false; // ���׵� Ȱ��ȭ ���� Ȯ�ο�

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();

        // ���׵� �ʱ� ��Ȱ��ȭ
        SetRagdollState(false);
        ChangeState(State.Idle);
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

        // F Ű�� Dead ���� ��ȯ
        if (Input.GetKeyDown(KeyCode.F) && currentState != State.Dead)
        {
            ChangeState(State.Dead); // Dead ���·� ��ȯ
        }

        // Dead ����� �������� Ȯ��
        if (currentState == State.Dead && !isRagdollActivated)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Dead") && stateInfo.normalizedTime >= 1.0f) // Dead ����� ������ ��
            {
                ActivateRagdoll(); // ���׵� Ȱ��ȭ
                isRagdollActivated = true; // ���׵��� Ȱ��ȭ�Ǿ����� ǥ��
            }
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
    protected virtual void TalkBehavior() { }
    protected virtual void DeadBehavior() { }
}
