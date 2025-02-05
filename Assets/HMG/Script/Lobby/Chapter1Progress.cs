using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class Chapter1Progress : MonoBehaviour
{
    [SerializeField] private Image image; // 게이지 바
    [SerializeField] private float maxValue = 100f; // 최대 값
    private float currentValue; // 현재 값

    private float maxWidth = 577f; // 게이지 바의 최대 너비

    private void Start()
    {
        currentValue = maxValue; // 시작 시 게이지를 최대값으로 설정
        UpdateGaugeBar();
    }

    public void SetGauge(float value)
    {
        currentValue = Mathf.Clamp(value, 0, maxValue); // 값이 0~maxValue 사이로 유지
        UpdateGaugeBar();
    }

    private void UpdateGaugeBar()
    {
        float fillWidth = (currentValue / maxValue) * maxWidth; // 비율 계산
        fillWidth = Mathf.Clamp(fillWidth, 0, maxWidth); // 최소/최대값 제한
        image.rectTransform.sizeDelta = new Vector2(fillWidth, image.rectTransform.sizeDelta.y);
    }
}

    

