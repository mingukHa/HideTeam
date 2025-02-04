using UnityEngine;

public class MarkerManager : MonoBehaviour
{
    private Renderer markerRenderer;
    private MaterialPropertyBlock propertyBlock;
    private float fillAmount = 0f; // 현재 FillAmount 값

    private void Initialize()
    {
        // Renderer와 Property Block 초기화
        markerRenderer = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

}
