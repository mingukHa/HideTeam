using UnityEngine;
using System.Collections;

public class Dumpster : MonoBehaviour
{
    //public Transform disposalPoint; // Ragdoll을 버릴 위치
    public Transform lid; // 뚜껑 오브젝트
    private Rigidbody ragdollRigidbody;
    private Transform ragdollRoot;
    private Animator playerAnim;
    private bool isPlayerNearby = false;
    private bool isRagdollNearby = false;
    private Quaternion lidClosedRotation;
    private Quaternion lidOpenRotation;

    private void Start()
    {
        // 뚜껑의 초기 회전 저장
        lidClosedRotation = lid.rotation;
        lidOpenRotation = lidClosedRotation * Quaternion.Euler(-80f, 0f, 0f);
    }

    private void Update()
    {
        if (isPlayerNearby && isRagdollNearby && Input.GetKeyDown(KeyCode.E))
        {
            DisposeRagdoll();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
            Destroy(ragdollRoot.gameObject);
        }


        // 플레이어 애니메이터에서 "IsDisposer" 트리거 실행
        if (playerAnim != null)
        {
            playerAnim.SetTrigger("isDisposer");
        }

        // Ragdoll을 Disposal Point로 이동
        StartCoroutine(MoveToDisposal());
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
