using UnityEngine;
using System.Collections;

public class DoorOpen : MonoBehaviour
{
    public bool isOpen = false; // 문 상태 (열림/닫힘)
    public float openAngle = -90f; // 문이 열릴 각도
    public float animationTime = 2f; // 문 열림/닫힘 애니메이션 시간

    public GameObject[] Stick; // 움직이는 스틱들
    public GameObject[] valve1; // 회전하는 벨브들
    public Transform[] StickPoint; // 스틱이 이동할 위치

    public float openvalveAngle1 = 720f; // 첫 번째 벨브 회전 각도
    public float openvalveAngle2 = -720f; // 두 번째 벨브 회전 각도

    private Quaternion closedRotation; // 닫힌 문 회전값
    private Quaternion openRotation; // 열린 문 회전값

    void Start()
    {
        // ✅ 데이터 확인
        CheckComponents();

        // 초기 회전값 설정
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;

        // ✅ 실행 확인을 위해 자동으로 문 열기/닫기 테스트
        ToggleDoor();
    }

    // ✅ 데이터 확인 함수
    private void CheckComponents()
    {
        if (Stick.Length == 0) Debug.LogError("🚨 Stick이 설정되지 않았습니다!");
        if (StickPoint.Length != Stick.Length) Debug.LogError("🚨 StickPoint 개수가 맞지 않습니다!");
        if (valve1.Length < 2) Debug.LogError("🚨 Valve가 누락되었습니다! (필수: 2개)");

        for (int i = 0; i < Stick.Length; i++)
        {
            if (Stick[i] == null) Debug.LogError($"🚨 Stick[{i}]이(가) 설정되지 않았습니다!");
        }
        for (int i = 0; i < valve1.Length; i++)
        {
            if (valve1[i] == null) Debug.LogError($"🚨 Valve[{i}]이(가) 설정되지 않았습니다!");
        }
        for (int i = 0; i < StickPoint.Length; i++)
        {
            if (StickPoint[i] == null) Debug.LogError($"🚨 StickPoint[{i}]이(가) 설정되지 않았습니다!");
        }
    }

    // 문 열기/닫기 토글
    public void ToggleDoor()
    {
        if (isOpen)
        {
            CloseDoor();
            Debug.Log("닫힘");
        }
        else
        {
            OpenDoor();
            Debug.Log("열림");
        }
    }

    // 문 열기 (스틱 → 벨브 → 문 순서대로 애니메이션 실행)
    public void OpenDoor()
    {
        StopAllCoroutines();
        StartCoroutine(OpenSequence());
        isOpen = true;
    }

    // 문 닫기
    private void CloseDoor()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateDoor(closedRotation));
        isOpen = false;
    }

    // 🔹 순차적으로 애니메이션 실행 (스틱 → 벨브 → 문)
    private IEnumerator OpenSequence()
    {
        // 1️⃣ 스틱 이동 (1초)
        yield return StartCoroutine(MoveSticks(1f));

        // 2️⃣ 벨브 회전 (2초)
        yield return StartCoroutine(RotateValves(2f));

        // 3️⃣ 문 열기
        yield return StartCoroutine(AnimateDoor(openRotation));
    }

    // 🔹 스틱 이동 (부드러운 Lerp)
    private IEnumerator MoveSticks(float duration)
    {
        float elapsedTime = 0;
        Vector3[] startPositions = new Vector3[Stick.Length];

        // 스틱의 초기 위치 저장
        for (int i = 0; i < Stick.Length; i++)
        {
            startPositions[i] = Stick[i].transform.position;
        }

        while (elapsedTime < duration)
        {
            for (int i = 0; i < Stick.Length; i++)
            {
                if (Stick[i] != null && StickPoint[i] != null)
                {
                    // ✅ "절대 위치"가 아닌 "상대 위치"로 이동하도록 수정
                    Stick[i].transform.position = Vector3.Lerp(startPositions[i], StickPoint[i].position, elapsedTime / duration);
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 정확한 위치로 이동 보정
        for (int i = 0; i < Stick.Length; i++)
        {
            Stick[i].transform.position = StickPoint[i].position;
        }
    }

    // 🔹 벨브 회전 (부드러운 Slerp)
    private IEnumerator RotateValves(float duration)
    {
        float elapsedTime = 0;
        Quaternion[] startRotations = new Quaternion[valve1.Length];
        Quaternion[] targetRotations = new Quaternion[valve1.Length];

        for (int i = 0; i < valve1.Length; i++)
        {
            startRotations[i] = valve1[i].transform.localRotation;

            // ✅ "Slerp"을 사용하여 벨브가 정상적으로 회전하도록 수정
            float targetAngle = (i == 0) ? openvalveAngle1 : openvalveAngle2;
            targetRotations[i] = startRotations[i] * Quaternion.Euler(0, 0, targetAngle);
        }

        while (elapsedTime < duration)
        {
            for (int i = 0; i < valve1.Length; i++)
            {
                if (valve1[i] != null)
                {
                    valve1[i].transform.localRotation = Quaternion.Slerp(startRotations[i], targetRotations[i], elapsedTime / duration);
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 정확한 회전 보정
        for (int i = 0; i < valve1.Length; i++)
        {
            valve1[i].transform.localRotation = targetRotations[i];
        }
    }

    // 🔹 문 애니메이션 처리 (부드러운 Slerp)
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
}
