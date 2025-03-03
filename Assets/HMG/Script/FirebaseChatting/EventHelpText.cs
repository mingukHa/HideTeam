using System.Collections;
using TMPro;
using UnityEngine;
using static EventManager;

public class EventHelpText : MonoBehaviour
{//게임 도움말 텍스트
    public TextMeshProUGUI text;

    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.Garbage, StartGarbage);
        EventManager.Subscribe(GameEventType.RichKill, StartRichKill);
        EventManager.Subscribe(GameEventType.Carkick, StartCarKick);
        EventManager.Subscribe(GameEventType.NPCKill, StartNPCKill); 
        EventManager.Subscribe(GameEventType.PlayerEnterBank, StartPlayerEnterBank);
        EventManager.Subscribe(GameEventType.CleanManTalk, StartCleanManTalk);
        EventManager.Subscribe(GameEventType.OldManTalkTeller, OldTalkTeller);
        EventManager.Subscribe(GameEventType.CleanManDie, CleanManDie);
        EventManager.Subscribe(GameEventType.RichmanToliet, StartTolietRichman);
        EventManager.Subscribe(GameEventType.EndingStop, EndingStop);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEventType.Garbage, StartGarbage);
        EventManager.Unsubscribe(GameEventType.RichKill, StartRichKill);
        EventManager.Unsubscribe(GameEventType.Carkick, StartCarKick);
        EventManager.Unsubscribe(GameEventType.NPCKill, StartNPCKill);
        EventManager.Unsubscribe(GameEventType.PlayerEnterBank, StartPlayerEnterBank);
        EventManager.Unsubscribe(GameEventType.OldManTalkTeller, OldTalkTeller);
        EventManager.Unsubscribe(GameEventType.CleanManDie, CleanManDie);
        EventManager.Unsubscribe(GameEventType.RichmanToliet, StartTolietRichman);
        EventManager.Unsubscribe(GameEventType.EndingStop, EndingStop);
    }
    private void EndingStop()
    {
        ScreenshotManager.Instance.CaptureScreenshot();
        StartCoroutine(textNull("이런.. 경비원을 밖으로 보내는 방법을 찾아보자"));
    }
    private void CleanManDie()
    {
        ScreenshotManager.Instance.CaptureScreenshot();
        StartCoroutine(textNull("이런 거기서 사람을 죽이면 어떡해? 얼른 시체를 숨겨"));
    }
    private void OldTalkTeller()
    {
        ScreenshotManager.Instance.CaptureScreenshot();
        StartCoroutine(textNull("할아버지가 난동을 부리고 있다"));
    }
    private void StartCleanManTalk()
    {
        ScreenshotManager.Instance.CaptureScreenshot();
        StartCoroutine(textNull("청소부를 성공적으로 유인했어"));
    }
    private void StartPlayerEnterBank()
    {
        ScreenshotManager.Instance.CaptureScreenshot();
        StartCoroutine(textNull("오른쪽 방 금고가 있다. VIP보다 먼저 물품을 탈취해라"));
    }
    private void StartNPCKill()
    {
        ScreenshotManager.Instance.CaptureScreenshot();
        StartCoroutine(textNull("이런...시체를 숨기지 못해서 발각되었어"));
    }
    private void StartGarbage()
    {
        ScreenshotManager.Instance.CaptureScreenshot();
        StartCoroutine(textNull("누군가가 다가오고 있어"));
    }
    private void StartRichKill()
    {
        ScreenshotManager.Instance.CaptureScreenshot();
        StartCoroutine(textNull("이런 누군가 소리를 듣고 오고있어 얼른 시체를 숨겨"));
    }
    private void StartCarKick()
    {
        ScreenshotManager.Instance.CaptureScreenshot();
        StartCoroutine(textNull("이런 차를 발로 차면 어떻해? 누가 오기전에 얼른 숨어"));        
    }
    private void StartTolietRichman()
    {
        ScreenshotManager.Instance.CaptureScreenshot();
        StartCoroutine(textNull("화장실을 향하는군요. 왠지 따라가면 좋은 정보를 얻을 수 있을거 같아요."));
    }
    private IEnumerator textNull(string texts)
    {
        text.text = texts;
        yield return new WaitForSeconds(4f);
        text.text = "";
    }

}
