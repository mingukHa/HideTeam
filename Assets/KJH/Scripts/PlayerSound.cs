using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private AudioSource audioSource; // 오디오소스 컴포넌트
    private Animator animator;
    private float lastFootstepTime = 0f; // 마지막 발소리 재생 시간
    private float footstepCooldown = 0.2f; // 발소리 최소 재생 간격 (0.2초)

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    public void Walk()
    {
        // 현재 시간이 마지막 발소리 재생 시간 + 쿨타임보다 커야 함
        if (Time.time < lastFootstepTime + footstepCooldown)
        {
            return; // 쿨타임이 지나지 않으면 중복 재생 방지
        }

        float forwardWeight = animator.GetFloat("Forward");
        float rightWeight = animator.GetFloat("Right");

        if (Mathf.Abs(forwardWeight) > 0.1f || Mathf.Abs(rightWeight) > 0.1f)
        {
            audioSource.Play(); // 발소리 재생
            lastFootstepTime = Time.time; // 마지막 재생 시간 업데이트
        }
    }

    //public void Gunshot()
    //{
    //    gunSound.Play();
    //}
}
