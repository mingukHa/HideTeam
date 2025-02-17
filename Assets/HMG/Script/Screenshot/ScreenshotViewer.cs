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
    public FadeOut fade;
    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.GameOver, gameover);
    }
    private void gameover()
    {
        Debug.Log("������ ������");
        Time.timeScale = 0f;
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
            
            StartSlideshow();
        }
    }

    public void StartSlideshow()
    {
        if (ScreenshotManager.Instance.screenshots.Count > 0)
        {

            StartCoroutine(ShowScreenshots());
            quad.SetActive(true);
            Time.timeScale = 0f;
            Post.SetActive(true);
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
            fade.Fadeout(0.5f);
            yield return new WaitForSecondsRealtime(displayTime);
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }
}
