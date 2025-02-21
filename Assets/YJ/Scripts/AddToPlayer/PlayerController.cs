using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static EventManager;


public class PlayerController : MonoBehaviour
{
    public Transform mainCam;

    private Animator anim = null;
    private Transform tr = null;

    private RagdollGrabber grabber; //��� ���۳�Ʈ
    private NPCIdentifier currentNPC;
    private PlayerDisguiser disguiser;
    private CarAlarm carAlarm;
    private TrashBin trashBin;
    //public RagdollGrabber ragdollGrabber;

    [Header ("���� ������Ʈ")]
    public GameObject gun = null;
    public ParticleSystem muzzleFlash;
    public GameObject muzzleFlashLight;
    public GameObject bloodEffect;

    [Header ("��� ������Ʈ")]
    public GameObject cigarette = null;
    public GameObject droppingCigarette = null;

    private float mouseX = 0;
    private float mouseSensitivity = 5f;

    public float moveSpeed = 4.0f;
    public float rotationSpeed = 10f;
    private Vector3 moveDirection;

    private bool isEChatActive = false;
    private bool isECarActive = false;
    private bool isMoving = false;
    private bool isCrouching = false;
    private bool isSuiciding = false;
    private static bool isStarted = false;

    public void SetStarted()
    {
        isStarted = false;
    }

    [Header("��ȣ�ۿ� Ű ��ư")]
    public Image eImage;    //EŰ �̹���
    public Slider eSlider;  //EŰ ������

    public Image rImage;    //RŰ �̹���
    public Slider rSlider;  //RŰ ������

    public Image fImage;    //FŰ �̹���

    public Image gImage;    //GŰ �̹���

    public GameObject E_Chat;   //��ȭŰ

    public GameObject CarKey;   //�� �߷� ���� Ű

    private float eholdTime = 0f;   //EŰ ���� �ð�
    private float eGoalholdTime = 1f;   //EŰ �������ϴ� �ð�

    private float rholdTime = 0f;   //RŰ ���� �ð�
    private float rGoalholdTime = 1f;   //RŰ �������ϴ� �ð�

    private bool isDisguised = false;   //�����ߴ��� üũ

    public List<DoorController> doorCons = new List<DoorController>();
    [SerializeField]
    private bool isFirstOpen = false;   // ���Թ� ù ���� Ȯ�ο�

    private NPCIdentifier disguisedNPC = null;  //������ NPC�� ����

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        grabber = GetComponent<RagdollGrabber>();
        anim = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        disguiser = GetComponent<PlayerDisguiser>();

