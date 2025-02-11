using Unity.VisualScripting;
using UnityEngine;

public class TrashBin : MonoBehaviour
{
    public Transform trashBinRid;
    public GameObject[] trashObjects; // Trash 오브젝트 4개
    //public Transform[] trashPositions; // 바닥에 떨어질 위치
    private Rigidbody rb = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void MessUpTrashBin()
    {
        //// Trash 오브젝트를 바닥 위치로 이동
        //for (int i = 0; i < trashObjects.Length; i++)
        //{
        //    if (i < trashPositions.Length)
        //    {
        //        trashObjects[i].transform.position = trashPositions[i].position;
        //        trashObjects[i].transform.rotation = trashPositions[i].rotation;
        //    }
        //}

        trashBinRid.Rotate(-90f, 0f, 0f);
        rb.AddForce(Vector3.up, ForceMode.Impulse);
    }
}
