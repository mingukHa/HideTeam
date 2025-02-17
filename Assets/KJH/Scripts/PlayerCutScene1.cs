using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayerCutScene1 : MonoBehaviour
{
    private PlayableDirector pd;
    public TimelineAsset[] ta;
    public GameObject cutscene1;

    private static bool hasPlayed = false; //���� ���ε�ǵ� ������.

    private void Start()
    {
        pd = GetComponent<PlayableDirector>();

        if(hasPlayed)
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
            pd.Play(ta[0]);
        }
    }
}
