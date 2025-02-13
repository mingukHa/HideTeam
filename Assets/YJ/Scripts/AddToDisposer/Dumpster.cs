using UnityEngine;
using System.Collections;

public class Dumpster : MonoBehaviour
{
    //public Transform disposalPoint; // Ragdoll�� ���� ��ġ
    public Transform lid; // �Ѳ� ������Ʈ
    private Rigidbody ragdollRigidbody;
    private Transform ragdollRoot;
    private Animator playerAnim;
    private bool isPlayerNearby = false;
    private bool isRagdollNearby = false;
    private Quaternion lidClosedRotation;
    private Quaternion lidOpenRotation;

    private void Start()
    {
        // �Ѳ��� �ʱ� ȸ�� ����
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

        // �Ѳ� ����
        StartCoroutine(RotateLid(lidOpenRotation));

        // �÷��̾ ��� �ִٸ� ����
        RagdollGrabber grabber = FindFirstObjectByType<RagdollGrabber>();
        if (grabber != null && grabber.IsHoldingRagdoll(ragdollRoot))
        {
            playerAnim.SetBool("isGrabbingRagdoll", false);
            grabber.ReleaseRagdoll();

            // Ragdoll ����
            Destroy(ragdollRoot.gameObject);
        }


        // �÷��̾� �ִϸ����Ϳ��� "IsDisposer" Ʈ���� ����
        if (playerAnim != null)
        {
            playerAnim.SetTrigger("isDisposer");
        }

        // Ragdoll�� Disposal Point�� �̵�
        StartCoroutine(MoveToDisposal());
    }

    private IEnumerator MoveToDisposal()
    {
        // 1�� ��� �� �̵� ����
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

        lid.rotation = targetRotation; // ��Ȯ�� �� ����
    }
}
