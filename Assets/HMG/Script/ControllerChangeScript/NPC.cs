using UnityEngine;

public class NomalNPC : MonoBehaviour
{
    public string npcName; // NPC�� �̸� ����
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public bool islook = false; // �ѷ����� ����
    public bool IsInHurry = false; // �޸��� ����
    public bool ShouldStartWalking = false; // �ȱ� ����
    public float crosstime = 0.2f;

    private FSM fsm;
    private Animator animator;

    // ���ϸ����� �߰��Ϸ��� ���⼭ �߰��ϸ� ��
    public RuntimeAnimatorController walkAnimator;
    public RuntimeAnimatorController runAnimator;
    public RuntimeAnimatorController lookAroundAnimator;

    private void Start()
    {
        fsm = new FSM(); // FSM ��ü �ʱ�ȭ
        animator = GetComponent<Animator>(); // ������Ʈ �޾ƿ���

        // �ʱ� ���� ����
        if (lookAroundAnimator != null)
        {
            fsm.ChangeState(new LookAroundState(lookAroundAnimator), this); // �ʱ� ����
            CrossFadeToState("Look", crosstime); // �ʱ� ���� �ִϸ��̼�
        }
        else
        {
            Debug.LogError("�ʱ� ���ϸ����Ͱ� �������� �ʾҽ��ϴ�.");
        }
    }

    private void Update()
    {
        fsm.Update(this); // ���� ������Ʈ �κ�

        // �׽�Ʈ�� �Է� ó��
        if (Input.GetKeyDown(KeyCode.W))
        {
            ShouldStartWalking = true;
            fsm.ChangeState(new WalkState(walkAnimator), this);
            CrossFadeToState("Walking", crosstime);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            IsInHurry = true;
            fsm.ChangeState(new RunState(runAnimator), this);
            CrossFadeToState("Run", crosstime);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            IsInHurry = false;
            fsm.ChangeState(new WalkState(walkAnimator), this);
            CrossFadeToState("Walking", crosstime);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ShouldStartWalking = false;
            fsm.ChangeState(new LookAroundState(lookAroundAnimator), this);
            CrossFadeToState("Look", crosstime);
        }
    }

    // ���� ��ȯ �� CrossFade�� ���� �ִϸ��̼� ��ȯ
    private void CrossFadeToState(string stateName, float transitionDuration)
    {
        if (animator.HasState(0, Animator.StringToHash(stateName)))
        {
            animator.CrossFade(stateName, transitionDuration); // ���� ��ȯ
            Debug.Log($"CrossFade: {stateName} ���·� ��ȯ (�ð�: {transitionDuration}��)");
        }
        else
        {
            Debug.LogError($"Animator ���� '{stateName}'�� ã�� �� �����ϴ�.");
        }
    }

    // ���⼭ �����, �� �Լ� ��� ��
    public void ChangeState(IState newState)
    {
        fsm.ChangeState(newState, this);
    }

    public void AssignAnimator(RuntimeAnimatorController newAnimator)
    {
        if (newAnimator == null)
        {
            Debug.LogError("Animator Controller�� null�Դϴ�. ���¸� Ȯ���ϼ���.");
            return;
        }

        if (animator.runtimeAnimatorController != newAnimator)
        {
            animator.runtimeAnimatorController = newAnimator;
            Debug.Log($"Animator �����: {newAnimator.name}");
        }
    }

    // A��Ÿ ��� �� ����
    public void Move(float speed)
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}