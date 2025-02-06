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
        Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ���� ȭ�� �߾ӿ� ����
        Cursor.visible = false;
    }
    private void Update()
    {
        PlayerBasicMove();
        PlayerAction();
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

        anim.SetFloat("Forward", vertical);
        anim.SetFloat("Right", horizontal);

        moveDirection = forward * vertical + right * horizontal;

        // ĳ���� �̵�
        if (moveDirection.magnitude > 0.1f)
        {
            RotateViewForward();
            // �̵�
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }

        // �ȱ�(Walk)
        if (Input.GetKey(KeyCode.LeftControl))
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }

        // �ɱ�(Crouch)
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching; // ���� ��ȯ
            anim.SetBool("Crouch", isCrouching); // �ִϸ������� Crouch �Ķ���� ����
        }

        // �ٱ�(Sprint)
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

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * 5f); // ȸ�� �ӵ� ����
    }

    private void OnTriggerEnter(Collider other)
    {
        // NPCIdentifier�� �ִ� ������Ʈ�� �浹 ��, currentNPC ����
        NPCIdentifier npc = other.GetComponent<NPCIdentifier>();
        if (npc != null)
        {
            currentNPC = npc;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // NPCIdentifier�� �ִ� ������Ʈ���� ����� currentNPC ����
        if (other.GetComponent<NPCIdentifier>() == currentNPC)
        {
            currentNPC = null;
        }
    }

    private void PlayerAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            disguiser.ChangeAppearance(currentNPC); // NPC ������ ����� ���� ����
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            disguiser.ResetToDefaultCharacter(); // �⺻ �������� ���ư�
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            // NPC ����ȭ ���� �߰�
            anim.SetTrigger("Neutralize");
        }
    }

}
