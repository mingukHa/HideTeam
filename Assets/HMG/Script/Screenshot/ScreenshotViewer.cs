using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;

public class ScreenshotViewer : MonoBehaviour
{
    public RawImage displayImage;  // UI에 배치할 RawImage
    public float displayTime = 1.0f;
    public GameObject display;
    public GameObject Post;
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Post.SetActive(true);
            display.SetActive(true);
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

        for (int i = tempScreenshots.Count; i <= 1; i--)
        {
            displayImage.texture = tempScreenshots[i];
            yield return new WaitForSeconds(displayTime);
        }

        SceneManager.LoadScene("MainScene");
    }

}
