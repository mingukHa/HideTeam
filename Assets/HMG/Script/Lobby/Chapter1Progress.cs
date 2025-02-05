using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class Chapter1Progress : MonoBehaviour
{
    [SerializeField] private Image image; // ������ ��
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
        float fillWidth = (currentValue / maxValue) * maxWidth; // ���� ���
        fillWidth = Mathf.Clamp(fillWidth, 0, maxWidth); // �ּ�/�ִ밪 ����
        image.rectTransform.sizeDelta = new Vector2(fillWidth, image.rectTransform.sizeDelta.y);
    }
}

    

