using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayerCutScene1 : MonoBehaviour
{
    private PlayableDirector pd;
    public TimelineAsset[] ta;

    private static bool hasPlayed = false; //���� ���ε�ǵ� ������.

    private void Start()
    {
        pd = GetComponent<PlayableDirector>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CutScene" && !hasPlayed) // �� ���� ����
        {
            hasPlayed = true;
            pd.Play(ta[0]);
        }
    }
}
