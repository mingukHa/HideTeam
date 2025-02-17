using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ScreenshotViewer : MonoBehaviour
{
    public Renderer quadRenderer; // Quad의 Mesh Renderer
    private Material quadMaterial; // Quad의 Material
    public float displayTime = 1.0f;
    public GameObject Post;

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
        List<Texture2D> tempScreenshots = new List<Texture2D>(ScreenshotManager.Instance.screenshots); // 리스트 복사

        for (int i = tempScreenshots.Count - 1; i >= 0; i--) // 역순 재생
        {
            if (quadMaterial != null)
            {
                quadMaterial.mainTexture = tempScreenshots[i]; // Quad의 Material 업데이트
            }

            yield return new WaitForSeconds(displayTime);
        }

        SceneManager.LoadScene("MainScene");
    }
}
