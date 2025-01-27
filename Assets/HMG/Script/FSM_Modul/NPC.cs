using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcName;
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public bool IsInHurry = false; // �޸��� ����
    public bool ShouldStartWalking = false; // �ȱ� ����

    private FSM fsm;
    private Animator animator;

    // Animator Controllers
    public RuntimeAnimatorController walkAnimator;
    public RuntimeAnimatorController runAnimator;
    public RuntimeAnimatorController lookAroundAnimator;

    private void Start()
    {
        fsm = new FSM(); // FSM ��ü �ʱ�ȭ
        animator = GetComponent<Animator>();

        // �ʱ� ���� ����
        if (lookAroundAnimator != null)
        {
            fsm.ChangeState(new LookAroundState(lookAroundAnimator), this);
        }
        else
        {
            Debug.LogError("LookAroundAnimator�� �������� �ʾҽ��ϴ�.");
        }
    }

    private void Update()
    {
        fsm.Update(this);

        // �׽�Ʈ�� �Է� ó��
        if (Input.GetKeyDown(KeyCode.W))
        {
            ShouldStartWalking = true;
            fsm.ChangeState(new WalkState(walkAnimator), this);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            IsInHurry = true;
            fsm.ChangeState(new RunState(runAnimator), this);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            IsInHurry = false;
            fsm.ChangeState(new WalkState(walkAnimator), this);
        }
    }

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

    public void Move(float speed)
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
