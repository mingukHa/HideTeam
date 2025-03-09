using System.Collections.Generic;
using TMPro;
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

    private bool isCleanDie = false;

    private List<TextMeshProUGUI> TextList = new List<TextMeshProUGUI>();

    private void Start()
    {
        TextList.Clear();
        TextList.Add(RichmanText);
        TextList.Add(OldManText);
        TextList.Add(CleanManText);
        TextList.Add(DoorGaurdText);
        TextList.Add(RightRoomGaurdText);
        TextList.Add(LobbyGaurdText);
        TextList.Add(TellerText);
        TextList.Add(TrashBinText);
        TextList.Add(CarText);
    }
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

        // RichMan 이벤트
        EventManager.Subscribe(GameEventType.PlayerEnterBank, () => RichManProgress(GameEventType.PlayerEnterBank));
        EventManager.Subscribe(GameEventType.TellerTalk, () => RichManProgress(GameEventType.TellerTalk));
        EventManager.Subscribe(GameEventType.RichmanTalkTeller, () => RichManProgress(GameEventType.RichmanTalkTeller));
        EventManager.Subscribe(GameEventType.OldManTalkTeller, () => RichManProgress(GameEventType.OldManTalkTeller));
        EventManager.Subscribe(GameEventType.RichKill, () => RichManProgress(GameEventType.RichKill));
        EventManager.Subscribe(GameEventType.RichToiletKill, () => RichManProgress(GameEventType.RichToiletKill));
        EventManager.Subscribe(GameEventType.RichHide, () => RichManProgress(GameEventType.RichHide));

        // CleanMan 이벤트
        EventManager.Subscribe(GameEventType.Garbage, () => CleanManProgress(GameEventType.Garbage));
        EventManager.Subscribe(GameEventType.CleanManDie, () => CleanManProgress(GameEventType.CleanManDie));
        EventManager.Subscribe(GameEventType.CleanManTalk, () => CleanManProgress(GameEventType.CleanManTalk));
        EventManager.Subscribe(GameEventType.RichKill, () => CleanManProgress(GameEventType.RichKill));
        EventManager.Subscribe(GameEventType.PlayerToiletOut, () => CleanManProgress(GameEventType.RichHide));
        EventManager.Subscribe(GameEventType.OldManOut, () => CleanManProgress(GameEventType.OldManOut));
        EventManager.Subscribe(GameEventType.CleanManHide, () => CleanManProgress(GameEventType.CleanManHide));

        // DoorGuard 이벤트
        EventManager.Subscribe(GameEventType.PlayerEnterBank, () => DoorGuardProgress(GameEventType.PlayerEnterBank));
        EventManager.Subscribe(GameEventType.GameClear, () => DoorGuardProgress(GameEventType.GameClear));

        // RightRoomGaurd 이벤트
        EventManager.Subscribe(GameEventType.PlayerEnterBank, () => RightRoomGaurdProgress(GameEventType.PlayerEnterBank));
        EventManager.Subscribe(GameEventType.Carkick, () => RightRoomGaurdProgress(GameEventType.Carkick));

        // LobbyGaurd 이벤트
        EventManager.Subscribe(GameEventType.PlayerEnterBank, () => LobbyGaurdProgress(GameEventType.PlayerEnterBank));
        EventManager.Subscribe(GameEventType.Carkick, () => LobbyGaurdProgress(GameEventType.Carkick));

        // RichMan 이벤트
        EventManager.Subscribe(GameEventType.PlayerEnterBank, () => TellerProgress(GameEventType.PlayerEnterBank));
        EventManager.Subscribe(GameEventType.TellerTalk, () => TellerProgress(GameEventType.TellerTalk));
        EventManager.Subscribe(GameEventType.RichmanTalkTeller, () => TellerProgress(GameEventType.RichmanTalkTeller));
        EventManager.Subscribe(GameEventType.OldManTalkTeller, () => TellerProgress(GameEventType.OldManTalkTeller));
        EventManager.Subscribe(GameEventType.RichToiletKill, () => TellerProgress(GameEventType.RichToiletKill));
        EventManager.Subscribe(GameEventType.GameClear, () => TellerProgress(GameEventType.GameClear));

        // TrashBin 이벤트
        EventManager.Subscribe(GameEventType.Garbage, () => TrashBinProgress(GameEventType.Garbage));

        // Car 이벤트
        EventManager.Subscribe(GameEventType.Carkick, () => CarProgress(GameEventType.Carkick));

        // GameOver 이벤트
        EventManager.Subscribe(GameEventType.GameOver, () => GameOverProgress(GameEventType.GameOver));
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

        // RichMan 이벤트
        EventManager.Unsubscribe(GameEventType.PlayerEnterBank, () => RichManProgress(GameEventType.PlayerEnterBank));
        EventManager.Unsubscribe(GameEventType.TellerTalk, () => RichManProgress(GameEventType.TellerTalk));
        EventManager.Unsubscribe(GameEventType.RichmanTalkTeller, () => RichManProgress(GameEventType.RichmanTalkTeller));
        EventManager.Unsubscribe(GameEventType.OldManTalkTeller, () => RichManProgress(GameEventType.OldManTalkTeller));
        EventManager.Unsubscribe(GameEventType.RichKill, () => RichManProgress(GameEventType.RichKill));
        EventManager.Unsubscribe(GameEventType.RichToiletKill, () => RichManProgress(GameEventType.RichToiletKill));
        EventManager.Unsubscribe(GameEventType.RichHide, () => RichManProgress(GameEventType.RichHide));

        // CleanMan 이벤트
        EventManager.Unsubscribe(GameEventType.Garbage, () => CleanManProgress(GameEventType.Garbage));
        EventManager.Unsubscribe(GameEventType.CleanManDie, () => CleanManProgress(GameEventType.CleanManDie));
        EventManager.Unsubscribe(GameEventType.CleanManTalk, () => CleanManProgress(GameEventType.CleanManTalk));
        EventManager.Unsubscribe(GameEventType.RichKill, () => CleanManProgress(GameEventType.RichKill));
        EventManager.Unsubscribe(GameEventType.PlayerToiletOut, () => CleanManProgress(GameEventType.RichHide));
        EventManager.Unsubscribe(GameEventType.OldManOut, () => CleanManProgress(GameEventType.OldManOut));
        EventManager.Unsubscribe(GameEventType.CleanManHide, () => CleanManProgress(GameEventType.CleanManHide));

        // DoorGuard 이벤트
        EventManager.Unsubscribe(GameEventType.PlayerEnterBank, () => DoorGuardProgress(GameEventType.PlayerEnterBank));
        EventManager.Unsubscribe(GameEventType.GameClear, () => DoorGuardProgress(GameEventType.GameClear));

        // RightRoomGaurd 이벤트
        EventManager.Unsubscribe(GameEventType.PlayerEnterBank, () => RightRoomGaurdProgress(GameEventType.PlayerEnterBank));
        EventManager.Unsubscribe(GameEventType.Carkick, () => RightRoomGaurdProgress(GameEventType.Carkick));

        // LobbyGaurd 이벤트
        EventManager.Unsubscribe(GameEventType.PlayerEnterBank, () => LobbyGaurdProgress(GameEventType.PlayerEnterBank));
        EventManager.Unsubscribe(GameEventType.Carkick, () => LobbyGaurdProgress(GameEventType.Carkick));

        // Teller 이벤트
        EventManager.Unsubscribe(GameEventType.PlayerEnterBank, () => TellerProgress(GameEventType.PlayerEnterBank));
        EventManager.Unsubscribe(GameEventType.TellerTalk, () => TellerProgress(GameEventType.TellerTalk));
        EventManager.Unsubscribe(GameEventType.RichmanTalkTeller, () => TellerProgress(GameEventType.RichmanTalkTeller));
        EventManager.Unsubscribe(GameEventType.OldManTalkTeller, () => TellerProgress(GameEventType.OldManTalkTeller));
        EventManager.Unsubscribe(GameEventType.RichToiletKill, () => TellerProgress(GameEventType.RichToiletKill));
        EventManager.Unsubscribe(GameEventType.GameClear, () => TellerProgress(GameEventType.GameClear));

        // TrashBin 이벤트
        EventManager.Unsubscribe(GameEventType.Garbage, () => TrashBinProgress(GameEventType.Garbage));

        // Car 이벤트
        EventManager.Unsubscribe(GameEventType.Carkick, () => CarProgress(GameEventType.Carkick));

        // GameOver 이벤트
        EventManager.Unsubscribe(GameEventType.GameOver, () => GameOverProgress(GameEventType.GameOver));
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
                isCleanDie = true;
                break;
            case GameEventType.CleanManTalk:
                if (!isCleanDie)
                    CleanManText.text = "Mop State";
                break;
            case GameEventType.RichKill:
                if (!isCleanDie)
                    CleanManText.text = "Chase Sound State";
                break;
            case GameEventType.PlayerToiletOut:
                if (!isCleanDie)
                    CleanManText.text = "Check State";
                break;
            case GameEventType.OldManOut:
                if (!isCleanDie)
                    CleanManText.text = "Mop State";
                break;
            case GameEventType.CleanManHide:
                CleanManText.text = "NONE";
                break;

        }
    }

    private void DoorGuardProgress(GameEventType eventType)
    {
        switch (eventType)
        {
            case GameEventType.PlayerEnterBank:
                DoorGaurdText.text = "Block State";
                break;
            case GameEventType.GameClear:
                DoorGaurdText.text = "No Block State";
                break;
        }
    }
    private void RightRoomGaurdProgress(GameEventType eventType)
    {
        switch (eventType)
        {
            case GameEventType.PlayerEnterBank:
                RightRoomGaurdText.text = "Block State";
                break;
            case GameEventType.Carkick:
                RightRoomGaurdText.text = "Patrol State";
                break;
        }
    }

    private void LobbyGaurdProgress(GameEventType eventType)
    {
        switch (eventType)
        {
            case GameEventType.PlayerEnterBank:
                LobbyGaurdText.text = "Patrol State";
                break;
            case GameEventType.Carkick:
                LobbyGaurdText.text = "Chase Patrol State";
                break;
        }
    }
    private void TellerProgress(GameEventType eventType)
    {
        switch (eventType)
        {
            case GameEventType.PlayerEnterBank:
                TellerText.text = "Waiting State";
                break;
            case GameEventType.TellerTalk:
                TellerText.text = "Player Interact State";
                break;
            case GameEventType.RichmanTalkTeller:
                TellerText.text = "Gone State";
                break;
            case GameEventType.OldManTalkTeller:
                TellerText.text = "DeadLock State";
                break;
            case GameEventType.RichToiletKill:
                TellerText.text = "Waiting State";
                break;
            case GameEventType.GameClear:
                TellerText.text = "Clear State";
                break;
        }
    }
    private void TrashBinProgress(GameEventType eventType)
    {
        switch (eventType)
        {
            case GameEventType.Garbage:
                TrashBinText.text = "Active";
                break;
        }
    }
    private void CarProgress(GameEventType eventType)
    {
        switch (eventType)
        {
            case GameEventType.Carkick:
                CarText.text = "Active";
                break;
        }
    }
    private void GameOverProgress(GameEventType eventType)
    {
        foreach(TextMeshProUGUI textmesh in TextList)
        {
            textmesh.text = "To Reset ...";
        }
    }
}
