using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ScreenshotManager : MonoBehaviour
{
    public static ScreenshotManager Instance; //싱글톤 방식의 매니저
    public List<Texture2D> screenshots = new List<Texture2D>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; //단 하나만 존재해야 함
        }
        
    }
  
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O)) //스크린샷 테스트 버튼
        {
            CaptureScreenshot();
        }
        
    }
    public void CaptureScreenshot()
    {
        StartCoroutine(TakeScreenshot());
    }

    private IEnumerator TakeScreenshot() //스크린샷 저장
    {
        yield return new WaitForEndOfFrame();

        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshot.Apply();

        screenshots.Add(screenshot);
        Debug.Log("Screenshot Captured! Total: " + screenshots.Count);
    }
}
