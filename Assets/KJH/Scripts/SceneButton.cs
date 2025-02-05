using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    public void OnClicked()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
