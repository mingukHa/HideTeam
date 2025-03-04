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
    
    private void OnEnable()
    {
        // OldMan�̺�Ʈ
        EventManager.Subscribe(GameEventType.TellerTalk, () => OldManProgress(GameEventType.TellerTalk));
        EventManager.Subscribe(GameEventType.RichKill, () => OldManProgress(GameEventType.RichKill));
    }
    //���ٽ����� �־���� �̺�Ʈ Ÿ���� switch���� �ֱ� ����
    private void OnDisable()
    {
        // OldMan�̺�Ʈ
        EventManager.Unsubscribe(GameEventType.TellerTalk, () => OldManProgress(GameEventType.TellerTalk));
        EventManager.Unsubscribe(GameEventType.RichKill, () => OldManProgress(GameEventType.RichKill));
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
            case GameEventType.TellerTalk:
                OldManText.text = "�ڷ��� �̾߱� ��";
                break;
            case GameEventType.RichKill:
                OldManText.text = "������ ������ ��";
                break;
            
        }
    }
}
