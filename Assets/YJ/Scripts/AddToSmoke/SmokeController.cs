using System.Collections;
using UnityEngine;

public class SmokeController : MonoBehaviour
{
    private static bool hasPlayed = false; // 씬이 바뀌어도 유지되는 변수

    void Start()
    {
        if (hasPlayed)
        {
            gameObject.SetActive(false); // 이미 실행되었다면 비활성화
            return;
        }

        StartCoroutine(PlayParticleOnce());
    }

    private IEnumerator PlayParticleOnce()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ps.Play();
        yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);

        hasPlayed = true; // 실행 상태 저장
    }
}

