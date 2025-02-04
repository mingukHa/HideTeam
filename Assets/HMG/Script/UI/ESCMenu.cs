using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ESCMenu : MonoBehaviour
{
    [SerializeField] private GameObject EscMenu;
    [SerializeField] private Button ReturnGame;
    [SerializeField] private Button Sound;
    [SerializeField] private GameObject SoundBar;
    [SerializeField] private Button RobbyReturn;
    [SerializeField] private Button GameOff;
    [SerializeField] private GameObject EscMain;
    
    private static bool isPaused = false;
    private bool SoundBerOnOff = false;
    private void Start()
    {
        ReturnGame.onClick.AddListener(Returngame);
        RobbyReturn.onClick.AddListener(Lobbygame);
        GameOff.onClick.AddListener(QuitGame);
        Sound.onClick.AddListener(Soundbar);
    }

    private void Update()
    {
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
    private void Soundbar()
    {
        if (SoundBerOnOff == false)
        {
            SoundBerOnOff = true;
            SoundBar.SetActive(true);
        }
        else
        {
            SoundBerOnOff = false;
            SoundBar.SetActive(false);
        }
    }
    private void PauseGame()
    {
        isPaused = true;
        EscMenu.SetActive(true);
        Time.timeScale = 0; // 게임 일시정지
    }

    private void ResumeGame()
    {
        isPaused = false;
        EscMenu.SetActive(false);
        Time.timeScale = 1; // 게임 재개
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
        ResumeGame(); // 게임 재개
    }

    private void Lobbygame()
    {
        ResumeGame(); 
        SceneManager.LoadScene("LobbyScene");
    }
}
