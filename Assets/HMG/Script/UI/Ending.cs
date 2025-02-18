using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
public class Ending : MonoBehaviour
{
    [SerializeField] private Image fadeImage; // 검은색 UI 이미지
    public float FadeInTime = 2f;
    private void OnTriggerEnter(Collider other)
    {
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeIn(FadeInTime));
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
        SceneManager.LoadScene("LobbyScene");
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

