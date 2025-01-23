using UnityEngine;

public class TestPlayerControl : MonoBehaviour
{
    public Transform mainCam;
    public Transform target; // ī�޶� ����ٴ� �÷��̾�

    public float distance = 3f; // �÷��̾�� ī�޶� �� �Ÿ�
    public float height = 1.5f; // ī�޶� ����
    public float sensitivity = 3f; // ���콺 ����
    public float cameraSpeed = 10f; // ī�޶� �ε巯�� ������ �ӵ�
    public float moveSpeed = 4.0f;
    public float rotationSpeed = 10f;

    private CharacterController characterController;
    private Vector3 moveDirection;
    private float yaw; // �¿� ȸ�� (Y�� ����)
    private float pitch; // ���� ȸ�� (X�� ����)

    private void Awake()
    {
        target = transform;
        mainCam = GetComponentInChildren<Camera>().transform.parent;
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    { 
        Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ���� ȭ�� �߾ӿ� ����
        Cursor.visible = false;
    }
    private void Update()
    {
        PlayerBasicMove();
        PlayerCamMove();
    }

    private void PlayerBasicMove()
    {
        // �Է� �ޱ� (WASD Ȥ�� ����Ű)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // ī�޶� �������� �̵� ���� ���
        Vector3 forward = mainCam.transform.forward;
        Vector3 right = mainCam.transform.right;

        forward.y = 0f; // ���� ���⸸ ���
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        moveDirection = forward * vertical + right * horizontal;

        // ĳ���� �̵�
        if (moveDirection.magnitude > 0.1f)
        {
            // �̵�
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
    }
    private void PlayerCamMove()
    {
        // ���콺 �Է� �ޱ�
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;

        // ���� ȸ�� ���� ����
        pitch = Mathf.Clamp(pitch, -30f, 60f); // -30������ 60�������� ����

        // ī�޶� ��ġ ���
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 offset = rotation * new Vector3(0f, height, -distance);

        // ī�޶� ��ġ�� ���� ������Ʈ
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, cameraSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * height); // Ÿ���� �ٶ�
    }
}
