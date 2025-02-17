using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using static EventManager;
using UnityEngine.InputSystem;

public class ScreenshotViewer : MonoBehaviour
{
    public Renderer quadRenderer; // Quad의 Mesh Renderer
    private Material quadMaterial; // Quad의 Material
    public float displayTime = 1.0f;
    public GameObject Post;
    public GameObject quad;
    public GameObject efKey;
    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.GameOver, gameover);
    }
    private void gameover()
    {
        Debug.Log("게임이 오버됨");
        quad.SetActive(true);
        Post.SetActive(true);
        efKey.SetActive(false);
        StartSlideshow();
    }
    void Start()
    {
        // Quad의 Material 가져오기
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
        }
        else
        {
            Debug.Log("No screenshots available.");
        }
    }

    private IEnumerator ShowScreenshots()
    {
        List<Texture2D> tempScreenshots = new List<Texture2D>(ScreenshotManager.Instance.screenshots);

        for (int i = tempScreenshots.Count - 1; i >= 0; i--)
        {
            if (quadMaterial != null)
            {
                quadMaterial.mainTexture = tempScreenshots[i];
            }

            
            yield return new WaitForSecondsRealtime(displayTime);
        }

        SceneManager.LoadScene("MainScene");
        
    }

}
