using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ESCMenu : MonoBehaviour
{
    [SerializeField] private GameObject EscMenu;
    [SerializeField] private Button ReturnGame;
    [SerializeField] private Button Sound;
    [SerializeField] private Button RobbyReturn;
    [SerializeField] private Button GameOff;
    [SerializeField] private GameObject EscMain;
    
    private static bool isPaused = false;
   
    private void Start()
    {
        ReturnGame.onClick.AddListener(Returngame);
        RobbyReturn.onClick.AddListener(Lobbygame);
        GameOff.onClick.AddListener(QuitGame);
    }

    private void Update()
    {
        // ESC Ű�� ������ �� �޴� ���
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        EscMenu.SetActive(true);
        Time.timeScale = 0; // ���� �Ͻ�����
    }

    private void ResumeGame()
    {
        isPaused = false;
        EscMenu.SetActive(false);
        Time.timeScale = 1; // ���� �簳
    }

    private void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void Returngame()
    {
        ResumeGame(); // ���� �簳
    }

    private void Lobbygame()
    {
        ResumeGame(); 
        SceneManager.LoadScene("LobbyScene");
    }
}
