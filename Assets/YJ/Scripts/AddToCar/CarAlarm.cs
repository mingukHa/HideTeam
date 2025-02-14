using UnityEngine;
using System.Collections;

public class CarAlarm : MonoBehaviour
{
    public Light frontLeftHeadLamp;  // 전방 왼쪽 헤드 램프
    public Light frontRightHeadLamp; // 전방 오른쪽 헤드 램프
    public Light rearLeftHeadLamp; // 후방 왼쪽 헤드 램프
    public Light rearRightHeadLamp; // 후방 오른쪽 헤드 램프
    public ReturnManager returnManager;
    [SerializeField] GameObject SoundPos = null;
    private bool isAlarmActive = false;
    
    public void ActivateAlarm()
    {
        if (!isAlarmActive)
        {
            SoundManager.instance.SFXPlay("CarKickSound", this.gameObject);
            SoundManager.instance.SFXPlay("CarKickFootSound", SoundPos);
            returnManager.StartCoroutine(returnManager.SaveAllNPCData(4f));
            StartCoroutine(BlinkHeadLamps());
        }
    }
    public void CarKick()
    {
        
        Debug.Log("차 발 사운드");
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
        if (frontLeftHeadLamp != null) frontLeftHeadLamp.enabled = state;
        if (frontRightHeadLamp != null) frontRightHeadLamp.enabled = state;
        if (rearLeftHeadLamp != null) rearLeftHeadLamp.enabled = state;
        if (rearRightHeadLamp != null) rearRightHeadLamp.enabled = state;
    }
}
