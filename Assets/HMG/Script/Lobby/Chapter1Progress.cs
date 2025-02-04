using UnityEngine;
using UnityEngine.UI;

public class Chapter1Progress : MonoBehaviour
{
    [SerializeField] private RectTransform gaugeRect; // 게이지 바 RectTransform
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
        float width = (currentValue / maxValue) * maxWidth; // 현재 값에 비례한 너비 계산
        gaugeRect.sizeDelta = new Vector2(width, gaugeRect.sizeDelta.y); // 너비 변경
    }
}
