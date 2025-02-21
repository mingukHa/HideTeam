using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private AudioSource audioSource; // ������ҽ� ������Ʈ
    private Animator animator;
    private float lastFootstepTime = 0f; // ������ �߼Ҹ� ��� �ð�
    private float footstepCooldown = 0.2f; // �߼Ҹ� �ּ� ��� ���� (0.2��)

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    public void Walk()
    {
        // ���� �ð��� ������ �߼Ҹ� ��� �ð� + ��Ÿ�Ӻ��� Ŀ�� ��
        if (Time.time < lastFootstepTime + footstepCooldown)
        {
            return; // ��Ÿ���� ������ ������ �ߺ� ��� ����
        }

        float forwardWeight = animator.GetFloat("Forward");
        float rightWeight = animator.GetFloat("Right");

        if (Mathf.Abs(forwardWeight) > 0.1f || Mathf.Abs(rightWeight) > 0.1f)
        {
            audioSource.Play(); // �߼Ҹ� ���
            lastFootstepTime = Time.time; // ������ ��� �ð� ������Ʈ
        }
    }

    //public void Gunshot()
    //{
    //    gunSound.Play();
    //}
}
