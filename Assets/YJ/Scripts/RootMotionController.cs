using UnityEngine;

public class RootMotionController : MonoBehaviour
{
    public CharacterController characterController;
    public Animator animator;
    private Vector3 velocity;
    public float gravity = 9.81f;

    void Update()
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
