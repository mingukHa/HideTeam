using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static EventManager;

public class EndCutScene : MonoBehaviour
{
    public GameObject Cam5;
    public GameObject Door;
    public Image Fadeimage2;

    public GameObject cutscene2;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("¿£µù¾À ¹ßµ¿");
        if (other.CompareTag("EndCutScene"))
        {
            EventManager.Trigger(GameEventType.TellerTalk);
            StartCoroutine(EndSequence());
            EventManager.Trigger(EventManager.GameEventType.Ending);
            Debug.Log("¿£µù¾À ¹ßµ¿");
        }
    }

    private IEnumerator EndSequence()
    {
        Cam5.SetActive(false);
        Door.SetActive(false);

        yield return new WaitForSeconds(0.01f);
        Cam5.SetActive(true);
        Door.SetActive(true);

        yield return new WaitForSeconds(5f);
        Fadeimage2.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        Door.SetActive(false);
        Cam5.SetActive(false);
        SceneManager.LoadScene("LobbyScene");
    }
}