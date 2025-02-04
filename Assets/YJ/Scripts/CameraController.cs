using UnityEngine;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineCamera followCam; // 기본 카메라
    public CinemachineCamera freeLookCam; // 자유 시점 카메라

    private bool isMoving = false;

    private void Start()
    {
        // 초기 설정: 기본 카메라 활성화
        SetCameraState(true);
    }

    private void Update()
    {
        isMoving = Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0;

        if (isMoving)
        {
            SetCameraState(true); // 기본 카메라 활성화
        }
        else if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            SetCameraState(false); // 자유 시점 카메라 활성화
        }
    }

    private void SetCameraState(bool useFollowCam)
    {
        if (useFollowCam)
        {
            followCam.Priority = 10;
            freeLookCam.Priority = 5;
        }
        else
        {
            followCam.Priority = 5;
            freeLookCam.Priority = 10;
        }
    }
}
