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
    public FadeOut fade;

    public Clockss clockss;
    public Renderer quadRenderer; // Quad의 Mesh Renderer
    private Material quadMaterial; // Quad의 Material
    public float displayTime = 1.0f;
    public GameObject Post;
    public GameObject quad;
    public GameObject efKey;
    public GameObject chat;
    public GameObject Clock;
    public GameObject Nav;
    public GameObject Light;
    public GameObject Fadeimg;
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
        if (quadRenderer != null)
        {
            quadMaterial = quadRenderer.material;
        }
    }
    private void gameover()
    {
        clockss.isReturning = true;
        
        chat.SetActive(false);
        quad.SetActive(true);
        Post.SetActive(true);
        efKey.SetActive(false);
        Nav.SetActive(false);
        StartSlideshow();
    }
    

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            //정 방향
            Light.SetActive(false);
            StartCoroutine(GameRestart());
            Clock.SetActive(true);
        }
    }

    private IEnumerator GameRestart()
    {
        yield return new WaitForSeconds(0.6f);
        EventManager.Trigger(GameEventType.GameOver);
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
        List<Texture2D> tempScreenshots = new List<Texture2D>(ScreenshotManager.Instance.screenshots);
        for (int i = tempScreenshots.Count - 1; i >= 0; i--)
        {
            if (quadMaterial != null)
            {
                quadMaterial.mainTexture = tempScreenshots[i];
            }

            yield return new WaitForSecondsRealtime(displayTime);
        }
        Fadeimg.SetActive(true);
        yield return new WaitForSecondsRealtime(displayTime);
        StopAllCoroutines();
        
        SceneManager.LoadScene("MainScene");
        
    }

}
