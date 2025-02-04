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
        Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ���� ȭ�� �߾ӿ� ����
        Cursor.visible = false;
    }
    private void Update()
    {
        PlayerBasicMove();
        NPCInteraction();
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
                Debug.Log("������");
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
                Debug.Log("������");
            }
        }
    }
}
