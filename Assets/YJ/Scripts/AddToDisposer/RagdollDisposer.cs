using System.Collections;
using UnityEngine;

public class RagdollDisposer : MonoBehaviour
{
    public Transform disposalPoint; // NPC를 이동시킬 특정 지점
    public float disposeTime = 2f; // 쓰레기통에 넣는 시간
    private bool isInDisposalRange = false; // 대형 쓰레기통 범위 내에 있는지 확인
    private Animator playerAnimator;
    private RagdollGrabber ragdollGrabber; // Ragdoll을 잡고 있는 객체 (RagdollGrabber 스크립트 참조)

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        ragdollGrabber = GetComponent<RagdollGrabber>(); // Player가 Ragdoll을 잡고 있다고 가정
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInDisposalRange && ragdollGrabber.isGrabbing)
        {
            DisposeRagdoll();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ragdoll") && ragdollGrabber.isGrabbing) // Ragdoll이 쓰레기통에 닿았을 때
        {
            isInDisposalRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ragdoll"))
        {
            isInDisposalRange = false;
        }
    }

    private void DisposeRagdoll()
    {
        // Animator의 isDisposer 파라미터를 true로 설정하여 쓰레기통에 Ragdoll을 넣는 애니메이션 시작
        playerAnimator.SetBool("isDisposer", true);

        // Ragdoll을 놓고, NPC를 특정 지점으로 이동
        StartCoroutine(DisposeRagdollCoroutine());
    }

    private IEnumerator DisposeRagdollCoroutine()
    {
        // Ragdoll을 놓는 동안 애니메이션에 맞춰 기다리기
        yield return new WaitForSeconds(disposeTime); // disposeTime만큼 기다린 후

        // Ragdoll을 놓기
        ragdollGrabber.ReleaseRagdoll();

        // NPC를 특정 지점으로 이동
        if (ragdollGrabber.ragdollRigidbody != null)
        {
            ragdollGrabber.ragdollRigidbody.transform.position = disposalPoint.position;
            ragdollGrabber.ragdollRigidbody.transform.rotation = disposalPoint.rotation;
        }

        // Player Animator의 isDisposer 파라미터를 false로 설정하여 애니메이션 종료
        playerAnimator.SetBool("isDisposer", false);
    }
}
