using UnityEngine;

public class RagdollGrabber : MonoBehaviour
{
    public Transform ragdollHoldPoint;
    public Transform leftHandIKTarget;
    public Transform rightHandIKTarget;
    private float ikLerpSpeed = 10f;

    private Animator anim;
    private ConfigurableJoint joint;
    [HideInInspector]
    public Rigidbody ragdollRigidbody;
    private Transform rootTransform;
    private Collider[] rootColliders;
    [HideInInspector]
    public bool isGrabbing = false;

    private Vector3 leftHandIKPosition;
    private Quaternion leftHandIKRotation;
    private Vector3 rightHandIKPosition;
    private Quaternion rightHandIKRotation;

    private Rigidbody rootRigidbody;
    private ConfigurableJoint rootJoint;

    private void Start()
    {
        anim = GetComponent<Animator>();

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

    private void FixedUpdate()
    {
        if (isGrabbing && rootTransform != null)
        {
            rootTransform.position = ragdollRigidbody.position;
        }
    }

    public bool IsHoldingRagdoll(Transform ragdoll)
    {
        return isGrabbing && rootTransform == ragdoll;
    }

    private void TryGrabRagdoll()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (var col in colliders)
        {
            if (col.attachedRigidbody != null && col.attachedRigidbody.CompareTag("Ragdoll"))
            {
                ragdollRigidbody = col.attachedRigidbody;
                AttachRagdoll();
                return;
            }
        }

        isGrabbing = false;
        anim.SetBool("isGrabbingRagdoll", false);
    }

    private void AttachRagdoll()
    {
        isGrabbing = true;
        rootTransform = ragdollRigidbody.transform.root;

        rootColliders = rootTransform.GetComponentsInChildren<Collider>();
        foreach (var col in rootColliders)
        {
            col.isTrigger = true;
        }

        // 루트 Rigidbody 추가
        if (!rootTransform.TryGetComponent(out rootRigidbody))
        {
            rootRigidbody = rootTransform.gameObject.AddComponent<Rigidbody>();
            rootRigidbody.mass = 10;
            rootRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        }

        // 루트 오브젝트 Rigidbody kinematic 활성화
        if (rootRigidbody != null)
        {
            rootRigidbody.isKinematic = true;
        }

        // HoldPoint로 이동
        rootTransform.position = ragdollHoldPoint.position;
        ragdollRigidbody.position = ragdollHoldPoint.position;

        // Configurable Joint 설정
        joint = ragdollRigidbody.gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = ragdollHoldPoint.GetComponent<Rigidbody>();

        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;

        JointDrive drive = new JointDrive
        {
            positionSpring = 200,
            positionDamper = 50,
            maximumForce = 1000
        };

        joint.xDrive = drive;
        joint.yDrive = drive;
        joint.zDrive = drive;

        // 루트와 래그돌을 연결하는 조인트 추가
        rootJoint = rootTransform.gameObject.AddComponent<ConfigurableJoint>();
        rootJoint.connectedBody = ragdollRigidbody;

        rootJoint.xMotion = ConfigurableJointMotion.Limited;
        rootJoint.yMotion = ConfigurableJointMotion.Limited;
        rootJoint.zMotion = ConfigurableJointMotion.Limited;
        rootJoint.angularXMotion = ConfigurableJointMotion.Limited;
        rootJoint.angularYMotion = ConfigurableJointMotion.Limited;
        rootJoint.angularZMotion = ConfigurableJointMotion.Limited;
        rootJoint.breakForce = Mathf.Infinity; // 강한 힘으로 인해 끊어지지 않도록 설정
    }

    public void ReleaseRagdoll()
    {
        isGrabbing = false;

        // 루트 오브젝트 Rigidbody kinematic 비활성화
        if (rootRigidbody != null)
        {
            rootRigidbody.isKinematic = false;
        }

        if (rootColliders != null)
        {
            foreach (var col in rootColliders)
            {
                col.isTrigger = false;
            }
        }

        if (joint != null)
        {
            Destroy(joint);
            joint = null;
        }

        if (rootJoint != null)
        {
            Destroy(rootJoint);
            rootJoint = null;
        }

        if (rootRigidbody != null)
        {
            Destroy(rootRigidbody);
            rootRigidbody = null;
        }

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
