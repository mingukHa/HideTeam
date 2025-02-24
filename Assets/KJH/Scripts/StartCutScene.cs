using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartCutScene : MonoBehaviour
{
    public GameObject Cam1;
    public GameObject Cam2;
    public GameObject Cam3;
    public GameObject Cam4;
    public Image Fadeimage;

    public GameObject cutscene1;

    private static bool hasPlayed = false; //씬이 리로드되도 유지됨.

    public void cutSceneCamReset()
    {
        hasPlayed = false;
    }

    public void SetStarted()
    {
        hasPlayed = false;
    }

    private void Start()
    {
        if (hasPlayed)
        {
            //트리거 콜라이더를 비활성화해서 다시 실행되지 않게 함.
            cutscene1.GetComponent<Collider>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed) return; // 이미 실행된 경우 무시

        if (other.tag == "CutScene" && !hasPlayed) // 한 번만 실행
        {
            hasPlayed = true;
            StartCoroutine(StartSequence());
        }
    }

    private IEnumerator StartSequence()
    {
        Cam1.SetActive(true);
        Cam2.SetActive(false);
        Cam3.SetActive(false);
        Cam4.SetActive(false);
        Fadeimage.gameObject.SetActive(false);

        yield return new WaitForSeconds(2.3f);
        Cam1.SetActive(false);
        Cam2.SetActive(true);

        yield return new WaitForSeconds(7f);
        Cam2.SetActive(false);
        Cam3.SetActive(true);

        yield return new WaitForSeconds(3.5f);
        Cam3.SetActive(false);
        Cam4.SetActive(true);
        Fadeimage.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        Cam4.SetActive(false);

        yield return new WaitForSeconds(2.5f);
        Fadeimage.gameObject.SetActive(false);
    }
}
