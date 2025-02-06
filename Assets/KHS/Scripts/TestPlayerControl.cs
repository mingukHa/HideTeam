using UnityEngine;

public class TestPlayerControl : MonoBehaviour
{
    public Transform mainCam;

    public float moveSpeed = 4.0f;
    public float rotationSpeed = 10f;

    private Animator anim = null;
    private CharacterController characterController;
    private Vector3 moveDirection;
    private NPCIdentifier currentNPC;
    private PlayerDisguiser disguiser;

    private bool isCrouching = false;

    private void Awake()
    {
        mainCam = GetComponentInChildren<Camera>().transform.parent;
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        disguiser = GetComponent<PlayerDisguiser>();
    }

    private void Start()
    { 
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서를 화면 중앙에 고정
        Cursor.visible = false;
    }
    private void Update()
    {
        PlayerBasicMove();
        PlayerAction();
    }

    private void PlayerBasicMove()
    {
        // 입력 받기 (WASD 혹은 방향키)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");


        // 카메라를 기준으로 이동 방향 계산
        Vector3 forward = mainCam.transform.forward;
        Vector3 right = mainCam.transform.right;

        forward.y = 0f; // 수평 방향만 계산
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        anim.SetFloat("Forward", vertical);
        anim.SetFloat("Right", horizontal);

        moveDirection = forward * vertical + right * horizontal;

        // 캐릭터 이동
        if (moveDirection.magnitude > 0.1f)
        {
            RotateViewForward();
            // 이동
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }

        // 걷기(Walk)
        if (Input.GetKey(KeyCode.LeftControl))
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }

        // 앉기(Crouch)
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching; // 상태 전환
            anim.SetBool("Crouch", isCrouching); // 애니메이터의 Crouch 파라미터 설정
        }

        // 뛰기(Sprint)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("Sprint", true);
        }
        else
        {
            anim.SetBool("Sprint", false);
        }
    }

    private void RotateViewForward()
    {
        Vector3 camForward = mainCam.transform.forward;
        camForward.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(camForward);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * 5f); // 회전 속도 조정
    }

    private void OnTriggerEnter(Collider other)
    {
        // NPCIdentifier가 있는 오브젝트와 충돌 시, currentNPC 설정
        NPCIdentifier npc = other.GetComponent<NPCIdentifier>();
        if (npc != null)
        {
            currentNPC = npc;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // NPCIdentifier가 있는 오브젝트에서 벗어나면 currentNPC 해제
        if (other.GetComponent<NPCIdentifier>() == currentNPC)
        {
            currentNPC = null;
        }
    }

    private void PlayerAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            disguiser.ChangeAppearance(currentNPC); // NPC 정보를 사용해 변장 실행
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            disguiser.ResetToDefaultCharacter(); // 기본 복장으로 돌아감
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            // NPC 무력화 로직 추가
            anim.SetTrigger("Neutralize");
        }
    }

}
