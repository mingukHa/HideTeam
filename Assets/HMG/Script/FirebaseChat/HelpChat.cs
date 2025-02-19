using TMPro;
using UnityEngine;

public class HelpChat : MonoBehaviour
{
    [Header("���� �ؽ�Ʈ �ִ� ��")]
    public TextMeshProUGUI textMeshPro;
    [Header("�ؽ�Ʈ ���� ��")]
    public string text;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        textMeshPro.text = text;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            textMeshPro.text = "";
    }
}
