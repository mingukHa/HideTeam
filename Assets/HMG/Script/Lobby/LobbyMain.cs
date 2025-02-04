using TMPro;
using UnityEngine;

public class LobbyMain : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerNameText; // UI에 표시할 TextMeshProUGUI

    private void Start()
    {
        // 저장된 플레이어 이름 불러오기
        string username = PlayerPrefs.GetString("username", "Guest");

        // UI에 표시
        playerNameText.text = $"플레이어: {username}";
    }
}
