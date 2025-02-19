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
    //public RagdollGrabber ragdollGrabber;

    [Header ("권총 오브젝트")]
    public GameObject gun = null;
    public ParticleSystem muzzleFlash;
    public GameObject muzzleFlashLight;

    [Header ("담배 오브젝트")]
    public GameObject cigarette = null;
    public GameObject droppingCigarette = null;

    private float mouseX = 0;
    private float mouseSensitivity = 5f;

    private bool isEChatActive = false;
    private bool isECarActive = false;
    private bool isMoving = false;
    private bool isCrouching = false;
    private bool isSuiciding = false;
    private static bool isStarted = false;

    [Header("상호작용 키 버튼")]
    public Image eImage;    //E키 이미지
    public Slider eSlider;  //E키 게이지

    public Image rImage;    //R키 이미지
    public Slider rSlider;  //R키 게이지

    public Image fImage;    //F키 이미지

    public GameObject E_Chat;   //대화키

    public GameObject CarKey;   //차 발로 차기 키

    private float eholdTime = 0f;   //E키 누른 시간
    private float eGoalholdTime = 1f;   //E키 눌러야하는 시간

    private float rholdTime = 0f;   //R키 누른 시간
    private float rGoalholdTime = 1f;   //R키 눌러야하는 시간

    private bool isDisguised = false;   //분장했는지 체크
    private bool isGrabed = false;      //잡은걸 체크

    public List<DoorController> doorCons = new List<DoorController>();
    [SerializeField]
    private bool isFirstOpen = false;   // 출입문 첫 개방 확인용

    private NPCIdentifier disguisedNPC = null;  //변장한 NPC를 추적

    private void Start()
    {
        anim = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        disguiser = GetComponent<PlayerDisguiser>();

        if (!isStarted)
        {
            cigarette.SetActive(true);
            Smoking();
        }

        isFirstOpen = false;
    }

    private void Update()
    {
        // 흡연이 끝나고 플레이어가 움직이고 있을 때만 마우스 입력 처리
        if (isMoving && isStarted && InputMouse(ref mouseX))
        {
            InputMouseProcess(mouseX);
        }

        if(isEChatActive && Input.GetKeyDown(KeyCode.E) || isEChatActive && Input.GetKeyDown(KeyCode.E))
        {
            E_Chat.SetActive(false);
            isEChatActive = false;
            isECarActive = false;
        }

        PlayerAction();
        PlayerMove();
        CheckFirstDoorOpen();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC") || other.CompareTag("NPCTeller"))
        {
            E_Chat.SetActive(true);
            isEChatActive = true; // 상태를 저장
        }

        if (other.CompareTag("Car"))
        {
            carAlarm = other.GetComponent<CarAlarm>();
            if (carAlarm != null)
            {
                CarKey.SetActive(true);
                isECarActive = true; // 상태를 저장
            }
        }

        if (other.CompareTag("TrashBin"))
        {
            trashBin = other.GetComponent<TrashBin>();
        }
    }

    //실시간으로 UI가 변해야하므로 Stay로 변경
    private void OnTriggerStay(Collider other)
    {
        // "NPC", "Ragdoll" 태그를 가진 오브젝트와만 처리
        if (!other.gameObject.CompareTag("NPC") && !other.gameObject.CompareTag("Ragdoll")) return;

        NPCIdentifier npc = other.GetComponent<NPCIdentifier>();    //NPCIdentifier스크립트랑 작용
        NPCFSM npcFSM = other.GetComponent<NPCFSM>(); // NPCFSM 가져오기
        NPCRichMan rich = other.GetComponent<NPCRichMan>(); //RichMan 전용 컴퍼넌트 받아오기

        Debug.Log("현재 변장중인 직업이 맞는지 : " + (npc == disguisedNPC));

        if (npc != null)
        {
            // 변장 상태에서, 변장한 NPC와 동일한 NPC에 대해 상호작용 차단
            if (isDisguised && npc == disguisedNPC)
            {
                eImage.gameObject.SetActive(false);
                eSlider.gameObject.SetActive(false); // E키 UI 비활성화
                return; // 변장한 NPC와는 상호작용할 수 없으므로 여기서 종료
            }
            else
            {
                currentNPC = npc; // 변장하지 않았다면, 현재 NPC 설정
            }
        }

        if (rich != null)
        {
            // 변장 상태에서, 변장한 NPC와 동일한 NPC에 대해 상호작용 차단
            if (isDisguised && rich == disguisedNPC)
            {
                Debug.Log("UI가 안숨겨짐.");
                eImage.gameObject.SetActive(false);
                eSlider.gameObject.SetActive(false); // E키 UI 비활성화
                return; // 변장한 NPC와는 상호작용할 수 없으므로 여기서 종료
            }
            else
            {
                currentNPC = npc; // 변장하지 않았다면, 현재 NPC 설정
            }
        }

        if (npcFSM != null)
        {
            if (npcFSM.isDead)
            {
                // 죽은 NPC와 상호작용 시 E키 활성화
                E_Chat.gameObject.SetActive(false);
                eImage.gameObject.SetActive(true);
                eSlider.gameObject.SetActive(true);
                fImage.gameObject.SetActive(false); // 죽은 NPC일 때 F키 관련 UI 비활성화
            }
            else
            {
                // 살아있는 NPC와 상호작용 시 E키 비활성화
                eImage.gameObject.SetActive(false);
                eSlider.gameObject.SetActive(false);
                //fImage.gameObject.SetActive(true); // 살아있는 NPC일 때 F키 관련 UI 활성화
            }
        }

        if(rich != null)
        {
            if(rich.isDead)
            {
                // 죽은 NPC와 상호작용 시 E키 활성화
                E_Chat.gameObject.SetActive(false);
                eImage.gameObject.SetActive(true);
                eSlider.gameObject.SetActive(true);
                fImage.gameObject.SetActive(false); // 죽은 NPC일 때 F키 관련 UI 비활성화
            }
            else
            {
                // 살아있는 NPC와 상호작용 시 E키 비활성화
                eImage.gameObject.SetActive(false);
                eSlider.gameObject.SetActive(false);
                //fImage.gameObject.SetActive(true); // 살아있는 NPC일 때 F키 관련 UI 활성화
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            fImage.gameObject.SetActive(false);
            E_Chat.SetActive(false);
        }

        if (other.CompareTag("NPCTeller"))
        {
            fImage.gameObject.SetActive(false);
            E_Chat.SetActive(false);
        }

        if (other.CompareTag("Car"))
        {
            CarKey.SetActive(false);
            carAlarm = null;  // carAlarm을 null로 설정하여, 차량과 상호작용 불가하도록 만듦
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
            Debug.Log("죽은 NPC에서 멀어짐");
            eImage.gameObject.SetActive(false);
            eSlider.gameObject.SetActive(false);
            //}

            // NPCIdentifier가 있는 오브젝트에서 벗어나면 currentNPC 해제
            //if (other.GetComponent<NPCIdentifier>() == currentNPC)
            //{
            currentNPC = null;
            //}

            // NPC가 떠나면 fImage 비활성화
            fImage.gameObject.SetActive(false);
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

        // 걷기(Walk)
        if (Input.GetKey(KeyCode.LeftControl))
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }

        // 앉기(Crouch)
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching; // 상태 전환
            anim.SetBool("Crouch", isCrouching); // 애니메이터의 Crouch 파라미터 설정
        }
        
        // 뛰기(Sprint)
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
            // currentNPC에서 NPCFSM 컴포넌트를 가져옴
            NPCFSM npcFSM = currentNPC.GetComponent<NPCFSM>();
            NPCRichMan rich = currentNPC.GetComponent<NPCRichMan>();
            Debug.Log($"{npcFSM},{rich} 받아오고 있음");
            if (npcFSM != null && npcFSM.isDead)
            {
                // NPC가 죽었을 때 실행할 로직
                Debug.Log("죽은 NPC와 상호작용");
                Debug.Log($"{npcFSM},{rich} 받아오고 있음2번");
                eholdTime += Time.deltaTime; // 누른 시간 증가
                eSlider.value = eholdTime / eGoalholdTime;  // 시간만큼 슬라이더 게이지
                Debug.Log($"{eSlider.value}");
                if (eholdTime >= eGoalholdTime)
                {
                    disguiser.ChangeAppearance(currentNPC); // NPC 정보를 사용해 변장 실행
                    isDisguised = true;
                    disguisedNPC = currentNPC; // 변장한 NPC 추적
                    eholdTime = 0f; // 다시 초기화
                }
            }

            if (rich != null && rich.isDead)
            {
                // NPC가 죽었을 때 실행할 로직
                Debug.Log("죽은 NPC와 상호작용");

                eholdTime += Time.deltaTime; // 누른 시간 증가
                eSlider.value = eholdTime / eGoalholdTime;  // 시간만큼 슬라이더 게이지

                if (eholdTime >= eGoalholdTime)
                {
                    disguiser.ChangeAppearance(currentNPC); // NPC 정보를 사용해 변장 실행
                    isDisguised = true;
                    disguisedNPC = currentNPC; // 변장한 NPC 추적
                    eholdTime = 0f; // 다시 초기화
                }
            }
        }
        else
        {
            eholdTime = 0f; // 키에서 손 떼면 0초로 초기화
            eSlider.value = 0f; // 슬라이더 게이지 초기화
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
            rholdTime += Time.deltaTime; //누른 시간 증가
            rSlider.value = rholdTime / rGoalholdTime;  //시간만큼 슬라이더 게이지

            if (rholdTime >= rGoalholdTime)
            {
                disguiser.ResetToDefaultCharacter(); // 기본 복장으로 돌아감
                rholdTime = 0f; //다시 초기화
                isDisguised = false;
            }
        }
        else
        {
            rImage.gameObject.SetActive(false);
            rSlider.gameObject.SetActive(false);
            rholdTime = 0f; //키에서 손 떼면 0초로 초기화
            rSlider.value = 0f; //슬라이더 게이지 초기화
        }

        //제압에 관한 코드
        if (Input.GetKeyDown(KeyCode.F) && currentNPC != null)
        {
            //NPC가 살아있을 때만 작동
            NPCFSM npcFSM = currentNPC.GetComponent<NPCFSM>();
            NPCRichMan rich = currentNPC.GetComponent<NPCRichMan>();

            if (npcFSM != null)
            {
                // 살아있는 NPC일 때만 무력화 로직 실행
                if (!npcFSM.isDead)
                {
                    // NPC 무력화 로직 추가
                    anim.SetTrigger("Neutralize");
                }

                // NPC가 죽었든 살아있든 F키를 누르면 바로 UI를 끄기
                fImage.gameObject.SetActive(false);
                E_Chat.gameObject.SetActive(false);
            }

            //자산가용
            if (rich != null)
            {
                // 살아있는 NPC일 때만 무력화 로직 실행
                if (!rich.isDead)
                {
                    // NPC 무력화 로직 추가
                    anim.SetTrigger("Neutralize");
                    EventManager.Trigger(GameEventType.RichToiletKill);
                }

                // NPC가 죽었든 살아있든 F키를 누르면 바로 UI를 끄기
                fImage.gameObject.SetActive(false);
                E_Chat.gameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.K) && !isSuiciding) // 중복 입력 방지
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
    }

    private IEnumerator KickTheCar()
    {
        yield return new WaitForSeconds(1f); // 1초 대기
        EventManager.Trigger(GameEventType.Carkick);
        Debug.Log("차킥 이벤트 발생");
        carAlarm.ActivateAlarm(); // 도난방지 알람 실행
    }

    private IEnumerator SuicideCoroutine()
    {
        isSuiciding = true; // 코루틴 실행 중 중복 입력 방지

        anim.SetTrigger("Suicide");

        // Upper Layer에서 'Suicide' 애니메이션 상태가 될 때까지 대기
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(1).IsName("Suicide"));

        // Forward, Right 값을 고정
        anim.SetFloat("Forward", 0f);
        anim.SetFloat("Right", 0f);

        disguiser.ResetToDefaultCharacter(); // 기본 복장으로 돌아감

        if (gun != null)
        {
            gun.SetActive(true);
        }

        Time.timeScale = 0.2f; // 슬로우 모션 적용

        // Suicide 애니메이션이 실행된 후 0.35초 대기
        yield return new WaitForSeconds(0.35f * Time.timeScale);

        // 1. Particle System 재생
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // 2. Light 켜기
        if (muzzleFlashLight != null)
        {
            muzzleFlashLight.SetActive(true);
        }

        // 0.1초 후 Light 끄기
        yield return new WaitForSeconds(0.1f * Time.timeScale);

        if (muzzleFlashLight != null)
        {
            muzzleFlashLight.SetActive(false);
        }

        // Upper Layer에서 Suicide 애니메이션의 길이를 가져옴
        float animLength = anim.GetCurrentAnimatorStateInfo(1).length;

        // 애니메이션이 끝날 때까지 Forward와 Right 값을 계속 0으로 유지
        float elapsedTime = 0f;
        while (elapsedTime < animLength)
        {
            anim.SetFloat("Forward", 0f);
            anim.SetFloat("Right", 0f);
            elapsedTime += Time.unscaledDeltaTime; // Time.timeScale 영향을 받지 않도록
            yield return null;
        }

        Time.timeScale = 1f; // 원래 속도로 복원
        gun.SetActive(false);

        isSuiciding = false; // 코루틴 종료 후 다시 K키 입력 가능
    }
}
