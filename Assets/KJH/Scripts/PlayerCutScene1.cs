using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayerCutScene1 : MonoBehaviour
{
    private PlayableDirector pd;
    public TimelineAsset[] ta;
    public GameObject cutscene1;

    private static bool hasPlayed = false; //씬이 리로드되도 유지됨.

    public void SetStarted()
    {
        hasPlayed = false;
    }

    private void Start()
    {
        pd = GetComponent<PlayableDirector>();

        if(hasPlayed)
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
            pd.Play(ta[0]);
        }
    }
}
