using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static EventManager;

public class Progress : MonoBehaviour
{
    [SerializeField] private GameObject ProgressUI;

    [SerializeField] private TextMeshProUGUI RichmanText;
    [SerializeField] private TextMeshProUGUI OldManText;
    [SerializeField] private TextMeshProUGUI CleanManText;
    [SerializeField] private TextMeshProUGUI DoorGaurdText;
    [SerializeField] private TextMeshProUGUI RightRoomGaurdText;
    [SerializeField] private TextMeshProUGUI LobbyGaurdText;
    [SerializeField] private TextMeshProUGUI TellerText;
    [SerializeField] private TextMeshProUGUI TrashBinText;
    [SerializeField] private TextMeshProUGUI CarText;

    private void OnEnable()
    {
        // OldMan이벤트
        EventManager.Subscribe(GameEventType.OldManGotoTeller, () => OldManProgress(GameEventType.OldManGotoTeller));
        EventManager.Subscribe(GameEventType.OldManHelp, () => OldManProgress(GameEventType.OldManHelp));
        EventManager.Subscribe(GameEventType.OldManoutside, () => OldManProgress(GameEventType.OldManoutside));
        EventManager.Subscribe(GameEventType.OldManTalkTeller, () => OldManProgress(GameEventType.OldManTalkTeller));
        EventManager.Subscribe(GameEventType.RichToiletKill, () => OldManProgress(GameEventType.RichToiletKill));
        EventManager.Subscribe(GameEventType.RichKill, () => OldManProgress(GameEventType.RichKill));
        EventManager.Subscribe(GameEventType.OldManOut, () => OldManProgress(GameEventType.OldManOut));
        EventManager.Subscribe(GameEventType.OldManMovingCounter, () => OldManProgress(GameEventType.OldManMovingCounter));
        EventManager.Subscribe(GameEventType.GameOver, () => OldManProgress(GameEventType.GameOver));

        // RichMan 이벤트
        EventManager.Subscribe(GameEventType.PlayerEnterBank, () => RichManProgress(GameEventType.PlayerEnterBank));
        EventManager.Subscribe(GameEventType.TellerTalk, () => RichManProgress(GameEventType.TellerTalk));
        EventManager.Subscribe(GameEventType.RichmanTalkTeller, () => RichManProgress(GameEventType.RichmanTalkTeller));
        EventManager.Subscribe(GameEventType.OldManTalkTeller, () => RichManProgress(GameEventType.OldManTalkTeller));
        EventManager.Subscribe(GameEventType.RichKill, () => RichManProgress(GameEventType.RichKill));
        EventManager.Subscribe(GameEventType.RichToiletKill, () => RichManProgress(GameEventType.RichToiletKill));
        EventManager.Subscribe(GameEventType.RichHide, () => RichManProgress(GameEventType.RichHide));
        EventManager.Subscribe(GameEventType.GameOver, () => RichManProgress(GameEventType.GameOver));

        // CleanMan 이벤트
        EventManager.Subscribe(GameEventType.Garbage, () => CleanManProgress(GameEventType.Garbage));
        EventManager.Subscribe(GameEventType.CleanManDie, () => CleanManProgress(GameEventType.CleanManDie));
        EventManager.Subscribe(GameEventType.CleanManTalk, () => CleanManProgress(GameEventType.CleanManTalk));
        EventManager.Subscribe(GameEventType.RichKill, () => CleanManProgress(GameEventType.RichKill));
        EventManager.Subscribe(GameEventType.PlayerToiletOut, () => CleanManProgress(GameEventType.RichHide));
        EventManager.Subscribe(GameEventType.OldManOut, () => CleanManProgress(GameEventType.OldManOut));
        EventManager.Subscribe(GameEventType.GameOver, () => CleanManProgress(GameEventType.GameOver));
    }
    //람다식으로 넣어줘야 이벤트 타입을 switch문에 넣기 가능
    private void OnDisable()
    {
        // OldMan이벤트
        EventManager.Unsubscribe(GameEventType.OldManGotoTeller, () => OldManProgress(GameEventType.OldManGotoTeller));
        EventManager.Unsubscribe(GameEventType.OldManHelp, () => OldManProgress(GameEventType.OldManHelp));
        EventManager.Unsubscribe(GameEventType.OldManoutside, () => OldManProgress(GameEventType.OldManoutside));
        EventManager.Unsubscribe(GameEventType.OldManTalkTeller, () => OldManProgress(GameEventType.OldManTalkTeller));
        EventManager.Unsubscribe(GameEventType.RichToiletKill, () => OldManProgress(GameEventType.RichToiletKill));
        EventManager.Unsubscribe(GameEventType.RichKill, () => OldManProgress(GameEventType.RichKill));
        EventManager.Unsubscribe(GameEventType.OldManOut, () => OldManProgress(GameEventType.OldManOut));
        EventManager.Unsubscribe(GameEventType.OldManMovingCounter, () => OldManProgress(GameEventType.OldManMovingCounter));
        EventManager.Unsubscribe(GameEventType.GameOver, () => OldManProgress(GameEventType.GameOver));

        // RichMan 이벤트
        EventManager.Unsubscribe(GameEventType.PlayerEnterBank, () => RichManProgress(GameEventType.PlayerEnterBank));
        EventManager.Unsubscribe(GameEventType.TellerTalk, () => RichManProgress(GameEventType.TellerTalk));
        EventManager.Unsubscribe(GameEventType.RichmanTalkTeller, () => RichManProgress(GameEventType.RichmanTalkTeller));
        EventManager.Unsubscribe(GameEventType.OldManTalkTeller, () => RichManProgress(GameEventType.OldManTalkTeller));
        EventManager.Unsubscribe(GameEventType.RichKill, () => RichManProgress(GameEventType.RichKill));
        EventManager.Unsubscribe(GameEventType.RichToiletKill, () => RichManProgress(GameEventType.RichToiletKill));
        EventManager.Unsubscribe(GameEventType.RichHide, () => RichManProgress(GameEventType.RichHide));
        EventManager.Unsubscribe(GameEventType.GameOver, () => RichManProgress(GameEventType.GameOver));

        // CleanMan 이벤트
        EventManager.Unsubscribe(GameEventType.Garbage, () => CleanManProgress(GameEventType.Garbage));
        EventManager.Unsubscribe(GameEventType.CleanManDie, () => CleanManProgress(GameEventType.CleanManDie));
        EventManager.Unsubscribe(GameEventType.CleanManTalk, () => CleanManProgress(GameEventType.CleanManTalk));
        EventManager.Unsubscribe(GameEventType.RichKill, () => CleanManProgress(GameEventType.RichKill));
        EventManager.Unsubscribe(GameEventType.PlayerToiletOut, () => CleanManProgress(GameEventType.RichHide));
        EventManager.Unsubscribe(GameEventType.OldManOut, () => CleanManProgress(GameEventType.OldManOut));
        EventManager.Unsubscribe(GameEventType.GameOver, () => CleanManProgress(GameEventType.GameOver));
    }
    private void Update()
    {
        ProgressUIOnOff();
    }
    private void ProgressUIOnOff()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            ProgressUI.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            ProgressUI.SetActive(false);
        }
    }
    private void OldManProgress(GameEventType eventType)
    {
        switch (eventType)
        {
            case GameEventType.OldManGotoTeller:
                OldManText.text = "Counter Moving Possible";
                break;
            case GameEventType.OldManTalkTeller:
                OldManText.text = "Angry State";
                break;
            case GameEventType.OldManHelp:
            case GameEventType.OldManoutside:
            case GameEventType.RichToiletKill:
            case GameEventType.RichKill:
            case GameEventType.OldManOut:
                OldManText.text = "Go Out State";
                break;
            case GameEventType.OldManMovingCounter:
                OldManText.text = "Go to Counter State";
                break;
            case GameEventType.GameOver:
                OldManText.text = "To Reset...";
                break;

        }
    }
    private void RichManProgress(GameEventType eventType)
    {
        switch (eventType)
        {
            case GameEventType.PlayerEnterBank:
                RichmanText.text = "Routine State";
                break;
            case GameEventType.TellerTalk:
                RichmanText.text = "Smoking State";
                break;
            case GameEventType.RichmanTalkTeller:
                RichmanText.text = "Gone State";
                break;
            case GameEventType.OldManTalkTeller:
                RichmanText.text = "Calling State";
                break;
            case GameEventType.RichKill:
            case GameEventType.RichToiletKill:
                RichmanText.text = "Dead State";
                break;
            case GameEventType.RichHide:
                RichmanText.text = "NONE";
                break;
            case GameEventType.GameOver:
                OldManText.text = "To Reset...";
                break;
        }
    }
    private void CleanManProgress(GameEventType eventType)
    {
        switch (eventType)
        {
            case GameEventType.Garbage:
                CleanManText.text = "Chase State";
                break;
            case GameEventType.CleanManDie:
                CleanManText.text = "Dead State";
                break;
            case GameEventType.CleanManTalk:
                CleanManText.text = "Mop State";
                break;
            case GameEventType.RichKill:
                CleanManText.text = "Chase Sound State";
                break;
            case GameEventType.PlayerToiletOut:
                CleanManText.text = "Check State";
                break;
            case GameEventType.OldManOut:
                CleanManText.text = "Mop State";
                break;
            case GameEventType.GameOver:
                CleanManText.text = "To Reset...";
                break;
        }
    }
}
