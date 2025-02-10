using UnityEngine;

public class ShoulderViewCam : MonoBehaviour
{
    public Transform target; // 플레이어 캐릭터
    public float distance = 3.0f; // 카메라와 타겟 간 거리
    public float height = 1.5f; // 카메라 높이
    public float shoulderOffset = 1.0f; // 숄더뷰 오프셋
    public float sensitivity = 3.0f; // 마우스 감도
    public float smoothSpeed = 10f; // 카메라 부드러움 속도
    public LayerMask collisionMask; // 카메라 충돌 감지 레이어

    private float yaw; // 좌우 회전
    private float pitch; // 상하 회전
    private Vector3 currentPosition;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서를 잠금
        Cursor.visible = false;
    }
    private void LateUpdate()
    {
        // 마우스 입력
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;

        // 상하 회전 제한
        pitch = Mathf.Clamp(pitch, -30f, 60f); // 상하 각도를 제한

        // 카메라 회전 계산
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // 기본 카메라 위치 계산 (숄더뷰)
        Vector3 shoulderOffsetPosition = rotation * new Vector3(shoulderOffset, height, -distance);
        Vector3 desiredPosition = target.position + shoulderOffsetPosition;

        // 충돌 감지
        RaycastHit hit;
        if (Physics.Linecast(target.position + Vector3.up * height, desiredPosition, out hit, collisionMask))
        {
            // 충돌 지점에 카메라 위치
            currentPosition = hit.point;
        }
        else
        {
            // 충돌이 없으면 원하는 위치로 이동
            currentPosition = desiredPosition;
        }

        // 카메라 위치 부드럽게 이동
        transform.position = Vector3.Lerp(transform.position, currentPosition, smoothSpeed * Time.deltaTime);

        // 타겟을 바라보도록 설정
        transform.LookAt(target.position + Vector3.up * height);
    }
}
