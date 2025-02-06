using UnityEngine;

public class RagdollGraber : MonoBehaviour
{
    private Animator anim;
    public Transform holdPoint; // Ragdoll�� ������ ��ġ (��: �÷��̾� ����)
    public KeyCode grabKey = KeyCode.F; // ��� Ű

    private FixedJoint joint;
    private Rigidbody ragdollRigidbody;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(grabKey))
        {
            if (joint == null)
            {
                TryGrabRagdoll();
                anim.SetBool("GrabbingRagdoll", true);
            }
            else
            {
                ReleaseRagdoll();
                anim.SetBool("GrabbingRagdoll", false);
            }
        }
    }

    private void TryGrabRagdoll()
    {
        // ����� Ragdoll�� Rigidbody ã��
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
        foreach (var col in colliders)
        {
            if (col.attachedRigidbody != null && col.attachedRigidbody.CompareTag("Ragdoll")) // Ragdoll �±׸� ���ؼ� ���׵� ����
            {
                ragdollRigidbody = col.attachedRigidbody;
                AttachRagdoll();
                return;
            }
        }
    }

    private void AttachRagdoll()
    {
        // Fixed Joint �߰�
        joint = ragdollRigidbody.gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = holdPoint.GetComponent<Rigidbody>();
        joint.breakForce = 5000; // �ʹ� ���� ����� ������ Joint ����
        joint.breakTorque = 5000;
    }

    private void ReleaseRagdoll()
    {
        if (joint != null)
        {
            Destroy(joint);
            joint = null;
            ragdollRigidbody = null;
        }
    }
}
