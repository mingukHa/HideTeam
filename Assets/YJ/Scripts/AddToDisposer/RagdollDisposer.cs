using System.Collections;
using UnityEngine;

public class RagdollDisposer : MonoBehaviour
{
    public Transform disposalPoint; // NPC�� �̵���ų Ư�� ����
    public float disposeTime = 2f; // �������뿡 �ִ� �ð�
    private bool isInDisposalRange = false; // ���� �������� ���� ���� �ִ��� Ȯ��
    private Animator playerAnimator;
    private RagdollGrabber ragdollGrabber; // Ragdoll�� ��� �ִ� ��ü (RagdollGrabber ��ũ��Ʈ ����)

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        ragdollGrabber = GetComponent<RagdollGrabber>(); // Player�� Ragdoll�� ��� �ִٰ� ����
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
        if (other.CompareTag("Ragdoll") && ragdollGrabber.isGrabbing) // Ragdoll�� �������뿡 ����� ��
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
        // Animator�� isDisposer �Ķ���͸� true�� �����Ͽ� �������뿡 Ragdoll�� �ִ� �ִϸ��̼� ����
        playerAnimator.SetBool("isDisposer", true);

        // Ragdoll�� ����, NPC�� Ư�� �������� �̵�
        StartCoroutine(DisposeRagdollCoroutine());
    }

    private IEnumerator DisposeRagdollCoroutine()
    {
        // Ragdoll�� ���� ���� �ִϸ��̼ǿ� ���� ��ٸ���
        yield return new WaitForSeconds(disposeTime); // disposeTime��ŭ ��ٸ� ��

        // Ragdoll�� ����
        ragdollGrabber.ReleaseRagdoll();

        // NPC�� Ư�� �������� �̵�
        if (ragdollGrabber.ragdollRigidbody != null)
        {
            ragdollGrabber.ragdollRigidbody.transform.position = disposalPoint.position;
            ragdollGrabber.ragdollRigidbody.transform.rotation = disposalPoint.rotation;
        }

        // Player Animator�� isDisposer �Ķ���͸� false�� �����Ͽ� �ִϸ��̼� ����
        playerAnimator.SetBool("isDisposer", false);
    }
}
