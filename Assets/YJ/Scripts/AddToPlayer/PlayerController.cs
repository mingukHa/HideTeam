using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Animator anim = null;
    private Transform tr = null;

    private NPCIdentifier currentNPC;
    private PlayerDisguiser disguiser;
    public GameObject gun = null;

    private float mouseX = 0;
    private float mouseSensitivity = 5f;
    private bool isMoving = false;
    private bool isCrouching = false;

    public Image eImage;
    public Slider eSlider;

    private void Start()
    {
        anim = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        disguiser = GetComponent<PlayerDisguiser>();
    }

    private void Update()
    {
        PlayerAction();
        PlayerMove();

        // �÷��̾ �����̰� ���� ���� ���콺 �Է� ó��
        if (isMoving && InputMouse(ref mouseX))
        {
            InputMouseProcess(mouseX);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // NPCIdentifier�� �ִ� ������Ʈ�� �浹 ��, currentNPC ����
        eImage.gameObject.SetActive(true);
        eSlider.gameObject.SetActive(true);
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

    private bool InputMouse(ref float _mouseX)
    {
        float mX = Input.GetAxis("Mouse X");
        float mY = Input.GetAxis("Mouse Y");

        if (mX == 0f && mY == 0f) return false;

        _mouseX += mX * mouseSensitivity;

        return true;
    }

    private void InputMouseProcess(float _mouseX)
    {
        tr.rotation = Quaternion.Euler(0f, _mouseX, 0f);
    }

    private void PlayerMove()
    {
        float axisV = Input.GetAxis("Vertical");
        float axisH = Input.GetAxis("Horizontal");

        isMoving = axisV != 0 || axisH != 0;

        anim.SetFloat("Forward", axisV);
        anim.SetFloat("Right", axisH);

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

    private void PlayerAction()
    {
        if (Input.GetKey(KeyCode.E))
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

        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(SuicideCoroutine());
        }
    }

    private IEnumerator SuicideCoroutine()
    {
        disguiser.ResetToDefaultCharacter(); // �⺻ �������� ���ư�

        if (gun != null)
        {
            gun.SetActive(true);
        }

        Time.timeScale = 0.6f; // ���ο� ��� ����
        anim.SetTrigger("Suicide");

        // �ִϸ��̼� ��� �� ������ ����(�ڻ�) ���� �߰��� �ڸ�

        // �ִϸ��̼� ���̸� ������
        float animLength = anim.GetCurrentAnimatorStateInfo(0).length;

        // �ִϸ��̼� ���̸�ŭ ��� (�ð� ������ ������ ���� �ʵ��� WaitForSecondsRealtime ���)
        yield return new WaitForSecondsRealtime(animLength);

        Time.timeScale = 1f; // ���� �ӵ��� ����
    }
}
