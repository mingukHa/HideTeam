using UnityEngine;
using System.Collections;

public class TrashBin : MonoBehaviour
{
    public Transform trashBinLid; // �������� �Ѳ�
    public GameObject[] trashObjects; // Trash ������Ʈ 4��
    private float lidRotationDuration = 1.5f; // �Ѳ� ȸ���� �Ϸ�Ǵ� �ð�

    private bool isMessUpTriggered = false; // �ߺ� ���� ����

    // PlayerController���� EŰ ������ �۵�
    public void MessUpTrashBin()
    {
        if (!isMessUpTriggered)
        {
            isMessUpTriggered = true;
            StartCoroutine(RotateTrashBinRid());
            AddRigidbodyToTrash();

            // û�Һ� ȣ�� �̺�Ʈ
            EventManager.Trigger(EventManager.GameEventType.Garbage);
        }
    }

    // �������� �Ѳ� ����
    private IEnumerator RotateTrashBinRid()
    {
        Quaternion startRotation = trashBinLid.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(-90f, 0f, 0f);

        float elapsedTime = 0f;

        while (elapsedTime < lidRotationDuration)
        {
            trashBinLid.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / lidRotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        trashBinLid.rotation = targetRotation;
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
