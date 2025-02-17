using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using static EventManager;

public class ScreenshotViewer : MonoBehaviour
{
    public Renderer quadRenderer; // Quad�� Mesh Renderer
    private Material quadMaterial; // Quad�� Material
    public float displayTime = 1.0f;
    public GameObject Post;
    public GameObject quad;

    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.Garbage, gameover);
    }
    private void gameover()
    {
        Debug.Log("������ ������");
        quad.SetActive(true);
        Post.SetActive(true);
        StartSlideshow();
    }
    void Start()
    {
        // Quad�� Material ��������
        if (quadRenderer != null)
        {
            quadMaterial = quadRenderer.material;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            quad.SetActive(true);
            Post.SetActive(true);
            StartSlideshow();
        }
    }

    public void StartSlideshow()
    {
        if (ScreenshotManager.Instance.screenshots.Count > 0)
        {
            StartCoroutine(ShowScreenshots());
        }
        else
        {
            Debug.Log("No screenshots available.");
        }
    }

    private IEnumerator ShowScreenshots()
    {
        List<Texture2D> tempScreenshots = new List<Texture2D>(ScreenshotManager.Instance.screenshots); // ����Ʈ ����

        for (int i = tempScreenshots.Count - 1; i >= 0; i--) // ���� ���
        {
            if (quadMaterial != null)
            {
                quadMaterial.mainTexture = tempScreenshots[i]; // Quad�� Material ������Ʈ
            }

            yield return new WaitForSeconds(displayTime);
        }

        SceneManager.LoadScene("MainScene");
    }
}
