using TMPro;
using UnityEngine;

public class HelpChat : MonoBehaviour
{
    [Header("도움말 텍스트 넣는 곳")]
    public TextMeshProUGUI textMeshPro;
    [Header("텍스트 넣을 것")]
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
