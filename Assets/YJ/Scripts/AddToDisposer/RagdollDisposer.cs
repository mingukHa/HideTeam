using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RagdollDisposer : MonoBehaviour
{
    //public Transform disposalPoint; // Ragdoll을 버릴 위치
    public Transform lid; // 뚜껑 or 문 오브젝트
    public RagdollGrabber ragdollgrab;
    private Rigidbody ragdollRigidbody;
    private Transform ragdollRoot;
    private Animator playerAnim;
    private bool isPlayerNearby = false;
    private bool isRagdollNearby = false;
    private bool isRichHide = false;
    private Quaternion lidClosedRotation;// 닫혀있는 뚜껑 각도
    [SerializeField]
    private Quaternion lidOpenRotation; // 열린 뚜껑 각도

    public Image egrabImage; //은폐 버튼

    private void Start()
    {
        egrabImage.gameObject.SetActive(false);
        lidClosedRotation = lid.rotation;
    }
    
    private void Update()
    {
        if (isPlayerNearby && isRagdollNearby && Input.GetKeyDown(KeyCode.E) && ragdollgrab.isGrabbing)
        {
            DisposeRagdoll();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && ragdollgrab.isGrabbing)
        {
            egrabImage.gameObject.SetActive(true);
            isPlayerNearby = true;
            playerAnim = other.GetComponent<Animator>();
        }

        if (other.CompareTag("Ragdoll"))
        {
            isRagdollNearby = true;
            ragdollRigidbody = other.attachedRigidbody;
            ragdollRoot = other.transform.root;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            egrabImage.gameObject.SetActive(false);
            isPlayerNearby = false;
            playerAnim = null;
        }

        if (other.CompareTag("Ragdoll"))
        {
            isRagdollNearby = false;
            ragdollRigidbody = null;
            ragdollRoot = null;
        }
    }

    private void DisposeRagdoll()
    {
        if (ragdollRoot == null) return;

        // 뚜껑 열기
        StartCoroutine(RotateLid(lidOpenRotation));

        // 플레이어가 잡고 있다면 해제
        RagdollGrabber grabber = FindFirstObjectByType<RagdollGrabber>();
        if (grabber != null && grabber.IsHoldingRagdoll(ragdollRoot))
        {
            playerAnim.SetBool("isGrabbingRagdoll", false);
            grabber.ReleaseRagdoll();

            // Ragdoll 삭제
            RichHide();
            Destroy(ragdollRoot.gameObject);
        }


        // 플레이어 애니메이터에서 "isDisposer" 트리거 실행
        if (playerAnim != null)
        {
            playerAnim.SetTrigger("isDisposer");
        }
        RichHide();
        // Ragdoll을 Disposal Point로 이동
        StartCoroutine(MoveToDisposal());
    }
    private void RichHide() //한번만 이벤트 호출
    {
        if (isRichHide == false)
        {
            EventManager.Trigger(EventManager.GameEventType.RichHide);
            isRichHide = true;
        }
        

    }
    private IEnumerator MoveToDisposal()
    {
        // 1초 대기 후 이동 시작
        yield return new WaitForSeconds(1f);

        StartCoroutine(RotateLid(lidClosedRotation));

    }


    private IEnumerator RotateLid(Quaternion targetRotation)
    {
        float duration = 1f;
        float elapsed = 0f;
        Quaternion startRotation = lid.rotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            lid.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsed / duration);
            yield return null;
        }

        lid.rotation = targetRotation; // 정확한 값 보정
    }
}
