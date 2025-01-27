using UnityEngine;

public class NomalNPC : MonoBehaviour
{
    public string npcName; //NPC�� �̸� ����
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public bool IsInHurry = false; // �޸��� ����
    public bool ShouldStartWalking = false; // �ȱ� ����

    private FSM fsm;
    private Animator animator;

    // ���ϸ����� �߰� �Ϸ��� ���⼭ �߰� �ϸ� ��
    public RuntimeAnimatorController walkAnimator;
    public RuntimeAnimatorController runAnimator;
    public RuntimeAnimatorController lookAroundAnimator;

    private void Start()
    {
        fsm = new FSM(); // FSM ��ü �ʱ�ȭ
        animator = GetComponent<Animator>(); //������Ʈ �޾ƿ���

        // �ʱ� ���� ����
        if (lookAroundAnimator != null)
        {
            fsm.ChangeState(new LookAroundState(lookAroundAnimator), this); //�ʱ� ����
        }
        else
        {
            Debug.LogError("�ʱ� ���ϸ����Ͱ� �������� �ʾҽ��ϴ�.");
        }
    }

    private void Update()
    {
        fsm.Update(this); //���� ������Ʈ �κ�

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
    //���⼭ ���� �� �� �Լ� ��� ��
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
    //A��Ÿ ��� �Ұ���
    public void Move(float speed)
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
