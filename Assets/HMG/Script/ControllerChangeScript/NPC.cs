using UnityEngine;

public class NomalNPC : MonoBehaviour
{
    public string npcName; // NPC의 이름 지정
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public bool islook = false; // 둘러보기 여부
    public bool IsInHurry = false; // 달리기 여부
    public bool ShouldStartWalking = false; // 걷기 여부
    public float crosstime = 0.2f;

    private FSM fsm;
    private Animator animator;

    // 에니메이터 추가하려면 여기서 추가하면 됨
    public RuntimeAnimatorController walkAnimator;
    public RuntimeAnimatorController runAnimator;
    public RuntimeAnimatorController lookAroundAnimator;

    private void Start()
    {
        fsm = new FSM(); // FSM 객체 초기화
        animator = GetComponent<Animator>(); // 컴포넌트 받아오기

        // 초기 상태 설정
        if (lookAroundAnimator != null)
        {
            fsm.ChangeState(new LookAroundState(lookAroundAnimator), this); // 초기 설정
            CrossFadeToState("Look", crosstime); // 초기 상태 애니메이션
        }
        else
        {
            Debug.LogError("초기 에니메이터가 설정되지 않았습니다.");
        }
    }

    private void Update()
    {
        fsm.Update(this); // 상태 업데이트 부분

        // 테스트용 입력 처리
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

    // 상태 전환 시 CrossFade를 통해 애니메이션 전환
    private void CrossFadeToState(string stateName, float transitionDuration)
    {
        if (animator.HasState(0, Animator.StringToHash(stateName)))
        {
            animator.CrossFade(stateName, transitionDuration); // 상태 전환
            Debug.Log($"CrossFade: {stateName} 상태로 전환 (시간: {transitionDuration}초)");
        }
        else
        {
            Debug.LogError($"Animator 상태 '{stateName}'를 찾을 수 없습니다.");
        }
    }

    // 여기서 변경됨, 이 함수 써야 함
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

    // A스타 사용 할 거임
    public void Move(float speed)
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}