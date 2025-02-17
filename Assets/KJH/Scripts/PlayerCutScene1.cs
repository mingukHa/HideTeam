using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayerCutScene1 : MonoBehaviour
{
    private PlayableDirector pd;
    public TimelineAsset[] ta;

    private static bool hasPlayed = false; //씬이 리로드되도 유지됨.

    private void Start()
    {
        pd = GetComponent<PlayableDirector>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CutScene" && !hasPlayed) // 한 번만 실행
        {
            hasPlayed = true;
            pd.Play(ta[0]);
        }
    }
}
