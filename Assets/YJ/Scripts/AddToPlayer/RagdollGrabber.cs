using UnityEngine;

public class RagdollGrabber : MonoBehaviour
{

    public Transform ragdollHoldPoint; // Ragdoll�� ������ ��ġ (��: �÷��̾� ����)
    public Transform handIKTarget; // IK Ÿ�� (������ ������)

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
                Debug.LogWarning("���׵� ������");
            }
            else
            {
                anim.SetBool("isGrabbingRagdoll", false);
                ReleaseRagdoll();
                Debug.LogWarning("���׵� ����");
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
        // ���� ����
        isGrabbing = true;

        handIKTarget.position = ragdollRigidbody.position;

        // Configurable Joint �߰�
        joint = ragdollRigidbody.gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = ragdollHoldPoint.GetComponent<Rigidbody>();

        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;

        // ���׵��� ���� ������� �ϴ� �� ����
        JointDrive drive = new JointDrive();
        drive.positionSpring = 200; // ���� ������� ��(������ �� �ε巴�� ����)
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

    // IK ����
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
