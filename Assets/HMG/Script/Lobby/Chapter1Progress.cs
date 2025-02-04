using UnityEngine;
using UnityEngine.UI;

public class Chapter1Progress : MonoBehaviour
{
    [SerializeField] private RectTransform gaugeRect; // ������ �� RectTransform
    [SerializeField] private float maxValue = 100f; // �ִ� ��
    private float currentValue; // ���� ��

    private float maxWidth = 577f; // ������ ���� �ִ� �ʺ�

    private void Start()
    {
        currentValue = maxValue; // ���� �� �������� �ִ밪���� ����
        UpdateGaugeBar();
    }

    public void SetGauge(float value)
    {
        currentValue = Mathf.Clamp(value, 0, maxValue); // ���� 0~maxValue ���̷� ����
        UpdateGaugeBar();
    }

    private void UpdateGaugeBar()
    {
        float width = (currentValue / maxValue) * maxWidth; // ���� ���� ����� �ʺ� ���
        gaugeRect.sizeDelta = new Vector2(width, gaugeRect.sizeDelta.y); // �ʺ� ����
    }
}
