using UnityEngine;

public class PlayerWalkSound : MonoBehaviour
{
    public AudioSource walkSound;
    private Animator animator;
    private int lastPlayedFrame = -1; // ���������� ����� �������� ����


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Walk()
    {
        // ���� ������ Ȯ��
        int currentFrame = Time.frameCount;

        // ���� �����ӿ��� ���� �� ȣ��Ǿ����� Ȯ��
        if (currentFrame == lastPlayedFrame)
        {
            return; // �̹� �� �����ӿ��� �Ҹ��� ��������� �ߺ� ���� ����
        }

        float forwardWeight = animator.GetFloat("Forward");
        float rightWeight = animator.GetFloat("Right");

        if (Mathf.Abs(forwardWeight) > 0.5f || Mathf.Abs(rightWeight) > 0.5f)
        {
            walkSound.Play(); // �߼Ҹ� ���
            lastPlayedFrame = currentFrame; // ������ ����� ������ ������Ʈ
        }
    }
}
