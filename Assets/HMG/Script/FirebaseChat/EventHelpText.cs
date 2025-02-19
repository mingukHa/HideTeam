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
        text.text = "û�Һθ� ���������� �����߾�";
    }
    private void StartPlayerEnterBank()
    {
        text.text = "������ �� �ݰ� �ִ�. VIP���� ���� ��ǰ�� Ż���ض�";
    }
    private void StartNPCKill()
    {
        text.text = "�̷� �ű⼭ ����� ���̸� ���?";
    }
    private void StartGarbage()
    {
        text.text = "�������� �ٰ����� �־�";
    }
    private void StartRichKill()
    {
        text.text = "�̷� ������ �Ҹ��� ��� �����־� �� ��ü�� ����";
    }
    private void StartRichToiletKill()
    {
        text.text = "������ ��Ű�� �ʾҾ�. ī���ͷ� �̵��غ���";
    }
    private void StartCarKick()
    {
        text.text = "�̷� �׷� ���� �߷� ���� ���? ���� �������� �� ����";
    }

}
