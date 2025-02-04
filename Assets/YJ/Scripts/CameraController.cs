using UnityEngine;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineCamera followCam; // �⺻ ī�޶�
    public CinemachineCamera freeLookCam; // ���� ���� ī�޶�

    private bool isMoving = false;

    private void Start()
    {
        // �ʱ� ����: �⺻ ī�޶� Ȱ��ȭ
        SetCameraState(true);
    }

    private void Update()
    {
        isMoving = Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0;

        if (isMoving)
        {
            SetCameraState(true); // �⺻ ī�޶� Ȱ��ȭ
        }
        else if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            SetCameraState(false); // ���� ���� ī�޶� Ȱ��ȭ
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
