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

    public Image eImage;    //EŰ �̹���
    public Slider eSlider;  //EŰ ������

    public Image rImage;    //RŰ �̹���
    public Slider rSlider;  //RŰ ������

    private float eholdTime = 0f;   //EŰ ���� �ð�
    private float eGoalholdTime = 1f;   //EŰ �������ϴ� �ð�

    private float rholdTime = 0f;   //EŰ ���� �ð�
    private float rGoalholdTime = 1f;   //EŰ �������ϴ� �ð�

    private bool isDisguised = false;

    private ReturnManager returnManager;

    private void Start()
    {
        anim = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        disguiser = GetComponent<PlayerDisguiser>();
        returnManager = GetComponent<ReturnManager>();
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
        // ���� other ������Ʈ�� �±װ� "NPC"�� �ƴϸ� ��ȯ (���� X)
        if (!other.gameObject.CompareTag("NPC")) return;

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
        if (!other.gameObject.CompareTag("NPC")) return;

        eImage.gameObject.SetActive(false);
        eSlider.gameObject.SetActive(false);

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
            eholdTime += Time.deltaTime; //���� �ð� ����
            eSlider.value = eholdTime / eGoalholdTime;  //�ð���ŭ �����̴� ������

            if(eholdTime >= eGoalholdTime)
            {
                disguiser.ChangeAppearance(currentNPC); // NPC ������ ����� ���� ����
                isDisguised = true;
                eholdTime = 0f; //�ٽ� �ʱ�ȭ
            }
        }
        else
        {
            eholdTime = 0f; //Ű���� �� ���� 0�ʷ� �ʱ�ȭ
            eSlider.value = 0f; //�����̴� ������ �ʱ�ȭ
        }

        if (isDisguised && Input.GetKey(KeyCode.R))
        {
            rImage.gameObject.SetActive(true);
            rSlider.gameObject.SetActive(true);
            rholdTime += Time.deltaTime; //���� �ð� ����
            rSlider.value = rholdTime / rGoalholdTime;  //�ð���ŭ �����̴� ������

            if (rholdTime >= rGoalholdTime)
            {
                disguiser.ResetToDefaultCharacter(); // �⺻ �������� ���ư�
                rholdTime = 0f; //�ٽ� �ʱ�ȭ
                isDisguised = false;
            }
        }
        else
        {
            rImage.gameObject.SetActive(false);
            rSlider.gameObject.SetActive(false);
            rholdTime = 0f; //Ű���� �� ���� 0�ʷ� �ʱ�ȭ
            rSlider.value = 0f; //�����̴� ������ �ʱ�ȭ
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
