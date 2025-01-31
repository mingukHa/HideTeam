using UnityEngine;

public class TextCameraVeiw : MonoBehaviour
{
    private Transform mainCamera;

    void Start()
    {
        mainCamera = Camera.main.transform; // ���� ī�޶� ��������
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.rotation * Vector3.forward,
                             mainCamera.rotation * Vector3.up);
        }
    }
}
