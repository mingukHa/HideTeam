using System.Collections;
using TMPro;
using UnityEngine;
using static EventManager;

public class EventHelpText : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.Garbage, StartGarbage);
        EventManager.Subscribe(GameEventType.RichKill, StartRichKill);
        EventManager.Subscribe(GameEventType.RichToiletKill, StartRichToiletKill);
        EventManager.Subscribe(GameEventType.Carkick, StartCarKick);
        EventManager.Subscribe(GameEventType.NPCKill, StartNPCKill); 
        EventManager.Subscribe(GameEventType.PlayerEnterBank, StartPlayerEnterBank);
        EventManager.Subscribe(GameEventType.CleanManTalk, StartCleanManTalk);
        EventManager.Subscribe(GameEventType.OldManTalkTeller, OldTalkTeller);
        EventManager.Subscribe(GameEventType.CleanManDie, CleanManDie);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEventType.Garbage, StartGarbage);
        EventManager.Unsubscribe(GameEventType.RichKill, StartRichKill);
        EventManager.Unsubscribe(GameEventType.RichToiletKill, StartRichToiletKill);
        EventManager.Unsubscribe(GameEventType.Carkick, StartCarKick);
        EventManager.Unsubscribe(GameEventType.NPCKill, StartNPCKill);
        EventManager.Unsubscribe(GameEventType.PlayerEnterBank, StartPlayerEnterBank);
        EventManager.Unsubscribe(GameEventType.OldManTalkTeller, OldTalkTeller);
        EventManager.Unsubscribe(GameEventType.CleanManDie, CleanManDie);
    }
    private void CleanManDie()
    {
        text.text = "이런 거기서 사람을 죽이면 어떻해? 얼른 시체를 숨겨";
    }
    private void OldTalkTeller()
    {
        StartCoroutine(textNull("할아버지가 난동을 부리고 있다"));
    }
    private void StartCleanManTalk()
    {
        StartCoroutine(textNull("청소부를 성공적으로 유인했어"));
    }
    private void StartPlayerEnterBank()
    {
        StartCoroutine(textNull("오른쪽 방 금고가 있다. VIP보다 먼저 물품을 탈취해라"));
    }
    private void StartNPCKill()
    {
        StartCoroutine(textNull("이런 거기서 사람을 죽이면 어떻해? 얼른 시체를 숨겨"));
    }
    private void StartGarbage()
    {
        StartCoroutine(textNull("누군가가 다가오고 있어"));
    }
    private void StartRichKill()
    {
        StartCoroutine(textNull("이런 누군가 소리를 듣고 오고있어 얼른 시체를 숨겨"));
    }
    private void StartRichToiletKill()
    {
        StartCoroutine(textNull("다행히 들키지 않았어. 카운터로 이동해보자"));
    }
    private void StartCarKick()
    {
        StartCoroutine(textNull("이런 차를 발로 차면 어떻해? 누가 오기전에 얼른 숨어"));        
    }
    private IEnumerator textNull(string texts)
    {
        text.text = texts;
        yield return new WaitForSeconds(3f);
        text.text = "";
    }

}
