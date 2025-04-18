using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
public class Ending : MonoBehaviour
{
    [SerializeField] private Image fadeImage; // 검은색 UI 이미지
    public float FadeInTime = 2f;
    public TextMeshProUGUI text;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player")) //플레이어가 닿았는가
        {
            fadeImage.gameObject.SetActive(true);
            StartCoroutine(FadeIn(FadeInTime));
        }
        if (other.CompareTag("NPCEND")) //NPC가 닿았는가
        {
            StartCoroutine(GameOverCoroutine());
        }
    }



    public IEnumerator FadeIn(float duration)
    {
        fadeImage.gameObject.SetActive(true);
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            fadeImage.color = color;
            yield return null;
        }
        FindAnyObjectByType<ESCMenu>().Lobbygame();
    }
    public IEnumerator GameOverCoroutine() //NPC가 닿았을 경우
    {
        text.text = "아무래도 주요인물이 타겟과 접촉한거 같군. 미션 실패야 다시 시작해.";
        yield return new WaitForSeconds(4.0f);
        EventManager.Trigger(EventManager.GameEventType.GameOver);
    }


}
