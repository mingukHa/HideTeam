using System.Collections;
using UnityEngine;

public class Dumpster : MonoBehaviour
{
    public Transform dumpsterLid; // ���� �������� �Ѳ�
    public Transform disposalPoint; // NPC ���׵��� �̵���ų ����
    private float lidRotationDuration = 1.5f; // 
    public float disposeTime = 2f; // �������뿡 �ִ� �ð�
    private bool isInDisposalRange = false; // ���� �������� ���� ���� �ִ��� Ȯ��
    private Animator anim;
    private RagdollGrabber ragdollGrabber; // RagdollGrabber ��ũ��Ʈ ����

    private void Start()
    {
        anim = GetComponent<Animator>();
        ragdollGrabber = GetComponent<RagdollGrabber>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInDisposalRange)//eee && ragdollGrabber.isGrabbing)
        {
            Debug.Log("��ü ó�� ���μ��� ����");
            //StartCoroutine(OpenDumpsterLid());
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
        // Animator�� isDisposer �Ķ���͸� true�� �����Ͽ� �������뿡 Ragdoll�� �ִ� �ִϸ��̼� ����
        anim.SetBool("isDisposer", true);

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
        anim.SetBool("isDisposer", false);
    }
}
