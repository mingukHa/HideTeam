using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class FadeOut : MonoBehaviour
{
    [SerializeField]private Image fadeImage; // 검은색 UI 이미지

    private void Start()
    {
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeIn(1f)); // 씬 시작 시 페이드 인 (1초)
    }

    public IEnumerator FadeIn(float duration)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            fadeImage.color = color;
            yield return null;
        }
        fadeImage.gameObject.SetActive(false);
    }

    public IEnumerator Fadeout(float duration)
    {
        fadeImage.gameObject.SetActive(true);
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            fadeImage.color = color;
            yield return null;
        }
    }
}
