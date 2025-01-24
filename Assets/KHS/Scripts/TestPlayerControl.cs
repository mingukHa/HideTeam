using UnityEngine;

public class TestPlayerControl : MonoBehaviour
{
    public Transform mainCam;
    public Transform npcTr;

    public float moveSpeed = 4.0f;
    public float rotationSpeed = 10f;

    private CharacterController characterController;
    private Vector3 moveDirection;

    private void Awake()
    {
        mainCam = GetComponentInChildren<Camera>().transform.parent;
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    { 
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서를 화면 중앙에 고정
        Cursor.visible = false;
    }
    private void Update()
    {
        PlayerBasicMove();
        NPCInteraction();
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

        moveDirection = forward * vertical + right * horizontal;

        // 캐릭터 이동
        if (moveDirection.magnitude > 0.1f)
        {
            // 이동
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
    }
    private void NPCInteraction()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(npcTr != null)
            {
                npcTr.GetComponent<NPCStats>().StunAnimPlay();
            }
            else
            {
                Debug.Log("대상없음");
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (npcTr != null)
            {
                npcTr.GetComponent<NPCStats>().EscapeStun();
            }
            else
            {
                Debug.Log("대상없음");
            }
        }
    }
}
