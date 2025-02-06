using System.Collections;
using UnityEngine;

public class VaultDoor : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Stick;
    [SerializeField]
    private Vector3[] StickPoint;
    [SerializeField]
    private GameObject[] Valve;
    [SerializeField]
    private int[] ValveRotation;
    [SerializeField]
    private float MoveTime = 1f;
    [SerializeField]
    private float MoveSpeed = 0.05f;

    private void Start()
    {
        StartCoroutine(StickMove());
    }

    private IEnumerator StickMove()
    {
        while (true) // ���� ���� ���� (���� ���� �� ����)
        {
            Stick[0].transform.position = Vector3.MoveTowards(Stick[0].transform.position, StickPoint[0], MoveSpeed * Time.deltaTime);

            // ��ǥ�� �����ϸ� ����
            if (Vector3.Distance(Stick[0].transform.position, StickPoint[0]) < 0.05f)
            {
                Debug.Log("�̵� �Ϸ�");
                Stick[0].transform.position = StickPoint[0]; // ���� ��ġ ����
                yield break; // �ڷ�ƾ ��� ����
            }

            yield return null;
        }
    }
}
