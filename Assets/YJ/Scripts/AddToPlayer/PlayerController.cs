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

    public Image eImage;    //E키 이미지
    public Slider eSlider;  //E키 게이지

    public Image rImage;    //R키 이미지
    public Slider rSlider;  //R키 게이지

    public Image fImage;    //K키 이미지

    public GameObject E_Chat;

    public GameObject CarKey;

    private float eholdTime = 0f;   //E키 누른 시간
    private float eGoalholdTime = 1f;   //E키 눌러야하는 시간

    private float rholdTime = 0f;   //R키 누른 시간
    private float rGoalholdTime = 1f;   //R키 눌러야하는 시간

    private bool isDisguised = false;
    private bool isGrabed = false;

    public List<DoorController> doorCons = new List<DoorController>();
    [SerializeField]
    private bool isFirstOpen = false;   // 출입문 첫 개방 확인용

    private NPCIdentifier disguisedNPC = null;  //변장한 NPC를 추적

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

        // 흡연이 끝나고 플레이어가 움직이고 있을 때만 마우스 입력 처리
        if (isMoving && InputMouse(ref mouseX))
        {
            InputMouseProcess(mouseX);
        }

        CheckFirstDoorOpen();
    }

    private void OnTriggerEnter(Collider other)
    {
        //    // 만약 other 오브젝트의 태그가 "NPC"가 아니면 반환 (실행 X)
        //    if (!other.gameObject.CompareTag("NPC")) return;

        //    NPCIdentifier npc = other.GetComponent<NPCIdentifier>();    
        //    NPCFSM npcFSM = other.GetComponent<NPCFSM>(); // NPCFSM 가져오기

        //    if (npcFSM != null && npcFSM.isDead)
        //    {
        //        Debug.Log("죽은 NPC와 상호작용");
        //        // NPCIdentifier가 있는 오브젝트와 충돌 시, currentNPC 설정
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
        //        // NPC가 살아있을 때 fImage 활성화
        //        fImage.gameObject.SetActive(true);
        //        currentNPC = npc; // NPC를 currentNPC에 설정
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

    //실시간으로 UI가 변해야하므로 Stay로 변경
    private void OnTriggerStay(Collider other)
    {
        // "NPC" 태그를 가진 오브젝트와만 처리
        if (!other.gameObject.CompareTag("NPC")) return;

        NPCIdentifier npc = other.GetComponent<NPCIdentifier>();
        NPCFSM npcFSM = other.GetComponent<NPCFSM>(); // NPCFSM 가져오기

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

        if (npcFSM != null)
        {
            if (npcFSM.isDead)
            {
                // 죽은 NPC와 상호작용 시 E키 활성화
                eImage.gameObject.SetActive(true);
                eSlider.gameObject.SetActive(true);
                fImage.gameObject.SetActive(false); // 죽은 NPC일 때 F키 관련 UI 비활성화
            }
            else
            {
                // 살아있는 NPC와 상호작용 시 E키 비활성화
                eImage.gameObject.SetActive(false);
                eSlider.gameObject.SetActive(false);
                fImage.gameObject.SetActive(true); // 살아있는 NPC일 때 F키 관련 UI 활성화
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
            carAlarm = null;  // carAlarm을 null로 설정하여, 차량과 상호작용 불가하도록 만듦
        }

        if (other.CompareTag("TrashBin"))
        {
            trashBin = null;
        }

        if (!other.gameObject.CompareTag("NPC")) return;

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

            if (npcFSM != null && npcFSM.isDead)
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
            EventManager.Trigger(GameEventType.Carkick);
            Debug.Log("차킥 이벤트 발생");
            carAlarm.ActivateAlarm(); // 도난방지 알람 실행
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

        if (Input.GetKeyDown(KeyCode.F) && currentNPC != null)
        {
            //NPC가 살아있을 때만 작동
            NPCFSM npcFSM = currentNPC.GetComponent<NPCFSM>();

            if (npcFSM != null && !npcFSM.isDead)
            {
                // NPC 무력화 로직 추가
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
        disguiser.ResetToDefaultCharacter(); // 기본 복장으로 돌아감

        if (gun != null)
        {
            gun.SetActive(true);
        }

        Time.timeScale = 0.6f; // 슬로우 모션 적용
        anim.SetTrigger("Suicide");

        // 애니메이션 재생 후 자의적 루프(자살) 로직 추가할 자리

        // 애니메이션 길이를 가져옴
        float animLength = anim.GetCurrentAnimatorStateInfo(0).length;

        // 애니메이션 길이만큼 대기 (시간 스케일 영향을 받지 않도록 WaitForSecondsRealtime 사용)
        yield return new WaitForSecondsRealtime(animLength);

        Time.timeScale = 1f; // 원래 속도로 복원
        gun.SetActive(false);
    }
}
