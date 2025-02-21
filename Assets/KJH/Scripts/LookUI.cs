using UnityEngine;

public class LookUI : MonoBehaviour
{
    private Camera cam;
    public Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(cam == null)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (cam != null && target != null)
        {
            // UI 위치를 캐릭터 머리 위로 고정
            transform.position = target.position;

            // UI가 카메라를 정확하게 바라보도록 설정
            Vector3 cameraForward = cam.transform.forward;
            cameraForward.y = 0; // UI가 기울어지는 걸 방지 (수평 회전만)

            transform.rotation = Quaternion.LookRotation(cameraForward);
        }
    }
}
