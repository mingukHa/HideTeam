using UnityEngine;

public class MarkerManager : MonoBehaviour
{
    private Renderer markerRenderer;
    private MaterialPropertyBlock propertyBlock;
    private float fillAmount = 0f; // ���� FillAmount ��

    private void Initialize()
    {
        // Renderer�� Property Block �ʱ�ȭ
        markerRenderer = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

}
