using System.Collections.Generic;
using UnityEngine;

public class RagdollGrabber : MonoBehaviour
{
    public Transform ragdollHoldPoint; // Grab 한 Ragdoll 부착 부위
    public Transform leftHandIKTarget; // Grab 한 뒤 왼손이 위치할 지점
    public Transform rightHandIKTarget; // Grab 한 뒤 오른손이 위치할 지점
    private float ikLerpSpeed = 10f; // 손이 선형보간을 이용해 이동할 때 움직이는 속도

    private Animator anim;
    private ConfigurableJoint joint;
    [HideInInspector]
    public Rigidbody ragdollRigidbody; // Grab 한 Ragdoll 부위
    private Transform rootTransform; // Ragdoll의 Root(최상위) Object Transform
    private Collider[] rootColliders; // Ragdoll에 부착된 모든 Collider의 배열
    [HideInInspector]
    public bool isGrabbing = false; // Ragdoll을 Grab 중인지 판정

    private Vector3 leftHandIKPosition; // Grab 전 왼손의 Position
    private Quaternion leftHandIKRotation; // Grab 전 왼손의 Rotation
    private Vector3 rightHandIKPosition; // Grab 전 오른손의 Position
    private Quaternion rightHandIKRotation; // Grab 전 오른손의 Rotation

    private Rigidbody rootRigidbody; // Ragdoll의 Root Object에 추가되는 Rigidbody
    private ConfigurableJoint rootJoint; // Ragdoll의 Root Object에 추가되는 Configurable Joint
    private float groundLevel; // 지면의 위치

    private Dictionary<Collider, bool> originalTriggerStates =
        new Dictionary<Collider, bool>(); // Ragdoll Root Object Collider들의 초기 상태 저장(isTrigger 여부)

