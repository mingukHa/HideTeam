using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcName;
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public bool IsInHurry = false; // 달리기 여부
    public bool ShouldStartWalking = false; // 걷기 여부

    private FSM fsm;
    private Animator animator;

    // Animator Controllers
    public RuntimeAnimatorController walkAnimator;
    public RuntimeAnimatorController runAnimator;
    public RuntimeAnimatorController lookAroundAnimator;

    private void Start()
    {
        fsm = new FSM(); // FSM 객체 초기화
        animator = GetComponent<Animator>();

        // 초기 상태 설정
        if (lookAroundAnimator != null)
        {
            fsm.ChangeState(new LookAroundState(lookAroundAnimator), this);
        }
        else
        {
            Debug.LogError("LookAroundAnimator가 설정되지 않았습니다.");
        }
    }

    private void Update()
    {
        fsm.Update(this);

        // 테스트용 입력 처리
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
            Debug.LogError("Animator Controller가 null입니다. 상태를 확인하세요.");
            return;
        }

        if (animator.runtimeAnimatorController != newAnimator)
        {
            animator.runtimeAnimatorController = newAnimator;
            Debug.Log($"Animator 변경됨: {newAnimator.name}");
        }
    }

    public void Move(float speed)
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
