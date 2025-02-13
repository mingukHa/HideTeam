using UnityEngine;

public class RagdollGrabber : MonoBehaviour
{

    public Transform ragdollHoldPoint; // Ragdoll을 붙잡을 위치 (예: 플레이어 앞쪽)
    public Transform handIKTarget; // IK 타겟 (오른손 포지션)

    private Animator anim;
    private ConfigurableJoint joint;
    private Rigidbody ragdollRigidbody;
    private bool isGrabbing = false;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!isGrabbing)
            {
                anim.SetBool("isGrabbingRagdoll", true);
                TryGrabRagdoll();
                Debug.LogWarning("래그돌 붙잡음");
            }
            else
            {
                anim.SetBool("isGrabbingRagdoll", false);
                ReleaseRagdoll();
                Debug.LogWarning("래그돌 놓음");
            }
        }
    }

    private void TryGrabRagdoll()
    {
        // 가까운 Ragdoll의 Rigidbody 찾기
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
        foreach (var col in colliders)
        {
            if (col.attachedRigidbody != null && col.attachedRigidbody.CompareTag("Ragdoll")) // Ragdoll 태그를 비교해서 래그돌 판정
            {
                ragdollRigidbody = col.attachedRigidbody;
                AttachRagdoll();
                return;
            }
        }
    }

    private void AttachRagdoll()
    {
        // 잡은 상태
        isGrabbing = true;

        handIKTarget.position = ragdollRigidbody.position;

        // Configurable Joint 추가
        joint = ragdollRigidbody.gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = ragdollHoldPoint.GetComponent<Rigidbody>();

        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;

        // 래그돌이 손을 따라오게 하는 힘 적용
        JointDrive drive = new JointDrive();
        drive.positionSpring = 200; // 손을 따라오는 힘(낮으면 더 부드럽게 끌림)
        drive.positionDamper = 50;
        drive.maximumForce = 1000;

        joint.xDrive = drive;
        joint.yDrive = drive;
        joint.zDrive = drive;
    }

    private void ReleaseRagdoll()
    {
        isGrabbing = false;
        if (joint != null)
        {
            Destroy(joint);
            joint = null;
        }
        ragdollRigidbody = null;
    }

    // IK 적용
    private void OnAnimatorIK(int layerIndex)
    {
        if (anim && isGrabbing)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

            anim.SetIKPosition(AvatarIKGoal.RightHand, handIKTarget.position);
            anim.SetIKRotation(AvatarIKGoal.RightHand, handIKTarget.rotation);
        }
        else
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
        }
    }
}
