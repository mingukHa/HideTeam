using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EventManager;

public class PlayerController : MonoBehaviour
{
    private Animator anim = null;
    private Transform tr = null;

    private NPCIdentifier currentNPC;
    private PlayerDisguiser disguiser;
    private CarAlarm carAlarm;
    private TrashBin trashBin;

    public GameObject gun = null;
    public GameObject cigarette = null;

    private float mouseX = 0;
    private float mouseSensitivity = 5f;

    private bool isMoving = false;
    private bool isCrouching = false;
    private bool isStarted = false;

    public Image eImage;    //EŰ �̹���
    public Slider eSlider;  //EŰ ������

    public Image rImage;    //RŰ �̹���
    public Slider rSlider;  //RŰ ������

    public Image fImage;    //KŰ �̹���

    public GameObject E_Chat;

    public GameObject CarKey;

    private float eholdTime = 0f;   //EŰ ���� �ð�
    private float eGoalholdTime = 1f;   //EŰ �������ϴ� �ð�

    private float rholdTime = 0f;   //RŰ ���� �ð�
    private float rGoalholdTime = 1f;   //RŰ �������ϴ� �ð�

    private bool isDisguised = false;
    private bool isGrabed = false;

    public List<DoorController> doorCons = new List<DoorController>();
    [SerializeField]
    private bool isFirstOpen = false;   // ���Թ� ù ���� Ȯ�ο�

    private NPCIdentifier disguisedNPC = null;  //������ NPC�� ����

    private void Start()
    {
        anim = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        disguiser = GetComponent<PlayerDisguiser>();

        Smoking();
        isFirstOpen = false;
    }

    private void Update()
    {
        PlayerAction();
        PlayerMove();

        // ���� ������ �÷��̾ �����̰� ���� ���� ���콺 �Է� ó��
        if (isMoving && InputMouse(ref mouseX))
        {
            InputMouseProcess(mouseX);
        }

        CheckFirstDoorOpen();
    }

    private void OnTriggerEnter(Collider other)
    {
        //    // ���� other ������Ʈ�� �±װ� "NPC"�� �ƴϸ� ��ȯ (���� X)
        //    if (!other.gameObject.CompareTag("NPC")) return;

        //    NPCIdentifier npc = other.GetComponent<NPCIdentifier>();    
        //    NPCFSM npcFSM = other.GetComponent<NPCFSM>(); // NPCFSM ��������

        //    if (npcFSM != null && npcFSM.isDead)
        //    {
        //        Debug.Log("���� NPC�� ��ȣ�ۿ�");
        //        // NPCIdentifier�� �ִ� ������Ʈ�� �浹 ��, currentNPC ����
        //        eImage.gameObject.SetActive(true);
        //        eSlider.gameObject.SetActive(true);
        //        fImage.gameObject.SetActive(false);

        //        if (npc != null)
        //        {
        //            currentNPC = npc;
        //        }
        //    }
        //    else
        //    {
        //        // NPC�� ������� �� fImage Ȱ��ȭ
        //        fImage.gameObject.SetActive(true);
        //        currentNPC = npc; // NPC�� currentNPC�� ����
        //    }
        if (other.CompareTag("NPC"))
        {
            E_Chat.SetActive(true);
        }
        if (other.CompareTag("Car"))
        {
            carAlarm = other.GetComponent<CarAlarm>();
            if (carAlarm != null)
            {
                CarKey.SetActive(true);
            }
        }

        if (other.CompareTag("TrashBin"))
        {
            trashBin = other.GetComponent<TrashBin>();
        }
    }

