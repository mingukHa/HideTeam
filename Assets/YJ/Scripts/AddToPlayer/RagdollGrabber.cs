using UnityEngine;

public class RagdollGraber : MonoBehaviour
{
    private Animator anim;
    public Transform holdPoint; // Ragdoll을 붙잡을 위치 (예: 플레이어 앞쪽)
    public KeyCode grabKey = KeyCode.F; // 잡기 키

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
        // 가까운 Ragdoll의 Rigidbody 찾기
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
        foreach (var col in colliders)
        {
            if (col.attachedRigidbody != null && col.attachedRigidbody.CompareTag("Ragdoll"))
            {
                ragdollRigidbody = col.attachedRigidbody;
                AttachRagdoll();
                return;
            }
        }
    }

    private void AttachRagdoll()
    {
        // Fixed Joint 추가
        joint = ragdollRigidbody.gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = holdPoint.GetComponent<Rigidbody>();
        joint.breakForce = 500; // 너무 강한 충격을 받으면 Joint 해제
        joint.breakTorque = 500;
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
