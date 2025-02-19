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
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEventType.Garbage, StartGarbage);
        EventManager.Unsubscribe(GameEventType.RichKill, StartRichKill);
        EventManager.Unsubscribe(GameEventType.RichToiletKill, StartRichToiletKill);
        EventManager.Unsubscribe(GameEventType.Carkick, StartCarKick);
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
