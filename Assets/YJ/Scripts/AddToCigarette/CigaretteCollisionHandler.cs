using System.Collections;
using UnityEngine;

public class CigaretteCollisionHandler : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ������Ʈ�� "Map" �±����� Ȯ��
        if (collision.gameObject.CompareTag("Map"))
        {
            StartCoroutine(StopAndDestroyCigarette());
        }
    }

    private IEnumerator StopAndDestroyCigarette()
    {
        // ���� ����
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        // 5�� ��� �� ������Ʈ ����
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
