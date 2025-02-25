using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static EventManager;


public class PlayerController : MonoBehaviour
{
    public Transform mainCam;   //메인 카메라

    private Animator anim = null;   //애니메이션
    private Transform tr = null;    //위치

    private RagdollGrabber grabber; //잡기 컴퍼넌트
    private NPCIdentifier currentNPC;   //NPC 위장 기능용
    private PlayerDisguiser disguiser;  //위장 기능
    private CarAlarm carAlarm;  //차알림
    private TrashBin trashBin;  //쓰레기통
    private PlayerSound playerSound;    //플레이어 사운드

    [Header ("권총 오브젝트")]
    public GameObject gun = null;
    public ParticleSystem muzzleFlash;
    public GameObject muzzleFlashLight;
    public GameObject bloodEffect;

    [Header ("담배 오브젝트")]
    public GameObject cigarette = null;
    public GameObject droppingCigarette = null;

    //private float mouseX = 0; //이제 안쓰는 마우스 방식
    //private float mouseSensitivity = 5f;  //이제 안쓰는 마우스 방식

    //마우스 관련 변수
    public float moveSpeed = 4.0f; 
    public float rotationSpeed = 10f;   
    private Vector3 moveDirection;
    public GameObject cutSceneCam;

    private bool isEChatActive = false;     //대화하는 중인지 체크
    private bool isECarActive = false;      //자동차 대화박스 눌렀는지 체크
    private bool isMoving = false;          //움직이는 중인지 체크
    private bool isCrouching = false;       //앉은 상태
    private bool isSuiciding = false;       //자살 중
    private static bool isStarted = false;  //

    public void SetStarted()
    {
        isStarted = false;
    }

    [Header("상호작용 키 버튼")]
    public Image eImage;    //위장 E키 이미지
    public Slider eSlider;  //위장 E키 게이지

    public Image rImage;    //환복 R키 이미지
    public Slider rSlider;  //환복 R키 게이지

    public Image fImage;    //제압 F키 이미지

    public Image gImage;    //잡기 G키 이미지

    public GameObject E_Chat;   //대화 E키

    public GameObject CarKey;   //차 발로 차기 E키

    private float eholdTime = 0f;   //위장 E키 누른 시간
    private float eGoalholdTime = 1f;   //위장 E키 눌러야하는 시간

    private float rholdTime = 0f;   //환복 R키 누른 시간
    private float rGoalholdTime = 1f;   //환복 R키 눌러야하는 시간

    private bool isDisguised = false;   //분장했는지 체크

    public List<DoorController> doorCons = new List<DoorController>();
    [SerializeField]
    private bool isFirstOpen = false;   // 출입문 첫 개방 확인용

    private NPCIdentifier disguisedNPC = null;  //변장한 NPC를 추적

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        grabber = GetComponent<RagdollGrabber>();
        anim = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        disguiser = GetComponent<PlayerDisguiser>();
        playerSound = GetComponent<PlayerSound>();

