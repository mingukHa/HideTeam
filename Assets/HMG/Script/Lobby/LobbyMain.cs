using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMain : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerNameText; // UI에 표시할 TextMeshProUGUI
    [SerializeField]
    private GameObject Chapter1;
    [SerializeField]
    private Button Chapter1OpenButton;
    [SerializeField]
    private Button Chapter1CloseButton;
    [SerializeField]
    private Button Chapter1StartButton;
    private void Start()
    {
        // 저장된 플레이어 이름 불러오기
        string username = PlayerPrefs.GetString("username", "Guest");
        playerNameText.text = $"{username}";
        Chapter1OpenButton.onClick.AddListener(() => OnChapter1());
        Chapter1CloseButton.onClick.AddListener(() => OffChapter1());
        Chapter1StartButton.onClick.AddListener(() => Chapter1Start());

    }
    private void OnChapter1()
    {
        Chapter1.SetActive(true);
    }
    private void OffChapter1()
    {
        Chapter1.SetActive(false);
    }
    private void Chapter1Start()
    {
        SceneManager.LoadScene("MainScene");
    }
}
