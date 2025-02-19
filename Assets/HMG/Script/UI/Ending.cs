using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
public class Ending : MonoBehaviour
{
    [SerializeField] private Image fadeImage; // ������ UI �̹���
    public float FadeInTime = 2f;
    public TextMeshProUGUI text;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fadeImage.gameObject.SetActive(true);
            StartCoroutine(FadeIn(FadeInTime));
        }
        if(other.CompareTag("NPC"))
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
        SceneManager.LoadScene("LobbyScene");
    }
    public IEnumerator GameOverCoroutine()
    {
        text.text = "�ƹ����� �ֿ��ι��� Ÿ�ٰ� �����Ѱ� ����. �̼� ���о� �ٽ� ������.";
        yield return new WaitForSeconds(4.0f);
        EventManager.Trigger(EventManager.GameEventType.GameOver);
    }


}

