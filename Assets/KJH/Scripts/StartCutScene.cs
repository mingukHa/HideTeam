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

    private static bool hasPlayed = false; //���� ���ε�ǵ� ������.

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
            //Ʈ���� �ݶ��̴��� ��Ȱ��ȭ�ؼ� �ٽ� ������� �ʰ� ��.
            cutscene1.GetComponent<Collider>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed) return; // �̹� ����� ��� ����

        if (other.tag == "CutScene" && !hasPlayed) // �� ���� ����
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
