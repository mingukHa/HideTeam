using UnityEngine;

public class TestPlayerControl : MonoBehaviour
{
    public Transform mainCam;
    public Transform target; // 카메라가 따라다닐 플레이어

    public float distance = 3f; // 플레이어와 카메라 간 거리
    public float height = 1.5f; // 카메라 높이
    public float sensitivity = 3f; // 마우스 감도
    public float cameraSpeed = 10f; // 카메라 부드러운 움직임 속도
    public float moveSpeed = 4.0f;
    public float rotationSpeed = 10f;

    private CharacterController characterController;
    private Vector3 moveDirection;
    private float yaw; // 좌우 회전 (Y축 기준)
    private float pitch; // 상하 회전 (X축 기준)

    private void Awake()
    {
        target = transform;
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
        PlayerCamMove();
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
    private void PlayerCamMove()
    {
        // 마우스 입력 받기
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;

        // 상하 회전 각도 제한
        pitch = Mathf.Clamp(pitch, -30f, 60f); // -30도에서 60도까지만 제한

        // 카메라 위치 계산
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 offset = rotation * new Vector3(0f, height, -distance);

        // 카메라 위치와 방향 업데이트
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, cameraSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * height); // 타겟을 바라봄
    }
}
