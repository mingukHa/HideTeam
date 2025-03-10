using System.Collections.Generic;
using UnityEngine;

public class RagdollGrabber : MonoBehaviour
{
    public Transform ragdollHoldPoint; // Grab �� Ragdoll ���� ����
    public Transform leftHandIKTarget; // Grab �� �� �޼��� ��ġ�� ����
    public Transform rightHandIKTarget; // Grab �� �� �������� ��ġ�� ����
    private float ikLerpSpeed = 10f; // ���� ���������� �̿��� �̵��� �� �����̴� �ӵ�

    private Animator anim;
    private ConfigurableJoint joint;
    [HideInInspector]
    public Rigidbody ragdollRigidbody; // Grab �� Ragdoll ����
    private Transform rootTransform; // Ragdoll�� Root(�ֻ���) Object Transform
    private Collider[] rootColliders; // Ragdoll�� ������ ��� Collider�� �迭
    [HideInInspector]
    public bool isGrabbing = false; // Ragdoll�� Grab ������ ����

    private Vector3 leftHandIKPosition; // Grab �� �޼��� Position
    private Quaternion leftHandIKRotation; // Grab �� �޼��� Rotation
    private Vector3 rightHandIKPosition; // Grab �� �������� Position
    private Quaternion rightHandIKRotation; // Grab �� �������� Rotation

    private Rigidbody rootRigidbody; // Ragdoll�� Root Object�� �߰��Ǵ� Rigidbody
    private ConfigurableJoint rootJoint; // Ragdoll�� Root Object�� �߰��Ǵ� Configurable Joint
    private float groundLevel; // ������ ��ġ

    private Dictionary<Collider, bool> originalTriggerStates =
        new Dictionary<Collider, bool>(); // Ragdoll Root Object Collider���� �ʱ� ���� ����(isTrigger ����)

