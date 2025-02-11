using UnityEngine;
using System.Collections;

public class CarAlarm : MonoBehaviour
{
    public Light leftHeadLamp;  // 왼쪽 헤드 램프 (Inspector에서 지정)
    public Light rightHeadLamp; // 오른쪽 헤드 램프 (Inspector에서 지정)

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

        for (int i = 0; i < 20; i++) // 20번 깜빡임
        {
            ToggleLights(true);  // 불 켜기
            yield return new WaitForSeconds(0.5f);
            ToggleLights(false); // 불 끄기
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
