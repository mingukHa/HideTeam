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
        while (true) // 무한 루프 실행 (도착 판정 후 종료)
        {
            Stick[0].transform.position = Vector3.MoveTowards(Stick[0].transform.position, StickPoint[0], MoveSpeed * Time.deltaTime);

            // 목표에 도달하면 종료
            if (Vector3.Distance(Stick[0].transform.position, StickPoint[0]) < 0.05f)
            {
                Debug.Log("이동 완료");
                Stick[0].transform.position = StickPoint[0]; // 최종 위치 보정
                yield break; // 코루틴 즉시 종료
            }

            yield return null;
        }
    }
}