    private void Start()
    {
        anim = GetComponent<Animator>();

        // �÷��̾��� �޼�, ������ �ʱ� ��ġ �� ȸ���� ����
        leftHandIKPosition = anim.GetIKPosition(AvatarIKGoal.LeftHand);
        leftHandIKRotation = anim.GetIKRotation(AvatarIKGoal.LeftHand);
        rightHandIKPosition = anim.GetIKPosition(AvatarIKGoal.RightHand);
        rightHandIKRotation = anim.GetIKRotation(AvatarIKGoal.RightHand);

        // Raycast�� ������ ��ġ�� �޾ƿ�
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            groundLevel = hit.point.y; // ���� �ٴ��� Y ��ǥ ����
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!isGrabbing) // Ragdoll�� ����� ���� ���� �� ���������� �õ���
            {
                anim.SetBool("isGrabbingRagdoll", true); // Animator�� Grabbing Ragdoll Blend Tree�� ��ȯ
                TryGrabRagdoll();
            }
            else // Ragdoll�� ����� ���� �� ������
            {
                anim.SetBool("isGrabbingRagdoll", false); // Idle ���·� �ٽ� ��ȯ
                ReleaseRagdoll();
            }
        }
    }

    private void FixedUpdate()
    {
        //if (isGrabbing && rootTransform != null)
        //{
        //    Vector3 targetPos = ragdollHoldPoint.position;
        //    rootRigidbody.MovePosition(Vector3.Lerp(rootRigidbody.position, targetPos, Time.fixedDeltaTime * 5f));
        //}

        // Ragdoll�� ���� �Ʒ��� �İ���� �ʵ��� ��
        if (ragdollRigidbody != null && ragdollRigidbody.transform.position.y < groundLevel) // Ragdoll�� Y���� ���麸�� �Ʒ��� ���� ���
        {
            Vector3 fixedPosition = ragdollRigidbody.transform.position;
            fixedPosition.y = groundLevel + 0.1f;  // Ragdoll�� Y���� �ٴں��� ��¦ ���� ����
            ragdollRigidbody.MovePosition(fixedPosition); // Ragdoll�� ������ ��ġ�� �̵�
        }
    }

    public bool IsHoldingRagdoll(Transform ragdoll)
    {
        // Ragdoll�� Grab ������ Ragdoll Disposer�� ������ ��
        return isGrabbing && rootTransform == ragdoll;
    }

    private void TryGrabRagdoll()
    {
        // �ݰ� 1f ���� Collider�� Ž���Ͽ� �迭�� ����
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (var col in colliders)
        {
            // �ش� �迭���� Ragdoll �±װ� ���� Rigidbody�� ���� ���
            if (col.attachedRigidbody != null && col.attachedRigidbody.CompareTag("Ragdoll"))
            {
                ragdollRigidbody = col.attachedRigidbody; // �� �� ù ��°�� ragdollRigidbody�� ����
                AttachRagdoll(); // Player���� �����ϱ� ���� AttachRagdoll �޼ҵ� ����
                return;
            }
        }

        // Collider �迭�� Ragdoll �±װ� ���� ���
        isGrabbing = false;
        anim.SetBool("isGrabbingRagdoll", false);
    }

    private void AttachRagdoll()
    {
        isGrabbing = true;
        rootTransform = ragdollRigidbody.transform.root; // Grab �� Ragdoll�� Root Object Transform ����
        rootColliders = rootTransform.GetComponentsInChildren<Collider>(); // Grab �� Ragdoll�� Root Object ���� ��� Collider�� ������
        originalTriggerStates.Clear(); // Collider�� �ʱ� ������ �ʱ�ȭ

        // Root Object ���� ��� Rigidbody ������
        Rigidbody[] allRigidbodies = rootTransform.GetComponentsInChildren<Rigidbody>();

        // Ragdoll�� �ٸ� Object �浹 ������ ���� Collider ����
        foreach (var col in rootColliders)
        {
            if (col.attachedRigidbody == ragdollRigidbody) // Grab�� Ragdoll ������ ������ Collider�� ������ ����
                continue;

            originalTriggerStates[col] = col.isTrigger; // ������ Collider�� isTrigger ���� ����
            col.isTrigger = true; // ����ó�� �� ������ �����ϰ� ��� Collider�� isTrigger Ȱ��ȭ
        }

        // Ragdoll�� �ٸ� Object �浹 ������ ���� Rigidbody ����
        foreach (var rb in allRigidbodies)
        {
            rb.useGravity = false; // �߷� ����
            rb.interpolation = RigidbodyInterpolation.Interpolate; // Interpolate �����Ͽ� ���� ȿ���� �ε巴�� ��
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // �浹 ������ Continuous�� ����
        }

        // Ragdoll Root Object�� Rigidbody �߰�
        if (!rootTransform.TryGetComponent(out rootRigidbody))
        {
            rootRigidbody = rootTransform.gameObject.AddComponent<Rigidbody>();
            rootRigidbody.mass = 10f; // ���� 10���� ����
        }

        // Root Object Rigidbody ����
        rootRigidbody.isKinematic = false; // ���� �������� ������ �� �ֵ��� isKinematic ��
        rootRigidbody.useGravity = false; // �߷� ����
        rootRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        rootRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // Root Object�� Ragdoll�� �����ϴ� Configurable Joint ����
        joint = ragdollRigidbody.gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = rootRigidbody; // �߰��� Configurable Joint�� Root Object�� ����

        // Configurable Joint ������ ����
        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;
        joint.angularXMotion = ConfigurableJointMotion.Limited;
        joint.angularYMotion = ConfigurableJointMotion.Limited;
        joint.angularZMotion = ConfigurableJointMotion.Limited;

        // Joint�� ���� ���� ����
        JointDrive drive = new JointDrive
        {
            positionSpring = 100f, // Joint�� ��ġ�� �����Ϸ��� Spring ����
            positionDamper = 10f, // �����ӿ� ���� Damper(����) ����
            maximumForce = 1000f // Joint�� ����ϴ� �ִ� ��
        };
        
        // ��� �࿡ ������ ���� ����
        joint.xDrive = drive;
        joint.yDrive = drive;
        joint.zDrive = drive;

        // Root Object�� HoldPoint�� �����ϴ� Configurable Joint ����
        rootJoint = rootTransform.gameObject.AddComponent<ConfigurableJoint>();
        rootJoint.connectedBody = ragdollHoldPoint.GetComponent<Rigidbody>(); // �߰��� Configurable Joint�� HoldPoint�� ����

        // Configurable Joint ������ ����
        rootJoint.xMotion = ConfigurableJointMotion.Limited;
        rootJoint.yMotion = ConfigurableJointMotion.Limited;
        rootJoint.zMotion = ConfigurableJointMotion.Limited;
        rootJoint.angularXMotion = ConfigurableJointMotion.Limited;
        rootJoint.angularYMotion = ConfigurableJointMotion.Limited;
        rootJoint.angularZMotion = ConfigurableJointMotion.Limited;
        rootJoint.breakForce = Mathf.Infinity;

        // Root Object�� Ragdoll�� ��ġ HoldPoint�� ����ȭ
        rootRigidbody.MovePosition(ragdollHoldPoint.position); // HoldPoint�� Root Object �̵�
        ragdollRigidbody.MovePosition(ragdollHoldPoint.position); // HoldPoint�� Ragdoll �̵�
    }


    public void ReleaseRagdoll()
    {
        isGrabbing = false;

        // ��� Ragdoll ������ Rigidbody �߷� �ٽ� Ȱ��ȭ
        Rigidbody[] allRigidbodies = rootTransform.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in allRigidbodies)
        {
            rb.useGravity = true;
        }

        // ����� Collider���� isTrigger ������ ����
        foreach (var entry in originalTriggerStates)
        {
            if (entry.Key != null) // Collider�� �����ϴ� ��츸 ����
            {
                entry.Key.isTrigger = entry.Value;
            }
        }

        // ���� �� ����Ʈ �ʱ�ȭ
        originalTriggerStates.Clear();

        // Grab�� Ragdoll ������ ������ Configurable Joint ����
        if (joint != null)
        {
            Destroy(joint);
            joint = null;
        }

        // Root Object�� ������ Configurable Joint ����
        if (rootJoint != null)
        {
            Destroy(rootJoint);
            rootJoint = null;
        }

        // Root Object�� ������ Rigidbody ����
        if (rootRigidbody != null)
        {
            Destroy(rootRigidbody);
            rootRigidbody = null;
        }

        // Grab�� Ragdoll ���� �� Root Object Transform �ʱ�ȭ
        ragdollRigidbody = null;
        rootTransform = null;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (anim)
        {
            if (isGrabbing)
            {
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
