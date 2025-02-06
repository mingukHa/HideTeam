using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    public bool isOpen = false; // 문 상태 (열림/닫힘)
    public float openAngle = 90f; // 문이 열릴 각도
    public float animationTime = 1f; // 문 열림/닫힘 애니메이션 시간

    private Quaternion closedRotation; // 닫힌 상태의 회전값
    private Quaternion openRotation; // 열린 상태의 회전값

    void Start()
    {
        closedRotation = transform.rotation;
    }

    // 플레이어나 NPC가 바라보는 방향을 기준으로 문 열기
    public void OpenDoorBasedOnView(Transform entity)
    {
        if (entity == null) return;

        // 플레이어(NPC)의 바라보는 방향
        Vector3 entityForward = entity.forward;

        // 문의 정면 방향 (문이 바라보는 방향)
        Vector3 doorForward = transform.forward;

        // 바라보는 방향과 문의 방향의 각도 계산
        float angle = Vector3.Angle(entityForward, doorForward);

        if (angle < 90) // 플레이어가 문을 정면에서 바라볼 때
        {
            openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;
        }
        else // 플레이어가 문을 뒤쪽에서 바라볼 때 (반대 방향에서 접근)
        {
            openRotation = Quaternion.Euler(0, -openAngle, 0) * closedRotation;
        }

        ToggleDoor();
    }

    // 문 열기/닫기 토글
    public void ToggleDoor()
    {
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    // 문 열기
    public void OpenDoor()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateDoor(openRotation));
        isOpen = true;
    }

    // 문 닫기
    public void CloseDoor()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateDoor(closedRotation));
        isOpen = false;
    }

    // 문 애니메이션 처리
    private IEnumerator AnimateDoor(Quaternion targetRotation)
    {
        float elapsedTime = 0;
        Quaternion startingRotation = transform.rotation;

        while (elapsedTime < animationTime)
        {
            transform.rotation = Quaternion.Slerp(startingRotation, targetRotation, elapsedTime / animationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    // 플레이어나 NPC가 문 가까이에 왔을 때 실행
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("NPC"))
        {
            OpenDoorBasedOnView(other.transform);
        }
    }

    // 문을 떠날 때 닫기
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("NPC"))
        {
            CloseDoor();
        }
    }
}
