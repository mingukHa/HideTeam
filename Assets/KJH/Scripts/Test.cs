using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    // �̵��� �� �̸��� ����
    public string sceneName;

    public void ChangeScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("�� �̸��� �������� �ʾҽ��ϴ�.");
        }
    }
}