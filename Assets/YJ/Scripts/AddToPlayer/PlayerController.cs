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

    public Image eImage;    //E키 이미지
    public Slider eSlider;  //E키 게이지

    public Image rImage;    //R키 이미지
    public Slider rSlider;  //R키 게이지

    private float eholdTime = 0f;   //E키 누른 시간
    private float eGoalholdTime = 1f;   //E키 눌러야하는 시간

    private float rholdTime = 0f;   //E키 누른 시간
    private float rGoalholdTime = 1f;   //E키 눌러야하는 시간

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

        // 플레이어가 움직이고 있을 때만 마우스 입력 처리
        if (isMoving && InputMouse(ref mouseX))
        {
            InputMouseProcess(mouseX);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 만약 other 오브젝트의 태그가 "NPC"가 아니면 반환 (실행 X)
        if (!other.gameObject.CompareTag("NPC")) return;

        // NPCIdentifier가 있는 오브젝트와 충돌 시, currentNPC 설정
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

        // NPCIdentifier가 있는 오브젝트에서 벗어나면 currentNPC 해제
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
        if (Input.GetKey(KeyCode.E))
        {
            eholdTime += Time.deltaTime; //누른 시간 증가
            eSlider.value = eholdTime / eGoalholdTime;  //시간만큼 슬라이더 게이지

            if(eholdTime >= eGoalholdTime)
            {
                disguiser.ChangeAppearance(currentNPC); // NPC 정보를 사용해 변장 실행
                isDisguised = true;
                eholdTime = 0f; //다시 초기화
            }
        }
        else
        {
            eholdTime = 0f; //키에서 손 떼면 0초로 초기화
            eSlider.value = 0f; //슬라이더 게이지 초기화
        }

        if (isDisguised && Input.GetKey(KeyCode.R))
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

        if (Input.GetKeyDown(KeyCode.F))
        {
            // NPC 무력화 로직 추가
            anim.SetTrigger("Neutralize");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(SuicideCoroutine());
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
    }
}
