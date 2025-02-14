using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayerCutScene1 : MonoBehaviour
{
    private PlayableDirector pd;
    public TimelineAsset[] ta;

    private void Start()
    {
        pd = GetComponent<PlayableDirector>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CutScene")
        {
            pd.Play(ta[0]);
        }
    }
}
