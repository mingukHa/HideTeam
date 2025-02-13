using UnityEngine;
using System.Collections;

public class Dumpster : MonoBehaviour
{
    public Transform disposalPoint; // Ragdoll을 버릴 위치
    public Transform lid; // 뚜껑 오브젝트
    private Rigidbody ragdollRigidbody;
    private Transform ragdollRoot;
    private bool isPlayerNearby = false;
    private bool isRagdollNearby = false;
    private Quaternion lidClosedRotation;
    private Quaternion lidOpenRotation;
    private Animation anim;

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
            anim = other.GetComponent<Animation>();
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
            anim = null;
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
        RagdollGrabber grabber = FindObjectOfType<RagdollGrabber>();
        if (grabber != null && grabber.IsHoldingRagdoll(ragdollRoot))
        {
            grabber.ReleaseRagdoll();
        }


        StartCoroutine(MoveToDisposal());
    }

    private IEnumerator MoveToDisposal()
    {
        float duration = 1.5f;
        float elapsed = 0f;
        Vector3 startPos = ragdollRoot.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            ragdollRoot.position = Vector3.Lerp(startPos, disposalPoint.position, elapsed / duration);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        // 뚜껑 닫기
        StartCoroutine(RotateLid(lidClosedRotation));

        // Ragdoll 삭제
        Destroy(ragdollRoot.gameObject);
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
