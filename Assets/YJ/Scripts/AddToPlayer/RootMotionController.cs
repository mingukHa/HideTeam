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
        Vector3 rootMotion = animator.deltaPosition; // �ִϸ��̼��� Root Motion �̵� ��
        rootMotion.y = 0; // ���� �̵��� ���� (�������� �߷� ����)
        rootMotion *= 0.1f; // �ӵ� 10%�� ����

        if (!characterController.isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime; // �߷� ����
        }
        else
        {
            velocity.y = -0.1f; // ���鿡 �پ� �ֵ��� ���� �� ����
        }

        characterController.Move(rootMotion + velocity * Time.deltaTime);
    }
}
