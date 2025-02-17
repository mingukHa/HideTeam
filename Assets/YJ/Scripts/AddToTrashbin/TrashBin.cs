using UnityEngine;
using System.Collections;

public class TrashBin : MonoBehaviour
{
    public Transform trashBinRid; // Trashbin�� ȸ�� ���� Transform
    public GameObject[] trashObjects; // Trash ������Ʈ 4��
    private float rotationDuration = 1.5f; // ȸ���� �Ϸ�Ǵ� �ð�
    public BoxCollider boxCollider;
    private bool isMessUpTriggered = false; // �ߺ� ���� ����
    public ReturnManager returnManager;
    // PlayerController���� EŰ ������ �۵�
    //public ScreenshotManager screenshotManager;
    public void MessUpTrashBin()
    {
        if (!isMessUpTriggered)
        {
            isMessUpTriggered = true;
            StartCoroutine(RotateTrashBinRid());
            Invoke("AddRigidbodyToTrash", 2f);
            boxCollider.size = new Vector3(0.1f, 0.1f, 0.1f);
            // û�Һ� ȣ�� �̺�Ʈ
            returnManager.StartCoroutine(returnManager.SaveAllNPCData(2f));
            ScreenshotManager.Instance.CaptureScreenshot();
            EventManager.Trigger(EventManager.GameEventType.Garbage);
        }
    }

    // �������� �Ѳ� ����
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

    // ����������� Rigidbody ����
    private void AddRigidbodyToTrash()
    {

        foreach (GameObject trash in trashObjects)
        {
            if (trash.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = trash.AddComponent<Rigidbody>();
                rb.useGravity = true;
                rb.mass = 0.2f; // ���� ���� ����
                rb.AddTorque(Vector3.up, ForceMode.Impulse); // �����Ⱑ ���� �ڱ�ħ
            }
        }
    }
}
