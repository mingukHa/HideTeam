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
            // UI ��ġ�� ĳ���� �Ӹ� ���� ����
            transform.position = target.position;

            // UI�� ī�޶� ��Ȯ�ϰ� �ٶ󺸵��� ����
            Vector3 cameraForward = cam.transform.forward;
            cameraForward.y = 0; // UI�� �������� �� ���� (���� ȸ����)

            transform.rotation = Quaternion.LookRotation(cameraForward);
        }
    }
}
