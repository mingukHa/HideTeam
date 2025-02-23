using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    // 이동할 씬 이름을 지정
    public string sceneName;

    public void ChangeScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("씬 이름이 설정되지 않았습니다.");
        }
    }
}