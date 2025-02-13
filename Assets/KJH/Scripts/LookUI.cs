using UnityEngine;

public class LookUI : MonoBehaviour
{
    private Camera cam;

    private void Start()
    {
        if(cam == null)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }

    private void Update()
    {
        if(cam != null)
        {
            transform.LookAt(transform.position + 
                cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
        }
    }
}
