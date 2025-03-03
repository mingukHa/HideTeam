using UnityEngine;

public class TextCameraVeiw : MonoBehaviour
{
    private Transform mainCamera;

    private void Start()
    {
        mainCamera = Camera.main.transform; // ���� ī�޶� ��������
    }

    private void LateUpdate()
    {
        if (mainCamera != null)
        {//��ȭ UI�� �÷��̾ �ٶ󺸰� ��
            transform.LookAt(transform.position + mainCamera.rotation * Vector3.forward, mainCamera.rotation * Vector3.up);
        }
    }
}
