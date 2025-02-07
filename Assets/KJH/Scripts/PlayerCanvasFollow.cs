using UnityEngine;

public class PlayerCanvasFollow : MonoBehaviour
{
    public Transform playerCamera; // ī�޶��� Transform
    public Canvas playerCanvas; // PlayerHolder ������ Canvas
    public Vector3 offset = new Vector3(0, -0.5f, 1f); // ȭ�� �� ��ġ ����

    void Update()
    {
        if (playerCamera == null || playerCanvas == null) return;

        // Canvas�� ī�޶� �տ� �����ǵ��� ����
        Vector3 targetPosition = playerCamera.position + playerCamera.forward * offset.z + playerCamera.up * offset.y;
        playerCanvas.transform.position = targetPosition;

        // Y�� ȸ���� ���󰡵��� ���� (������ ����)
        Vector3 lookDirection = playerCanvas.transform.position - playerCamera.position;
        lookDirection.y = 0; // X, Z�� �����ϰ� Y���� ����
        playerCanvas.transform.rotation = Quaternion.LookRotation(lookDirection);
    }
}
