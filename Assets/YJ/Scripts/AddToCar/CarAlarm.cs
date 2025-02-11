using UnityEngine;
using System.Collections;

public class CarAlarm : MonoBehaviour
{
    public Light leftHeadLamp;  // ���� ��� ���� (Inspector���� ����)
    public Light rightHeadLamp; // ������ ��� ���� (Inspector���� ����)

    private bool isAlarmActive = false;

    public void ActivateAlarm()
    {
        if (!isAlarmActive)
        {
            StartCoroutine(BlinkHeadLamps());
        }
    }

    private IEnumerator BlinkHeadLamps()
    {
        isAlarmActive = true;

        for (int i = 0; i < 20; i++) // 20�� ������
        {
            ToggleLights(true);  // �� �ѱ�
            yield return new WaitForSeconds(0.5f);
            ToggleLights(false); // �� ����
            yield return new WaitForSeconds(0.5f);
        }

        isAlarmActive = false;
    }

    private void ToggleLights(bool state)
    {
        if (leftHeadLamp != null) leftHeadLamp.enabled = state;
        if (rightHeadLamp != null) rightHeadLamp.enabled = state;
    }
}
