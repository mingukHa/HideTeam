using UnityEngine;

public class ShoulderViewCam : MonoBehaviour
{
    public Transform target; // �÷��̾� ĳ����
    public float distance = 3.0f; // ī�޶�� Ÿ�� �� �Ÿ�
    public float height = 1.5f; // ī�޶� ����
    public float shoulderOffset = 1.0f; // ����� ������
    public float sensitivity = 3.0f; // ���콺 ����
    public float smoothSpeed = 10f; // ī�޶� �ε巯�� �ӵ�
    public LayerMask collisionMask; // ī�޶� �浹 ���� ���̾�

    private float yaw; // �¿� ȸ��
    private float pitch; // ���� ȸ��
    private Vector3 currentPosition;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ���� ���
        Cursor.visible = false;
    }
    private void LateUpdate()
    {
        // ���콺 �Է�
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;

        // ���� ȸ�� ����
        pitch = Mathf.Clamp(pitch, -30f, 60f); // ���� ������ ����

        // ī�޶� ȸ�� ���
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // �⺻ ī�޶� ��ġ ��� (�����)
        Vector3 shoulderOffsetPosition = rotation * new Vector3(shoulderOffset, height, -distance);
        Vector3 desiredPosition = target.position + shoulderOffsetPosition;

        // �浹 ����
        RaycastHit hit;
        if (Physics.Linecast(target.position + Vector3.up * height, desiredPosition, out hit, collisionMask))
        {
            // �浹 ������ ī�޶� ��ġ
            currentPosition = hit.point;
        }
        else
        {
            // �浹�� ������ ���ϴ� ��ġ�� �̵�
            currentPosition = desiredPosition;
        }

        // ī�޶� ��ġ �ε巴�� �̵�
        transform.position = Vector3.Lerp(transform.position, currentPosition, smoothSpeed * Time.deltaTime);

        // Ÿ���� �ٶ󺸵��� ����
        transform.LookAt(target.position + Vector3.up * height);
    }
}
