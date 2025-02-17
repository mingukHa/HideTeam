using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenshotManager : MonoBehaviour
{
    public static ScreenshotManager Instance;
    public List<Texture2D> screenshots = new List<Texture2D>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            CaptureScreenshot();
        }
    }
    public void CaptureScreenshot()
    {
        StartCoroutine(TakeScreenshot());
    }

    private IEnumerator TakeScreenshot()
    {
        yield return new WaitForEndOfFrame();

        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshot.Apply();

        screenshots.Add(screenshot);
        Debug.Log("Screenshot Captured! Total: " + screenshots.Count);
    }
}
