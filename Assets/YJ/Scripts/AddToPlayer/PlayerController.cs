using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim = null;
    private Transform tr = null;

    private NPCIdentifier currentNPC;
    private PlayerDisguiser disguiser;

    private float mouseX = 0;
    private float mouseSensitivity = 5f;
    private bool isMoving = false;
    private bool isCrouching = false;

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

        // 플레이어가 움직이고 있을 때만 마우스 입력 처리
        if (isMoving && InputMouse(ref mouseX))
        {
            InputMouseProcess(mouseX);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // NPCIdentifier가 있는 오브젝트와 충돌 시, currentNPC 설정
        NPCIdentifier npc = other.GetComponent<NPCIdentifier>();
        if (npc != null)
        {
            currentNPC = npc;
        }
    }

    private void OnTriggerExit(Collider other)
    {
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            disguiser.ChangeAppearance(currentNPC); // NPC 정보를 사용해 변장 실행
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            disguiser.ResetToDefaultCharacter();
        }
    }
}
