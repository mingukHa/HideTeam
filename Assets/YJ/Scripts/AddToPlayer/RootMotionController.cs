using UnityEngine;

public class RootMotionController : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;
    private Vector3 velocity;
    [SerializeField]
    private float gravity = 9.8f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 rootMotion = animator.deltaPosition; // 애니메이션의 Root Motion 이동 값
        rootMotion.y = 0; // 수직 이동을 제거 (수동으로 중력 적용)
        rootMotion *= 0.1f; // 속도 10%로 제한

        if (!characterController.isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime; // 중력 적용
        }
        else
        {
            velocity.y = -0.1f; // 지면에 붙어 있도록 작은 값 설정
        }

        characterController.Move(rootMotion + velocity * Time.deltaTime);
    }
}
