using Unity.VisualScripting;
using UnityEngine;

public class TrashBin : MonoBehaviour
{
    public Transform trashBinRid;
    public GameObject[] trashObjects; // Trash ������Ʈ 4��
    //public Transform[] trashPositions; // �ٴڿ� ������ ��ġ
    private Rigidbody rb = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void MessUpTrashBin()
    {
        //// Trash ������Ʈ�� �ٴ� ��ġ�� �̵�
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
