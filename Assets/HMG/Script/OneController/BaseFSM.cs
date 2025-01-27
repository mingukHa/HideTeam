using UnityEngine;

public class NPCFSM : MonoBehaviour
{
    protected enum State { Idle, Look, Walk, Run ,Talk}
    protected State currentState = State.Idle;

    protected Animator animator;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        ChangeState(State.Idle);
    }

    protected virtual void Update()
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
        }
    }

    protected virtual void ChangeState(State newState)
    {
        currentState = newState;
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Look");
        animator.ResetTrigger("Walk");
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Talk");

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

    // 각 상태의 기본 행동
    protected virtual void IdleBehavior() { }
    protected virtual void LookBehavior() { }
    protected virtual void WalkBehavior() { }
    protected virtual void RunBehavior() { }
    protected virtual void TalkBehavior() { }
}