    private void Start()
    {
        anim = GetComponent<Animator>();

        // 플레이어의 왼손, 오른손 초기 위치 및 회전값 저장
        leftHandIKPosition = anim.GetIKPosition(AvatarIKGoal.LeftHand);
        leftHandIKRotation = anim.GetIKRotation(AvatarIKGoal.LeftHand);
        rightHandIKPosition = anim.GetIKPosition(AvatarIKGoal.RightHand);
        rightHandIKRotation = anim.GetIKRotation(AvatarIKGoal.RightHand);

        // Raycast로 지면의 위치를 받아옴
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
            if (!isGrabbing) // Ragdoll을 붙잡고 있지 않을 때 붙잡으려고 시도함
            {
                anim.SetBool("isGrabbingRagdoll", true); // Animator의 Grabbing Ragdoll Blend Tree로 전환
                TryGrabRagdoll();
            }
            else // Ragdoll을 붙잡고 있을 때 해제함
            {
                anim.SetBool("isGrabbingRagdoll", false); // Idle 상태로 다시 전환
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

        // Ragdoll이 지면 아래로 파고들지 않도록 함
        if (ragdollRigidbody != null && ragdollRigidbody.transform.position.y < groundLevel) // Ragdoll의 Y값이 지면보다 아래에 있을 경우
        {
            Vector3 fixedPosition = ragdollRigidbody.transform.position;
            fixedPosition.y = groundLevel + 0.1f;  // Ragdoll의 Y값을 바닥보다 살짝 위로 보정
            ragdollRigidbody.MovePosition(fixedPosition); // Ragdoll을 보정한 위치로 이동
        }
    }

    public bool IsHoldingRagdoll(Transform ragdoll)
    {
        // Ragdoll을 Grab 중인지 Ragdoll Disposer가 참조할 값
        return isGrabbing && rootTransform == ragdoll;
    }

    private void TryGrabRagdoll()
    {
        // 반경 1f 안의 Collider를 탐색하여 배열로 받음
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (var col in colliders)
        {
            // 해당 배열에서 Ragdoll 태그가 붙은 Rigidbody가 있을 경우
            if (col.attachedRigidbody != null && col.attachedRigidbody.CompareTag("Ragdoll"))
            {
                ragdollRigidbody = col.attachedRigidbody; // 그 중 첫 번째를 ragdollRigidbody로 설정
                AttachRagdoll(); // Player에게 부착하기 위한 AttachRagdoll 메소드 실행
                return;
            }
        }

        // Collider 배열에 Ragdoll 태그가 없을 경우
        isGrabbing = false;
        anim.SetBool("isGrabbingRagdoll", false);
    }

    private void AttachRagdoll()
    {
        isGrabbing = true;
        rootTransform = ragdollRigidbody.transform.root; // Grab 한 Ragdoll의 Root Object Transform 받음
        rootColliders = rootTransform.GetComponentsInChildren<Collider>(); // Grab 한 Ragdoll의 Root Object 이하 모든 Collider들 가져옴
        originalTriggerStates.Clear(); // Collider들 초기 설정값 초기화

        // Root Object 이하 모든 Rigidbody 가져옴
        Rigidbody[] allRigidbodies = rootTransform.GetComponentsInChildren<Rigidbody>();

        // Ragdoll과 다른 Object 충돌 방지를 위한 Collider 설정
        foreach (var col in rootColliders)
        {
            if (col.attachedRigidbody == ragdollRigidbody) // Grab한 Ragdoll 부위에 부착된 Collider는 제외한 다음
                continue;

            originalTriggerStates[col] = col.isTrigger; // 나머지 Collider의 isTrigger 상태 저장
            col.isTrigger = true; // 예외처리 한 부위를 제외하고 모든 Collider의 isTrigger 활성화
        }

        // Ragdoll과 다른 Object 충돌 방지를 위한 Rigidbody 설정
        foreach (var rb in allRigidbodies)
        {
            rb.useGravity = false; // 중력 해제
            rb.interpolation = RigidbodyInterpolation.Interpolate; // Interpolate 설정하여 물리 효과를 부드럽게 함
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // 충돌 감지를 Continuous로 설정
        }

        // Ragdoll Root Object에 Rigidbody 추가
        if (!rootTransform.TryGetComponent(out rootRigidbody))
        {
            rootRigidbody = rootTransform.gameObject.AddComponent<Rigidbody>();
            rootRigidbody.mass = 10f; // 무게 10으로 설정
        }

        // Root Object Rigidbody 설정
        rootRigidbody.isKinematic = false; // 끄는 방향으로 움직일 수 있도록 isKinematic 켬
        rootRigidbody.useGravity = false; // 중력 해제
        rootRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        rootRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // Root Object와 Ragdoll을 연결하는 Configurable Joint 부착
        joint = ragdollRigidbody.gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = rootRigidbody; // 추가한 Configurable Joint를 Root Object와 연결

        // Configurable Joint 움직임 제한
        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;
        joint.angularXMotion = ConfigurableJointMotion.Limited;
        joint.angularYMotion = ConfigurableJointMotion.Limited;
        joint.angularZMotion = ConfigurableJointMotion.Limited;

        // Joint의 연결 강도 설정
        JointDrive drive = new JointDrive
        {
            positionSpring = 100f, // Joint의 위치를 복원하려는 Spring 강도
            positionDamper = 10f, // 움직임에 대한 Damper(저항) 강도
            maximumForce = 1000f // Joint가 허용하는 최대 힘
        };
        
        // 모든 축에 동일한 설정 적용
        joint.xDrive = drive;
        joint.yDrive = drive;
        joint.zDrive = drive;

        // Root Object와 HoldPoint를 연결하는 Configurable Joint 부착
        rootJoint = rootTransform.gameObject.AddComponent<ConfigurableJoint>();
        rootJoint.connectedBody = ragdollHoldPoint.GetComponent<Rigidbody>(); // 추가한 Configurable Joint를 HoldPoint와 연결

        // Configurable Joint 움직임 제한
        rootJoint.xMotion = ConfigurableJointMotion.Limited;
        rootJoint.yMotion = ConfigurableJointMotion.Limited;
        rootJoint.zMotion = ConfigurableJointMotion.Limited;
        rootJoint.angularXMotion = ConfigurableJointMotion.Limited;
        rootJoint.angularYMotion = ConfigurableJointMotion.Limited;
        rootJoint.angularZMotion = ConfigurableJointMotion.Limited;
        rootJoint.breakForce = Mathf.Infinity;

        // Root Object와 Ragdoll의 위치 HoldPoint와 동기화
        rootRigidbody.MovePosition(ragdollHoldPoint.position); // HoldPoint로 Root Object 이동
        ragdollRigidbody.MovePosition(ragdollHoldPoint.position); // HoldPoint로 Ragdoll 이동
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

        // 저장된 Collider들의 isTrigger 설정값 복원
        foreach (var entry in originalTriggerStates)
        {
            if (entry.Key != null) // Collider가 존재하는 경우만 복원
            {
                entry.Key.isTrigger = entry.Value;
            }
        }

        // 복원 후 리스트 초기화
        originalTriggerStates.Clear();

        // Grab한 Ragdoll 부위에 부착된 Configurable Joint 제거
        if (joint != null)
        {
            Destroy(joint);
            joint = null;
        }

        // Root Object에 부착된 Configurable Joint 제거
        if (rootJoint != null)
        {
            Destroy(rootJoint);
            rootJoint = null;
        }

        // Root Object에 부착된 Rigidbody 제거
        if (rootRigidbody != null)
        {
            Destroy(rootRigidbody);
            rootRigidbody = null;
        }

        // Grab한 Ragdoll 부위 및 Root Object Transform 초기화
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
