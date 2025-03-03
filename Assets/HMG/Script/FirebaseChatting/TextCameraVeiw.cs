using UnityEngine;

public class TextCameraVeiw : MonoBehaviour
{
    private Transform mainCamera;

    private void Start()
    {
        mainCamera = Camera.main.transform; // 메인 카메라 가져오기
    }

    private void LateUpdate()
    {
        if (mainCamera != null)
        {//대화 UI가 플레이어를 바라보게 함
            transform.LookAt(transform.position + mainCamera.rotation * Vector3.forward, mainCamera.rotation * Vector3.up);
        }
    }
}
