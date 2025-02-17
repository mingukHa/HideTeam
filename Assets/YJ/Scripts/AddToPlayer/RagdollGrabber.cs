using UnityEngine;

public class RagdollGrabber : MonoBehaviour
{
    public Transform ragdollHoldPoint; // Ragdoll을 붙잡을 위치 (예: 플레이어 앞쪽)
    public Transform leftHandIKTarget; // 왼손 IK 타겟
    public Transform rightHandIKTarget; // 오른손 IK 타겟
    private float ikLerpSpeed = 10f; // IK 보간 속도

    private Animator anim;
    private ConfigurableJoint joint;
    //[HideInInspector]
    public Rigidbody ragdollRigidbody;
    private Transform rootTransform; // NPC 최상위 오브젝트의 Transform
    private Collider[] rootColliders; // NPC 최상위 오브젝트의 Collider 목록
    //[HideInInspector]
    public bool isGrabbing = false;

    private Vector3 leftHandIKPosition;
    private Quaternion leftHandIKRotation;
    private Vector3 rightHandIKPosition;
    private Quaternion rightHandIKRotation;


    private void Start()
    {
        anim = GetComponent<Animator>();

        // 손의 초기 위치와 회전 저장
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
        // 가까운 Ragdoll의 Rigidbody 찾기
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (var col in colliders)
        {
            if (col.attachedRigidbody != null && col.attachedRigidbody.CompareTag("Ragdoll")) // Ragdoll 태그를 비교해서 래그돌 판정
            {
                ragdollRigidbody = col.attachedRigidbody;
                AttachRagdoll();
                return;
            }
        }

        // 주변에 Ragdoll이 없는 경우
        isGrabbing = false;
        anim.SetBool("isGrabbingRagdoll", false);
    }

    private void AttachRagdoll()
    {
        // 잡은 상태
        isGrabbing = true;

        // Ragdoll의 루트 오브젝트 찾기
        rootTransform = ragdollRigidbody.transform.root; // 최상위 부모 찾기

        // 부모 오브젝트의 모든 Collider 저장 및 비활성화
        rootColliders = rootTransform.GetComponents<Collider>();
        foreach (var col in rootColliders)
        {
            col.enabled = false;
        }

        //ragdollRigidbody.position = handIKTarget.position;

        // HoldPoint 위치로 잡은 Ragdoll 이동
        ragdollRigidbody.MovePosition(ragdollHoldPoint.position);
        ragdollRigidbody.MoveRotation(ragdollHoldPoint.rotation);

        // Configurable Joint 추가
        joint = ragdollRigidbody.gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = ragdollHoldPoint.GetComponent<Rigidbody>();

        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;

        // 래그돌이 손을 따라오게 하는 힘 적용
        JointDrive drive = new JointDrive();
        drive.positionSpring = 200; // 손을 따라오는 힘(낮으면 더 부드럽게 끌림)
        drive.positionDamper = 50;
        drive.maximumForce = 1000;

        joint.xDrive = drive;
        joint.yDrive = drive;
        joint.zDrive = drive;
    }

    public void ReleaseRagdoll()
    {
        isGrabbing = false;

        // 최상위 부모 오브젝트의 Collider 다시 활성화
        if (rootColliders != null)
        {
            foreach (var col in rootColliders)
            {
                col.enabled = true;
            }
        }

        // Configurable Joint 삭제
        if (joint != null)
        {
            Destroy(joint);
            joint = null;
        }

        ragdollRigidbody = null;
        rootTransform = null; // 루트 Transform 초기화
    }

    // IK 적용
    private void OnAnimatorIK(int layerIndex)
    {
        if (anim)
        {
            if (isGrabbing)
            {
                // Lerp & Slerp를 이용한 부드러운 IK 이동
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