        if (!isStarted)
        {
            cigarette.SetActive(true);
            Smoking();
        }
        else
        {
            isMoving = true;
        }
            isFirstOpen = false;
    }

    private void Update()
    {
        // ���� ������ �÷��̾ �����̰� ���� ���� ���콺 �Է� ó��
        if (isMoving)
        {
            PlayerAction();
            //PlayerMove();
            PlayerBasicMove();      // ī�޶� �ٲ�鼭 �ٲ� �÷��̾� ����
            CheckFirstDoorOpen();
        }

        if(isEChatActive && Input.GetKeyDown(KeyCode.E) || isEChatActive && Input.GetKeyDown(KeyCode.E))
        {
            E_Chat.SetActive(false);
            isEChatActive = false;
            isECarActive = false;
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC") || other.CompareTag("NPCTeller"))
        {
            E_Chat.SetActive(true);
            isEChatActive = true; // ���¸� ����
        }

        if (other.CompareTag("Car"))
        {
            carAlarm = other.GetComponent<CarAlarm>();
            if (carAlarm != null)
            {
                CarKey.SetActive(true);
                isECarActive = true; // ���¸� ����
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
        // "NPC", "Ragdoll" �±׸� ���� ������Ʈ�͸� ó��
        if (!other.gameObject.CompareTag("NPC") && !other.gameObject.CompareTag("Ragdoll")) return;

        NPCIdentifier npc = other.GetComponent<NPCIdentifier>();    //NPCIdentifier��ũ��Ʈ�� �ۿ�
        NPCFSM npcFSM = other.GetComponent<NPCFSM>(); // NPCFSM ��������
        NPCRichMan rich = other.GetComponent<NPCRichMan>(); //RichMan ���� ���۳�Ʈ �޾ƿ���

        Debug.Log("���� �������� ������ �´��� : " + (npc == disguisedNPC));

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

        if (rich != null)
        {
            // ���� ���¿���, ������ NPC�� ������ NPC�� ���� ��ȣ�ۿ� ����
            if (isDisguised && rich == disguisedNPC)
            {
                Debug.Log("UI�� �ȼ�����.");
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
                E_Chat.gameObject.SetActive(false);
                eImage.gameObject.SetActive(true);
                eSlider.gameObject.SetActive(true);
                fImage.gameObject.SetActive(false); // ���� NPC�� �� FŰ UI ��Ȱ��ȭ
                gImage.gameObject.SetActive(true); // ���� NPC�� �� GŰ UI Ȱ��ȭ
            }
            else
            {
                // ����ִ� NPC�� ��ȣ�ۿ� �� EŰ ��Ȱ��ȭ
                eImage.gameObject.SetActive(false);
                eSlider.gameObject.SetActive(false);
                fImage.gameObject.SetActive(true); // ����ִ� NPC�� �� FŰ ���� UI Ȱ��ȭ
                gImage.gameObject.SetActive(false); // ����ִ� NPC�� �� GŰ UI ��Ȱ��ȭ
            }
        }

        if(rich != null)
        {
            if(rich.isDead)
            {
                // ���� NPC�� ��ȣ�ۿ� �� EŰ Ȱ��ȭ
                E_Chat.gameObject.SetActive(false);
                eImage.gameObject.SetActive(true);
                eSlider.gameObject.SetActive(true);
                fImage.gameObject.SetActive(false); // ���� NPC�� �� FŰ ���� UI ��Ȱ��ȭ
                gImage.gameObject.SetActive(true); // ���� NPC�� �� GŰ UI Ȱ��ȭ
            }
            else
            {
                // ����ִ� NPC�� ��ȣ�ۿ� �� EŰ ��Ȱ��ȭ
                E_Chat.gameObject.SetActive(false);
                eImage.gameObject.SetActive(false);
                eSlider.gameObject.SetActive(false);
                fImage.gameObject.SetActive(true); // ����ִ� NPC�� �� FŰ ���� UI Ȱ��ȭ
                gImage.gameObject.SetActive(false); // ����ִ� NPC�� �� GŰ UI ��Ȱ��ȭ
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            eSlider.gameObject.SetActive(false);
            fImage.gameObject.SetActive(false);
            gImage.gameObject.SetActive(false);
            E_Chat.SetActive(false);
        }

        if (other.CompareTag("NPCTeller"))
        {
            eSlider.gameObject.SetActive(false);
            fImage.gameObject.SetActive(false);
            gImage.gameObject.SetActive(false);
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

        if (other.CompareTag("Ragdoll"))
        {
            eImage.gameObject.SetActive(false);
            eSlider.gameObject.SetActive(false);
        }

        if (!other.gameObject.CompareTag("Ragdoll")) return;
        {
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
    }

    //private bool InputMouse(ref float _mouseX)
    //{
    //    float mX = Input.GetAxis("Mouse X");
    //    float mY = Input.GetAxis("Mouse Y");

    //    if (mX == 0f && mY == 0f) return false;

    //    _mouseX += mX * mouseSensitivity;

    //    return true;
    //}

    //private void InputMouseProcess(float _mouseX)
    //{
    //    tr.rotation = Quaternion.Euler(0f, _mouseX, 0f);
    //}

    //private void PlayerMove()
    //{
    //    float axisV = Input.GetAxis("Vertical");
    //    float axisH = Input.GetAxis("Horizontal");

    //    isMoving = axisV != 0 || axisH != 0;

    //    anim.SetFloat("Forward", axisV);
    //    anim.SetFloat("Right", axisH);

    //    // �ȱ�(Walk)
    //    if (Input.GetKey(KeyCode.LeftControl))
    //    {
    //        anim.SetBool("Walk", true);
    //    }
    //    else
    //    {
    //        anim.SetBool("Walk", false);
    //    }

    //    // �ɱ�(Crouch)
    //    if (Input.GetKeyDown(KeyCode.C))
    //    {
    //        isCrouching = !isCrouching; // ���� ��ȯ
    //        anim.SetBool("Crouch", isCrouching); // �ִϸ������� Crouch �Ķ���� ����
    //    }
        
    //    // �ٱ�(Sprint)
    //    if (Input.GetKey(KeyCode.LeftShift))
    //    {
    //        anim.SetBool("Sprint", true);
    //    }
    //    else
    //    {
    //        anim.SetBool("Sprint", false);
    //    }

    //}
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

    private void PlayerAction()
    {
        if (Input.GetKey(KeyCode.E) && !isDisguised && currentNPC != null)
        {
            // currentNPC���� NPCFSM ������Ʈ�� ������
            NPCFSM npcFSM = currentNPC.GetComponent<NPCFSM>();
            NPCRichMan rich = currentNPC.GetComponent<NPCRichMan>();
            Debug.Log($"{npcFSM},{rich} �޾ƿ��� ����");
            if (npcFSM != null && npcFSM.isDead)
            {
                // NPC�� �׾��� �� ������ ����
                Debug.Log("���� NPC�� ��ȣ�ۿ�");
                Debug.Log($"{npcFSM},{rich} �޾ƿ��� ����2��");
                eholdTime += Time.deltaTime; // ���� �ð� ����
                eSlider.value = eholdTime / eGoalholdTime;  // �ð���ŭ �����̴� ������
                Debug.Log($"{eSlider.value}");
                if (eholdTime >= eGoalholdTime)
                {
                    disguiser.ChangeAppearance(currentNPC); // NPC ������ ����� ���� ����
                    isDisguised = true;
                    disguisedNPC = currentNPC; // ������ NPC ����
                    eholdTime = 0f; // �ٽ� �ʱ�ȭ
                }
            }

            if (rich != null && rich.isDead)
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
            StartCoroutine(KickTheCar());
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

        //���п� ���� �ڵ�
        if (Input.GetKeyDown(KeyCode.F) && currentNPC != null)
        {
            NPCRichMan rich = currentNPC.GetComponent<NPCRichMan>();

            anim.SetTrigger("Neutralize");
                
            // NPC�� �׾��� ����ֵ� FŰ�� ������ �ٷ� UI�� ����
            fImage.gameObject.SetActive(false);
            E_Chat.gameObject.SetActive(false);

            //�ڻ갡��
            if (rich != null)
            {
                // ����ִ� NPC�� ���� ����ȭ ���� ����
                if (!rich.isDead)
                {
                    // NPC ����ȭ ���� �߰�
                    anim.SetTrigger("Neutralize");
                    EventManager.Trigger(GameEventType.RichToiletKill);
                }

                // NPC�� �׾��� ����ֵ� FŰ�� ������ �ٷ� UI�� ����
                fImage.gameObject.SetActive(false);
                E_Chat.gameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.K) && !isSuiciding) // �ߺ� �Է� ����
        {
            StartCoroutine(SuicideCoroutine());
        }
    }
    private void CheckFirstDoorOpen()
    {
        foreach(DoorController doorCon in doorCons)
        {
            if(!isFirstOpen && doorCon.isPrOpen)
            {
                isFirstOpen = true;
                EventManager.Trigger(GameEventType.PlayerEnterBank);
            }
        }
    }

    private void Smoking()
    {
        anim.SetTrigger("isStarted");
        StartCoroutine(ThrowCigarette());
    }

    private IEnumerator ThrowCigarette()
    {
        yield return new WaitForSeconds(9.67f);

        if (cigarette != null)
        {
            cigarette.SetActive(false);
            droppingCigarette.SetActive(true);
        }

        yield return new WaitForSeconds(5f);
        isStarted = true;
        isMoving = true;
    }

    private IEnumerator KickTheCar()
    {
        yield return new WaitForSeconds(1f); // 1�� ���
        EventManager.Trigger(GameEventType.Carkick);
        Debug.Log("��ű �̺�Ʈ �߻�");
        carAlarm.ActivateAlarm(); // �������� �˶� ����
    }

    private IEnumerator SuicideCoroutine()
    {
        isSuiciding = true; // �ڷ�ƾ ���� �� �ߺ� �Է� ����

        anim.SetTrigger("Suicide");

        // Upper Layer���� 'Suicide' �ִϸ��̼� ���°� �� ������ ���
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(1).IsName("Suicide"));

        // Forward, Right ���� ����
        anim.SetFloat("Forward", 0f);
        anim.SetFloat("Right", 0f);

        disguiser.ResetToDefaultCharacter(); // �⺻ �������� ���ư�

        if (gun != null)
        {
            gun.SetActive(true);
        }

        Time.timeScale = 0.2f; // ���ο� ��� ����

        // Suicide �ִϸ��̼��� ����� �� 0.35�� ���
        yield return new WaitForSeconds(0.35f * Time.timeScale);

        // 1. �����÷��� ���
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // 2. �ѱ� ȭ�� �ѱ�
        if (muzzleFlashLight != null)
        {
            muzzleFlashLight.SetActive(true);
        }

        // 3. ���� ȿ�� �ѱ�
        if (bloodEffect != null)
        {
            bloodEffect.SetActive(true);
        }

        // 0.1�� �� �ѱ� ȭ�� ����
        yield return new WaitForSeconds(0.1f * Time.timeScale);

        if (muzzleFlashLight != null)
        {
            muzzleFlashLight.SetActive(false);
        }


        // Upper Layer���� Suicide �ִϸ��̼��� ���̸� ������
        float animLength = anim.GetCurrentAnimatorStateInfo(1).length;

        // �ִϸ��̼��� ���� ������ Forward�� Right ���� ��� 0���� ����
        float elapsedTime = 0f;
        while (elapsedTime < animLength)
        {
            anim.SetFloat("Forward", 0f);
            anim.SetFloat("Right", 0f);
            elapsedTime += Time.unscaledDeltaTime; // Time.timeScale ������ ���� �ʵ���
            yield return null;
        }

        Time.timeScale = 1f; // ���� �ӵ��� ����
        gun.SetActive(false);

        isSuiciding = false; // �ڷ�ƾ ���� �� �ٽ� KŰ �Է� ����
        EventManager.Trigger(GameEventType.GameOver);
    }
}
