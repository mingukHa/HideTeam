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
        // 충돌한 오브젝트가 "Map" 태그인지 확인
        if (collision.gameObject.CompareTag("Map"))
        {
            StartCoroutine(StopAndDestroyCigarette());
        }
    }

    private IEnumerator StopAndDestroyCigarette()
    {
        // 물리 중지
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        // 5초 대기 후 오브젝트 삭제
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
