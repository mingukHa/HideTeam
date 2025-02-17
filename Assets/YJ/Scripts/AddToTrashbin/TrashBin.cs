using UnityEngine;
using System.Collections;

public class TrashBin : MonoBehaviour
{
    public Transform trashBinRid; // Trashbin의 회전 기준 Transform
    public GameObject[] trashObjects; // Trash 오브젝트 4개
    private float rotationDuration = 1.5f; // 회전이 완료되는 시간
    public BoxCollider boxCollider;
    private bool isMessUpTriggered = false; // 중복 실행 방지
    public ReturnManager returnManager;
    // PlayerController에서 E키 누르면 작동
    //public ScreenshotManager screenshotManager;
    public void MessUpTrashBin()
    {
        if (!isMessUpTriggered)
        {
            isMessUpTriggered = true;
            StartCoroutine(RotateTrashBinRid());
            Invoke("AddRigidbodyToTrash", 2f);
            boxCollider.size = new Vector3(0.1f, 0.1f, 0.1f);
            // 청소부 호출 이벤트
            returnManager.StartCoroutine(returnManager.SaveAllNPCData(2f));
            ScreenshotManager.Instance.CaptureScreenshot();
            EventManager.Trigger(EventManager.GameEventType.Garbage);
        }
    }

    // 쓰레기통 뚜껑 열기
    private IEnumerator RotateTrashBinRid()
    {
        Quaternion startRotation = trashBinRid.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(-90f, 0f, 0f);

        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            trashBinRid.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        trashBinRid.rotation = targetRotation;
    }

    // 쓰레기봉지에 Rigidbody 부착
    private void AddRigidbodyToTrash()
    {

        foreach (GameObject trash in trashObjects)
        {
            if (trash.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = trash.AddComponent<Rigidbody>();
                rb.useGravity = true;
                rb.mass = 0.2f; // 무게 조절 가능
                rb.AddTorque(Vector3.up, ForceMode.Impulse); // 쓰레기가 위로 솟구침
            }
        }
    }
}
