using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenshotViewer : MonoBehaviour
{
    public RawImage displayImage;  // UI에 배치할 RawImage
    public float displayTime = 1.0f;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
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
        foreach (var screenshot in ScreenshotManager.Instance.screenshots)
        {
            displayImage.texture = screenshot;
            yield return new WaitForSeconds(displayTime);
        }
    }
}
