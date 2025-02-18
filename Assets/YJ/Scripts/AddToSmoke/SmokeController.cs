using System.Collections;
using UnityEngine;

public class SmokeController : MonoBehaviour
{
    private static bool hasPlayed = false; // ���� �ٲ� �����Ǵ� ����

    void Start()
    {
        if (hasPlayed)
        {
            gameObject.SetActive(false); // �̹� ����Ǿ��ٸ� ��Ȱ��ȭ
            return;
        }

        StartCoroutine(PlayParticleOnce());
    }

    private IEnumerator PlayParticleOnce()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ps.Play();
        yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);

        hasPlayed = true; // ���� ���� ����
    }
}

