using System.Collections;
using UnityEngine;

public class Dumpster : MonoBehaviour
{
    public Transform dumpsterLid; // 대형 쓰레기통 뚜껑
    public Transform disposalPoint; // NPC 래그돌을 이동시킬 지점
    private float lidRotationDuration = 1.5f; // 
    public float disposeTime = 2f; // 쓰레기통에 넣는 시간
    private bool isInDisposalRange = false; // 대형 쓰레기통 범위 내에 있는지 확인
    private Animator anim;
    private RagdollGrabber ragdollGrabber; // RagdollGrabber 스크립트 참조

    private void Start()
    {
        anim = GetComponent<Animator>();
        ragdollGrabber = GetComponent<RagdollGrabber>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInDisposalRange)//eee && ragdollGrabber.isGrabbing)
        {
            Debug.Log("시체 처리 프로세스 시작");
            //StartCoroutine(OpenDumpsterLid());
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

    private IEnumerator OpenDumpsterLid()
    {
        Quaternion startRotation = dumpsterLid.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(-80f, 0f, 0f);

        float elapsedTime = 0f;

        while (elapsedTime < lidRotationDuration)
        {
            dumpsterLid.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / lidRotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        dumpsterLid.rotation = targetRotation;
    }

    private void DisposeRagdoll()
    {
        // Animator의 isDisposer 파라미터를 true로 설정하여 쓰레기통에 Ragdoll을 넣는 애니메이션 시작
        anim.SetBool("isDisposer", true);

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
        anim.SetBool("isDisposer", false);
    }
}