        if (!isStarted)
        {
            cutSceneCam.SetActive(true);
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
        // 흡연이 끝나고 플레이어가 움직이고 있을 때만 마우스 입력 처리
        if (isMoving)
        {
            PlayerAction();
            //PlayerMove();
            PlayerBasicMove();      // 카메라 바뀌면서 바뀐 플레이어 무브
            CheckFirstDoorOpen();
        }

        //대화상태 활성화 시
        if (isEChatActive && Input.GetKeyDown(KeyCode.E) || isECarActive && Input.GetKeyDown(KeyCode.E))
        {
            E_Chat.SetActive(false);
            isEChatActive = false;
            isECarActive = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //NPC, NPCTeller Tag와 TriggerEnter 시
        if (other.CompareTag("NPC") || other.CompareTag("NPCTeller"))
        {
            E_Chat.SetActive(true); //대화UI 켜기
            isEChatActive = true; // 상태를 저장
        }

        //Car Tag와 TriggerEnter 시
        if (other.CompareTag("Car"))
        {
            carAlarm = other.GetComponent<CarAlarm>();  
            if (carAlarm != null)   
            {
                CarKey.SetActive(true);     //차 발로차기 UI 켜기
                isECarActive = true;        //상태를 저장
            }
        }

        //TrashBin Tag와 TriggerEnter 시
        if (other.CompareTag("TrashBin"))
        {
            trashBin = other.GetComponent<TrashBin>();
        }
    }

    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.TellerTalk, LockMoving);
        EventManager.Subscribe(GameEventType.ConvEnd, UnlockMoving);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEventType.TellerTalk, LockMoving);
        EventManager.Unsubscribe(GameEventType.ConvEnd, UnlockMoving);
    }

    public void LockMoving()
    {
        anim.Rebind();
        isMoving = false;
    }
    public void UnlockMoving()
    {
        isMoving = true;
    }
    //실시간으로 UI가 변해야하므로 Stay 사용
    private void OnTriggerStay(Collider other)
    {
        // "NPC", "Ragdoll" 태그를 가진 오브젝트와만 처리
        if (!other.gameObject.CompareTag("NPC") && !other.gameObject.CompareTag("Ragdoll")) return;

        NPCIdentifier npc = other.GetComponent<NPCIdentifier>();    //NPCIdentifier스크립트랑 작용 (위장 컴퍼넌트)
        NPCFSM npcFSM = other.GetComponent<NPCFSM>(); // NPCFSM 가져오기 (상태 컴퍼넌트)
        NPCRichMan rich = other.GetComponent<NPCRichMan>(); //RichMan 전용 컴퍼넌트 받아오기

        Debug.Log("현재 변장중인 직업이 맞는지 : " + (npc == disguisedNPC));

        //NPCIdentifier 있는 상태
        if (npc != null)
        {
            // 변장 상태에서, 변장한 NPC와 동일한 NPC라면?
            if (isDisguised && npc == disguisedNPC)
            {
                eImage.gameObject.SetActive(false); // 대화 UI 비활성화
                eSlider.gameObject.SetActive(false); // E키 UI 비활성화
                return; // 변장한 NPC와는 상호작용할 수 없으므로 여기서 종료
            }
            else
            {
                currentNPC = npc; // 변장하지 않았다면, 현재 NPC 설정
            }
        }
        

        //NPCRichMan 있는 상태
        if (rich != null)
        {
            // 변장 상태에서, Rich로 변장해있다면?
            if (isDisguised && rich == disguisedNPC)
            {
                Debug.Log("UI가 안숨겨짐.");
                eImage.gameObject.SetActive(false); //위장 E키 이미지 UI 비활성화
                eSlider.gameObject.SetActive(false); //위장 E키 슬라이더 UI 비활성화
                return; // 변장한 NPC와는 상호작용할 수 없으므로 여기서 종료
            }
            else
            {
                currentNPC = npc; // 변장하지 않았다면, 현재 NPC 설정
            }

            if (rich.isDead)
            {
                //죽은 Rich와 상호작용 시
                E_Chat.gameObject.SetActive(false); //대화 UI 비활성화
                eImage.gameObject.SetActive(true);  //위장 이미지 활성화
                eSlider.gameObject.SetActive(true); //위장 슬라이더 활성화
                fImage.gameObject.SetActive(false); //F키 제압 UI 비활성화
                gImage.gameObject.SetActive(true);  //G키 잡기 UI 활성화
            }
            else
            {
                //살아있는 Rich와 상호작용 시
                E_Chat.gameObject.SetActive(false); //대화 UI 비활성화
                eImage.gameObject.SetActive(false); //위장 이미지 비활성화
                eSlider.gameObject.SetActive(false);//위장 슬라이더 비활성화
                fImage.gameObject.SetActive(true);  // 살아있는 NPC일 때 F키 관련 UI 활성화
                gImage.gameObject.SetActive(false); // 살아있는 NPC일 때 G키 UI 비활성화
            }
        }

        //NPCFSM 있는 상태
        if (npcFSM != null)
        {
            //NPC가 죽었다면?
            if (npcFSM.isDead)
            {
                E_Chat.gameObject.SetActive(false); //대화 UI 비활성화
                eImage.gameObject.SetActive(true);  //위장 이미지 활성화
                eSlider.gameObject.SetActive(true); //위장 슬라이더 활성화
                fImage.gameObject.SetActive(false); //F키 제압 UI 비활성화
                gImage.gameObject.SetActive(true);  //G키 잡기 UI 활성화
            }
            else //NPC가 살아있다면?
            {
                eImage.gameObject.SetActive(false);  //위장 이미지 비활성화
                eSlider.gameObject.SetActive(false); //위장 슬라이더 비활성화
                fImage.gameObject.SetActive(true);   //F키 제압 UI 활성화
                gImage.gameObject.SetActive(false);  //G키 잡기 UI 비활성화
            }
        }

        ////Rich 있는 상태 (대화하기 상호작용 없기 때문에 E_Chat은 상시 꺼줘야함.
        //if(rich != null)
        //{
        //    if(rich.isDead)
        //    {
        //        //죽은 Rich와 상호작용 시
        //        E_Chat.gameObject.SetActive(false); //대화 UI 비활성화
        //        eImage.gameObject.SetActive(true);  //위장 이미지 활성화
        //        eSlider.gameObject.SetActive(true); //위장 슬라이더 활성화
        //        fImage.gameObject.SetActive(false); //F키 제압 UI 비활성화
        //        gImage.gameObject.SetActive(true);  //G키 잡기 UI 활성화
        //    }
        //    else
        //    {
        //        //살아있는 Rich와 상호작용 시
        //        E_Chat.gameObject.SetActive(false); //대화 UI 비활성화
        //        eImage.gameObject.SetActive(false); //위장 이미지 비활성화
        //        eSlider.gameObject.SetActive(false);//위장 슬라이더 비활성화
        //        fImage.gameObject.SetActive(true);  // 살아있는 NPC일 때 F키 관련 UI 활성화
        //        gImage.gameObject.SetActive(false); // 살아있는 NPC일 때 G키 UI 비활성화
        //    }
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        //NPC Tag와 TriggerExit 시
        if (other.CompareTag("NPC") || other.CompareTag("NPCTeller"))
        {
            eImage.gameObject.SetActive(false); //위장 이미지 비활성화
            eSlider.gameObject.SetActive(false);//위장 슬라이더 비활성화
            fImage.gameObject.SetActive(false); //제압 F키 비활성화
            gImage.gameObject.SetActive(false); //잡기 G키 비활성화
            E_Chat.SetActive(false);            //대화 E키 비활성화
        }

        ////NPCTeller Tag와 TriggerExit 시
        //if (other.CompareTag("NPCTeller"))
        //{
        //    eImage.gameObject.SetActive(false); //위장 이미지 비활성화
        //    eSlider.gameObject.SetActive(false);
        //    fImage.gameObject.SetActive(false);
        //    gImage.gameObject.SetActive(false);
        //    E_Chat.SetActive(false);
        //}

        //Car와 TriggerExit 시
        if (other.CompareTag("Car"))
        {
            CarKey.SetActive(false);    //차 발로차기 버튼 비활성화
            carAlarm = null;    //carAlarm을 null로 설정하여, 차량과 상호작용 불가하도록 만듦
        }

        //TrashBin과 TriggerExit 시
        if (other.CompareTag("TrashBin"))
        {
            trashBin = null;    //trashBin을 null로 설정하여, 차량과 상호작용 불가하도록 만듦
        }

        //Ragdoll과 TriggerExit 시 (rich가 isDead되면 Ragdoll이 됨)
        if (other.CompareTag("Ragdoll"))
        {
            eImage.gameObject.SetActive(false); //위장키 비활성화
            eSlider.gameObject.SetActive(false);//위장키 비활성화
            gImage.gameObject.SetActive(false); //잡기 비활성화
        }

        //if (!other.gameObject.CompareTag("Ragdoll")) return;
        //{
        //    //NPCFSM npcFSM = other.GetComponent<NPCFSM>();

        //    //if (npcFSM != null && npcFSM.isDead)
        //    //{
        //    Debug.Log("죽은 NPC에서 멀어짐");
        //    eImage.gameObject.SetActive(false);
        //    eSlider.gameObject.SetActive(false);
        //    //}

        //    // NPCIdentifier가 있는 오브젝트에서 벗어나면 currentNPC 해제
        //    //if (other.GetComponent<NPCIdentifier>() == currentNPC)
        //    //{
        //    currentNPC = null;
        //    //}

        //    // NPC가 떠나면 fImage 비활성화
        //    fImage.gameObject.SetActive(false);
        //}
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

    //    // 걷기(Walk)
    //    if (Input.GetKey(KeyCode.LeftControl))
    //    {
    //        anim.SetBool("Walk", true);
    //    }
    //    else
    //    {
    //        anim.SetBool("Walk", false);
    //    }

    //    // 앉기(Crouch)
    //    if (Input.GetKeyDown(KeyCode.C))
    //    {
    //        isCrouching = !isCrouching; // 상태 전환
    //        anim.SetBool("Crouch", isCrouching); // 애니메이터의 Crouch 파라미터 설정
    //    }
        
    //    // 뛰기(Sprint)
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
        // 입력 받기 (WASD 혹은 방향키)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");


        // 카메라를 기준으로 이동 방향 계산
        Vector3 forward = mainCam.transform.forward;
        Vector3 right = mainCam.transform.right;

        forward.y = 0f; // 수평 방향만 계산
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        anim.SetFloat("Forward", vertical);
        anim.SetFloat("Right", horizontal);

        moveDirection = forward * vertical + right * horizontal;

        // 캐릭터 이동
        if (moveDirection.magnitude > 0.1f)
        {
            RotateViewForward();
        }

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
    private void RotateViewForward()
    {
        Vector3 camForward = mainCam.transform.forward;
        camForward.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(camForward);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * 5f); // 회전 속도 조정
    }

    private void PlayerAction()
    {
        if (Input.GetKey(KeyCode.E) && !isDisguised && currentNPC != null)
        {
            // currentNPC에서 NPCFSM 컴포넌트를 가져옴
            NPCFSM npcFSM = currentNPC.GetComponent<NPCFSM>();
            NPCRichMan rich = currentNPC.GetComponent<NPCRichMan>();
            Debug.Log($"{npcFSM},{rich} 받아오고 있음");

            //NPC의 위장E키 관련 함수
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

            //rich의 위장E키 관련 함수
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
        else //손뗐을 때
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
            NPCRichMan rich = currentNPC.GetComponent<NPCRichMan>();

            anim.SetTrigger("Neutralize");
                
            // NPC가 죽었든 살아있든 F키를 누르면 바로 UI를 끄기
            fImage.gameObject.SetActive(false);
            E_Chat.gameObject.SetActive(false);

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
        yield return new WaitForSeconds(1f); // 1초 대기
        EventManager.Trigger(GameEventType.Carkick);
        Debug.Log("차킥 이벤트 발생");
        carAlarm.ActivateAlarm(); // 도난방지 알람 실행
    }

    private IEnumerator SuicideCoroutine()
    {
        isSuiciding = true; // 코루틴 실행 중 중복 입력 방지

        disguiser.ResetToDefaultCharacter(); // 기본 복장으로 돌아감

        anim.SetTrigger("Suicide");

        // Upper Layer에서 'Suicide' 애니메이션 상태가 될 때까지 대기
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(1).IsName("Suicide"));

        // Forward, Right 값을 고정
        anim.SetFloat("Forward", 0f);
        anim.SetFloat("Right", 0f);

        if (gun != null)
        {
            gun.SetActive(true);
        }

        Time.timeScale = 0.2f; // 슬로우 모션 적용

        // Suicide 애니메이션이 실행된 후 0.35초 대기
        yield return new WaitForSeconds(0.35f * Time.timeScale);

        // 1. 머즐플래시 재생
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // 2. 총구 화염 켜기
        if (muzzleFlashLight != null)
        {
            muzzleFlashLight.SetActive(true);
        }

        // 3. 총성 재생
        playerSound.Gunshot();

        // 4. 혈흔 효과 켜기
        if (bloodEffect != null)
        {
            bloodEffect.SetActive(true);
        }

        // 0.1초 후 총구 화염 끄기
        yield return new WaitForSeconds(0.1f * Time.timeScale);

        if (muzzleFlashLight != null)
        {
            muzzleFlashLight.SetActive(false);
        }


        // Upper Layer에서 Suicide 애니메이션의 길이를 가져옴
        float animLength = anim.GetCurrentAnimatorStateInfo(1).length + anim.GetCurrentAnimatorStateInfo(2).length;

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
        EventManager.Trigger(GameEventType.GameOver);
    }
}
