using UnityEngine;

public class PlayerWalkSound : MonoBehaviour
{
    public AudioSource walkSound;
    private Animator animator;
    private int lastPlayedFrame = -1; // 마지막으로 재생된 프레임을 저장


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Walk()
    {
        // 현재 프레임 확인
        int currentFrame = Time.frameCount;

        // 같은 프레임에서 여러 번 호출되었는지 확인
        if (currentFrame == lastPlayedFrame)
        {
            return; // 이미 이 프레임에서 소리를 재생했으면 중복 실행 방지
        }

        float forwardWeight = animator.GetFloat("Forward");
        float rightWeight = animator.GetFloat("Right");

        if (Mathf.Abs(forwardWeight) > 0.5f || Mathf.Abs(rightWeight) > 0.5f)
        {
            walkSound.Play(); // 발소리 재생
            lastPlayedFrame = currentFrame; // 마지막 재생된 프레임 업데이트
        }
    }
}
