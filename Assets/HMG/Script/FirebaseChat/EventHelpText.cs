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
        EventManager.Subscribe(GameEventType.OldManOut, OldmanOut);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEventType.Garbage, StartGarbage);
        EventManager.Unsubscribe(GameEventType.RichKill, StartRichKill);
        EventManager.Unsubscribe(GameEventType.RichToiletKill, StartRichToiletKill);
        EventManager.Unsubscribe(GameEventType.Carkick, StartCarKick);
        EventManager.Unsubscribe(GameEventType.NPCKill, StartNPCKill);
        EventManager.Unsubscribe(GameEventType.PlayerEnterBank, StartPlayerEnterBank);
        EventManager.Unsubscribe(GameEventType.OldManOut, OldmanOut);
    }
    private void OldmanOut()
    {
        
    }
    private void StartCleanManTalk()
    {
        text.text = "청소부를 성공적으로 유인했어";
    }
    private void StartPlayerEnterBank()
    {
        text.text = "오른쪽 방 금고가 있다. VIP보다 먼저 물품을 탈취해라";
    }
    private void StartNPCKill()
    {
        text.text = "이런 거기서 사람을 죽이면 어떻해?";
    }
    private void StartGarbage()
    {
        text.text = "누군가가 다가오고 있어";
    }
    private void StartRichKill()
    {
        text.text = "이런 누군가 소리를 듣고 오고있어 얼른 시체를 숨겨";
    }
    private void StartRichToiletKill()
    {
        text.text = "다행히 들키지 않았어. 카운터로 이동해보자";
    }
    private void StartCarKick()
    {
        text.text = "이런 그런 차를 발로 차면 어떻해? 누가 오기전에 얼른 숨어";
    }

}
