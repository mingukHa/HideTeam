using System.Collections.Generic;
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
    private float groundLevel;

    private Dictionary<Collider, bool> originalTriggerStates = new Dictionary<Collider, bool>(); // 기존 상태 저장

    private void Start()
    {
        anim = GetComponent<Animator>();

        leftHandIKPosition = anim.GetIKPosition(AvatarIKGoal.LeftHand);
        leftHandIKRotation = anim.GetIKRotation(AvatarIKGoal.LeftHand);
        rightHandIKPosition = anim.GetIKPosition(AvatarIKGoal.RightHand);
        rightHandIKRotation = anim.GetIKRotation(AvatarIKGoal.RightHand);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            groundLevel = hit.point.y; // 현재 바닥의 Y 좌표 감지
        }
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
        //if (isGrabbing && rootTransform != null)
        //{
        //    Vector3 targetPos = ragdollHoldPoint.position;
        //    rootRigidbody.MovePosition(Vector3.Lerp(rootRigidbody.position, targetPos, Time.fixedDeltaTime * 5f));
        //}

        if (ragdollRigidbody != null && ragdollRigidbody.transform.position.y < groundLevel)
        {
            Vector3 fixedPosition = ragdollRigidbody.transform.position;
            fixedPosition.y = groundLevel + 0.1f;  // 바닥보다 살짝 위로 유지
            ragdollRigidbody.MovePosition(fixedPosition);
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
        originalTriggerStates.Clear();

        // 모든 Rigidbody 가져오기 (루트 포함)
        Rigidbody[] allRigidbodies = rootTransform.GetComponentsInChildren<Rigidbody>();

        foreach (var col in rootColliders)
        {
            // Ragdoll에 부착된 Rigidbody를 가진 Collider는 저장하지 않음 (예외 처리)
            if (col.attachedRigidbody == ragdollRigidbody)
                continue;

            // 기존 isTrigger 상태 저장
            originalTriggerStates[col] = col.isTrigger;
            col.isTrigger = true; // Trigger 활성화
        }

        // 모든 Ragdoll 부위의 Rigidbody 설정 적용
        foreach (var rb in allRigidbodies)
        {
            rb.useGravity = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        // 루트 Rigidbody 추가
        if (!rootTransform.TryGetComponent(out rootRigidbody))
        {
            rootRigidbody = rootTransform.gameObject.AddComponent<Rigidbody>();
            rootRigidbody.mass = 10;
        }

        // Root Rigidbody 설정
        rootRigidbody.isKinematic = false; // 중요: 움직일 수 있도록 설정
        rootRigidbody.useGravity = false;
        rootRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        rootRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // HoldPoint로 즉시 이동
        rootRigidbody.MovePosition(ragdollHoldPoint.position);
        ragdollRigidbody.MovePosition(ragdollHoldPoint.position);

        // ragdoll을 root에 부착하는 Configurable Joint 설정
        joint = ragdollRigidbody.gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = rootRigidbody; // rootRigidbody에 연결 (HoldPoint가 아님)

        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;
        joint.angularXMotion = ConfigurableJointMotion.Limited;
        joint.angularYMotion = ConfigurableJointMotion.Limited;
        joint.angularZMotion = ConfigurableJointMotion.Limited;

        JointDrive drive = new JointDrive
        {
            positionSpring = 100,
            positionDamper = 10,
            maximumForce = 1000
        };

        joint.xDrive = drive;
        joint.yDrive = drive;
        joint.zDrive = drive;

        // 루트와 HoldPoint를 연결하는 조인트 추가
        rootJoint = rootTransform.gameObject.AddComponent<ConfigurableJoint>();
        rootJoint.connectedBody = ragdollHoldPoint.GetComponent<Rigidbody>(); // HoldPoint에 연결

        rootJoint.xMotion = ConfigurableJointMotion.Limited;
        rootJoint.yMotion = ConfigurableJointMotion.Limited;
        rootJoint.zMotion = ConfigurableJointMotion.Limited;
        rootJoint.angularXMotion = ConfigurableJointMotion.Limited;
        rootJoint.angularYMotion = ConfigurableJointMotion.Limited;
        rootJoint.angularZMotion = ConfigurableJointMotion.Limited;
        rootJoint.breakForce = Mathf.Infinity;
    }


    public void ReleaseRagdoll()
    {
        isGrabbing = false;

        // 모든 Ragdoll 부위의 Rigidbody 중력 다시 활성화
        Rigidbody[] allRigidbodies = rootTransform.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in allRigidbodies)
        {
            rb.useGravity = true;
        }

        // 저장했던 Collider 상태 복원
        foreach (var entry in originalTriggerStates)
        {
            if (entry.Key != null) // Collider가 존재하는 경우만 복원
            {
                entry.Key.isTrigger = entry.Value;
            }
        }

        originalTriggerStates.Clear(); // 복원 후 리스트 초기화

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
