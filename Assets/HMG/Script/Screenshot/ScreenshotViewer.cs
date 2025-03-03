using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using static EventManager;
using UnityEngine.InputSystem;
using System;
using UnityEngine.AI;

public class ScreenshotViewer : MonoBehaviour
{
    public FadeOut Fade;
    public PlayerController Pc;
    public Clockss Clockss;
    public Renderer QuadRenderer; // Quad의 Mesh Renderer
    private Material QuadMaterial; // Quad의 Material
    public float DisplayTime = 1.0f;
    public GameObject Post;
    public GameObject Quad;
    public GameObject EfKey;
    public GameObject Chat;
    public GameObject Clock;
    public GameObject Nav;
    public GameObject Light;
    public GameObject Fadeimg;
    public GameObject Text;
    
    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.GameOver, gameover);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEventType.GameOver, gameover);
    }
    
    private void Start()
    {
        // Quad의 Material 가져오기
        if (QuadRenderer != null)
        {
            QuadMaterial = QuadRenderer.material;
        }
    }
    private void gameover()
    {
        Clockss.isReturning = true;
        Chat.SetActive(false);
        Quad.SetActive(true);
        Post.SetActive(true);
        EfKey.SetActive(false);
        Nav.SetActive(false);
        Clock.SetActive(true);
        StartSlideshow();
        Light.SetActive(false);
        Text.SetActive(false);
       
    }

    public IEnumerator GameRestart()
    {
        yield return null;
        EventManager.Trigger(GameEventType.GameOver);
    }
 
    public void DisableAllMoutline()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = currentScene.GetRootGameObjects();

        int count = 0;

        foreach (GameObject rootObject in rootObjects)
        {
            Moutline[] outlines = rootObject.GetComponentsInChildren<Moutline>(true);

            foreach (Moutline outline in outlines)
            {
                outline.enabled = false;
                count++;
            }
        }

        Debug.Log($"현재 씬에서 {count}개의 Moutline을 비활성화했습니다.");
    }


public void StartSlideshow()
    {
        if (ScreenshotManager.Instance.screenshots.Count > 0)
        {
            
            StartCoroutine(ShowScreenshots());
        }
        else
        {
            SceneManager.LoadScene("MainScene");
        }
    }
    private IEnumerable Returnroding ()
    {
        yield return null;
    }
    private IEnumerator ShowScreenshots()
    {
        DisableAllMoutline();
        EventManager.Trigger(GameEventType.TellerTalk);
        List<Texture2D> tempScreenshots = new List<Texture2D>(ScreenshotManager.Instance.screenshots);
        for (int i = tempScreenshots.Count - 1; i >= 0; i--)
        {
            if (QuadMaterial != null)
            {
                QuadMaterial.mainTexture = tempScreenshots[i];
            }

            yield return new WaitForSecondsRealtime(DisplayTime);
        }
        Fadeimg.SetActive(true);
        yield return new WaitForSecondsRealtime(DisplayTime);
        //StopAllCoroutines();
        
        SceneManager.LoadScene("MainScene");
        
    }

}
