using UnityEngine;

public class RagdollGrabber : MonoBehaviour
{
    public Transform ragdollHoldPoint; // Ragdoll�� ������ ��ġ (��: �÷��̾� ����)
    public Transform leftHandIKTarget; // �޼� IK Ÿ��
    public Transform rightHandIKTarget; // ������ IK Ÿ��
    private float ikLerpSpeed = 10f; // IK ���� �ӵ�

    private Animator anim;
    private ConfigurableJoint joint;
    //[HideInInspector]
    public Rigidbody ragdollRigidbody;
    private Transform rootTransform; // NPC �ֻ��� ������Ʈ�� Transform
    private Collider[] rootColliders; // NPC �ֻ��� ������Ʈ�� Collider ���
    //[HideInInspector]
    public bool isGrabbing = false;

    private Vector3 leftHandIKPosition;
    private Quaternion leftHandIKRotation;
    private Vector3 rightHandIKPosition;
    private Quaternion rightHandIKRotation;


    private void Start()
    {
        anim = GetComponent<Animator>();

        // ���� �ʱ� ��ġ�� ȸ�� ����
        leftHandIKPosition = anim.GetIKPosition(AvatarIKGoal.LeftHand);
        leftHandIKRotation = anim.GetIKRotation(AvatarIKGoal.LeftHand);
        rightHandIKPosition = anim.GetIKPosition(AvatarIKGoal.RightHand);
        rightHandIKRotation = anim.GetIKRotation(AvatarIKGoal.RightHand);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!isGrabbing)
            {
                anim.SetBool("isGrabbingRagdoll", true);
                TryGrabRagdoll();
            }
            else
            {
                anim.SetBool("isGrabbingRagdoll", false);
                ReleaseRagdoll();
            }
        }
    }

    public bool IsHoldingRagdoll(Transform ragdoll)
    {
        return isGrabbing && rootTransform == ragdoll;
    }

    private void TryGrabRagdoll()
    {
        // ����� Ragdoll�� Rigidbody ã��
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (var col in colliders)
        {
            if (col.attachedRigidbody != null && col.attachedRigidbody.CompareTag("Ragdoll")) // Ragdoll �±׸� ���ؼ� ���׵� ����
            {
                ragdollRigidbody = col.attachedRigidbody;
                AttachRagdoll();
                return;
            }
        }

        // �ֺ��� Ragdoll�� ���� ���
        isGrabbing = false;
        anim.SetBool("isGrabbingRagdoll", false);
    }

    private void AttachRagdoll()
    {
        // ���� ����
        isGrabbing = true;

        // Ragdoll�� ��Ʈ ������Ʈ ã��
        rootTransform = ragdollRigidbody.transform.root; // �ֻ��� �θ� ã��

        // �θ� ������Ʈ�� ��� Collider ���� �� ��Ȱ��ȭ
        rootColliders = rootTransform.GetComponents<Collider>();
        foreach (var col in rootColliders)
        {
            col.enabled = false;
        }

        //ragdollRigidbody.position = handIKTarget.position;

        // HoldPoint ��ġ�� ���� Ragdoll �̵�
        ragdollRigidbody.MovePosition(ragdollHoldPoint.position);
        ragdollRigidbody.MoveRotation(ragdollHoldPoint.rotation);

        // Configurable Joint �߰�
        joint = ragdollRigidbody.gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = ragdollHoldPoint.GetComponent<Rigidbody>();

        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;

        // ���׵��� ���� ������� �ϴ� �� ����
        JointDrive drive = new JointDrive();
        drive.positionSpring = 200; // ���� ������� ��(������ �� �ε巴�� ����)
        drive.positionDamper = 50;
        drive.maximumForce = 1000;

        joint.xDrive = drive;
        joint.yDrive = drive;
        joint.zDrive = drive;
    }

    public void ReleaseRagdoll()
    {
        isGrabbing = false;

        // �ֻ��� �θ� ������Ʈ�� Collider �ٽ� Ȱ��ȭ
        if (rootColliders != null)
        {
            foreach (var col in rootColliders)
            {
                col.enabled = true;
            }
        }

        // Configurable Joint ����
        if (joint != null)
        {
            Destroy(joint);
            joint = null;
        }

        ragdollRigidbody = null;
        rootTransform = null; // ��Ʈ Transform �ʱ�ȭ
    }

    // IK ����
    private void OnAnimatorIK(int layerIndex)
    {
        if (anim)
        {
            if (isGrabbing)
            {
                // Lerp & Slerp�� �̿��� �ε巯�� IK �̵�
                leftHandIKPosition = Vector3.Lerp(leftHandIKPosition,
                    leftHandIKTarget.position, Time.deltaTime * ikLerpSpeed);
                leftHandIKRotation = Quaternion.Slerp(leftHandIKRotation,
                    leftHandIKTarget.rotation, Time.deltaTime * ikLerpSpeed);
                rightHandIKPosition = Vector3.Lerp(rightHandIKPosition,
                    rightHandIKTarget.position, Time.deltaTime * ikLerpSpeed);
                rightHandIKRotation = Quaternion.Slerp(rightHandIKRotation,
                    rightHandIKTarget.rotation, Time.deltaTime * ikLerpSpeed);

                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

                anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIKPosition);
                anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIKRotation);
                anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandIKPosition);
                anim.SetIKRotation(AvatarIKGoal.RightHand, rightHandIKRotation);
            }
            else
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            }
        }
    }
}