    //�ǽð����� UI�� ���ؾ��ϹǷ� Stay�� ����
    private void OnTriggerStay(Collider other)
    {
        // "NPC" �±׸� ���� ������Ʈ�͸� ó��
        if (!other.gameObject.CompareTag("NPC")) return;

        NPCIdentifier npc = other.GetComponent<NPCIdentifier>();
        NPCFSM npcFSM = other.GetComponent<NPCFSM>(); // NPCFSM ��������

        if (npc != null)
        {
            // ���� ���¿���, ������ NPC�� ������ NPC�� ���� ��ȣ�ۿ� ����
            if (isDisguised && npc == disguisedNPC)
            {
                eImage.gameObject.SetActive(false);
                eSlider.gameObject.SetActive(false); // EŰ UI ��Ȱ��ȭ
                return; // ������ NPC�ʹ� ��ȣ�ۿ��� �� �����Ƿ� ���⼭ ����
            }
            else
            {
                currentNPC = npc; // �������� �ʾҴٸ�, ���� NPC ����
            }
        }

        if (npcFSM != null)
        {
            if (npcFSM.isDead)
            {
                // ���� NPC�� ��ȣ�ۿ� �� EŰ Ȱ��ȭ
                eImage.gameObject.SetActive(true);
                eSlider.gameObject.SetActive(true);
                fImage.gameObject.SetActive(false); // ���� NPC�� �� FŰ ���� UI ��Ȱ��ȭ
            }
            else
            {
                // ����ִ� NPC�� ��ȣ�ۿ� �� EŰ ��Ȱ��ȭ
                eImage.gameObject.SetActive(false);
                eSlider.gameObject.SetActive(false);
                fImage.gameObject.SetActive(true); // ����ִ� NPC�� �� FŰ ���� UI Ȱ��ȭ
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            E_Chat.SetActive(false);
        }
        if (other.CompareTag("Car"))
        {
            CarKey.SetActive(false);
            carAlarm = null;  // carAlarm�� null�� �����Ͽ�, ������ ��ȣ�ۿ� �Ұ��ϵ��� ����
        }

        if (other.CompareTag("TrashBin"))
        {
            trashBin = null;
        }

        if (!other.gameObject.CompareTag("NPC")) return;

        //NPCFSM npcFSM = other.GetComponent<NPCFSM>();

        //if (npcFSM != null && npcFSM.isDead)
        //{
            Debug.Log("���� NPC���� �־���");
            eImage.gameObject.SetActive(false);
            eSlider.gameObject.SetActive(false);
        //}

        // NPCIdentifier�� �ִ� ������Ʈ���� ����� currentNPC ����
        //if (other.GetComponent<NPCIdentifier>() == currentNPC)
        //{
            currentNPC = null;
        //}

        // NPC�� ������ fImage ��Ȱ��ȭ
        fImage.gameObject.SetActive(false);
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
        if (Input.GetKey(KeyCode.E) && !isDisguised && currentNPC != null)
        {
            // currentNPC���� NPCFSM ������Ʈ�� ������
            NPCFSM npcFSM = currentNPC.GetComponent<NPCFSM>();

            if (npcFSM != null && npcFSM.isDead)
            {
                // NPC�� �׾��� �� ������ ����
                Debug.Log("���� NPC�� ��ȣ�ۿ�");

                eholdTime += Time.deltaTime; // ���� �ð� ����
                eSlider.value = eholdTime / eGoalholdTime;  // �ð���ŭ �����̴� ������

                if (eholdTime >= eGoalholdTime)
                {
                    disguiser.ChangeAppearance(currentNPC); // NPC ������ ����� ���� ����
                    isDisguised = true;
                    disguisedNPC = currentNPC; // ������ NPC ����
                    eholdTime = 0f; // �ٽ� �ʱ�ȭ
                }
            }
        }
        else
        {
            eholdTime = 0f; // Ű���� �� ���� 0�ʷ� �ʱ�ȭ
            eSlider.value = 0f; // �����̴� ������ �ʱ�ȭ
        }

        if (Input.GetKeyDown(KeyCode.E) && carAlarm != null)
        {
            anim.SetTrigger("isCar");
            EventManager.Trigger(GameEventType.Carkick);
            Debug.Log("��ű �̺�Ʈ �߻�");
            carAlarm.ActivateAlarm(); // �������� �˶� ����
        }

        if (Input.GetKeyDown(KeyCode.E) && trashBin != null)
        {
            anim.SetTrigger("isMessUp");
            trashBin.MessUpTrashBin();
        }

        if (Input.GetKey(KeyCode.R) && isDisguised)
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

        if (Input.GetKeyDown(KeyCode.F) && currentNPC != null)
        {
            //NPC�� ������� ���� �۵�
            NPCFSM npcFSM = currentNPC.GetComponent<NPCFSM>();

            if (npcFSM != null && !npcFSM.isDead)
            {
                // NPC ����ȭ ���� �߰�
                anim.SetTrigger("Neutralize");
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(SuicideCoroutine());
        }
    }
    private void CheckFirstDoorOpen()
    {
        foreach(DoorController doorCon in doorCons)
        {
            if(!isFirstOpen && doorCon.isOpen)
            {
                isFirstOpen = true;
                EventManager.Trigger(GameEventType.PlayerEnterBank);
            }
        }
    }

    private void Smoking()
    {
        if (!isStarted)
        {
            anim.SetTrigger("isStarted");
            isStarted = true;
        }

        StartCoroutine(ThrowCigarette());
    }

private IEnumerator ThrowCigarette()
    {
        yield return new WaitForSecondsRealtime(12f);

        if (cigarette != null)
        {
            cigarette.SetActive(false);
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
        gun.SetActive(false);
    }
}
