using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ESCMenu : MonoBehaviour
{
    [SerializeField] private GameObject EscMenu;
    [SerializeField] private Button ReturnGame;
    [SerializeField] private Button Sound;
    [SerializeField] private Button RobbyReturn;

    private bool EscMenuOnOff = false;
    private void Start()
    {
        ReturnGame.onClick.AddListener(() => Returngame());
        RobbyReturn.onClick.AddListener(() => Lobbygame());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!EscMenuOnOff)
            {
                EscMenuOnOff = true;
                EscMenu.SetActive(true);
            }
            else
            {
                EscMenuOnOff = false;
                EscMenu.SetActive(false);
            }
        }
    }

    private void Returngame()
    {
        EscMenuOnOff = false;
        EscMenu.SetActive(false);
    }
    private void Lobbygame()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
