using UnityEngine;

public class PlayerCanvasFollow : MonoBehaviour
{
    public Transform playerCamera; // 카메라의 Transform
    public Canvas playerCanvas; // PlayerHolder 내부의 Canvas
    public Vector3 offset = new Vector3(0, -0.5f, 1f); // 화면 내 위치 조절

    void Update()
    {
        if (playerCamera == null || playerCanvas == null) return;

        // Canvas가 카메라 앞에 고정되도록 설정
        Vector3 targetPosition = playerCamera.position + playerCamera.forward * offset.z + playerCamera.up * offset.y;
        playerCanvas.transform.position = targetPosition;

        // Y축 회전만 따라가도록 설정 (기울어짐 방지)
        Vector3 lookDirection = playerCanvas.transform.position - playerCamera.position;
        lookDirection.y = 0; // X, Z만 적용하고 Y축은 고정
        playerCanvas.transform.rotation = Quaternion.LookRotation(lookDirection);
    }
}
