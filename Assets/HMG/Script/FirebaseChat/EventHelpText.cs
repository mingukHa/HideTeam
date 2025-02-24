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
        EventManager.Subscribe(EventManager.GameEventType.RichmanToliet, StartTolietRichman);
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
        EventManager.Unsubscribe(EventManager.GameEventType.RichmanToliet, StartTolietRichman);
    }
    private void CleanManDie()
    {
        ScreenshotManager.Instance.CaptureScreenshot();
        StartCoroutine(textNull("�̷� �ű⼭ ����� ���̸� ���? �� ��ü�� ����"));
    }
    private void OldTalkTeller()
    {
        ScreenshotManager.Instance.CaptureScreenshot();
        StartCoroutine(textNull("�Ҿƹ����� ������ �θ��� �ִ�"));
    }
    private void StartCleanManTalk()
    {
        ScreenshotManager.Instance.CaptureScreenshot();
        StartCoroutine(textNull("û�Һθ� ���������� �����߾�"));
    }
    private void StartPlayerEnterBank()
    {
        ScreenshotManager.Instance.CaptureScreenshot();
        StartCoroutine(textNull("������ �� �ݰ� �ִ�. VIP���� ���� ��ǰ�� Ż���ض�"));
    }
    private void StartNPCKill()
    {
        ScreenshotManager.Instance.CaptureScreenshot();
        StartCoroutine(textNull("�̷�...��ü�� ������ ���ؼ� �߰��Ǿ���"));
    }
    private void StartGarbage()
    {
        ScreenshotManager.Instance.CaptureScreenshot();
        StartCoroutine(textNull("�������� �ٰ����� �־�"));
    }
    private void StartRichKill()
    {
        ScreenshotManager.Instance.CaptureScreenshot();
        StartCoroutine(textNull("�̷� ������ �Ҹ��� ��� �����־� �� ��ü�� ����"));
    }
    private void StartRichToiletKill()
    {
        ScreenshotManager.Instance.CaptureScreenshot();
        StartCoroutine(textNull("������ ��Ű�� �ʾҾ�. ī���ͷ� �̵��غ���"));
    }
    private void StartCarKick()
    {
        ScreenshotManager.Instance.CaptureScreenshot();
        StartCoroutine(textNull("�̷� ���� �߷� ���� ���? ���� �������� �� ����"));        
    }
    private void StartTolietRichman()
    {
        ScreenshotManager.Instance.CaptureScreenshot();
        StartCoroutine(textNull("ȭ����� ���ϴ±���. ���� ���󰡸� ���� ������ ���� �� ������ ���ƿ�."));
    }
    private IEnumerator textNull(string texts)
    {
        text.text = texts;
        yield return new WaitForSeconds(3f);
        text.text = "";
    }

}
