using UnityEngine;
using System.Collections;

public class CarAlarm : MonoBehaviour
{
    public Light frontLeftHeadLamp;  // ���� ���� ��� ����
    public Light frontRightHeadLamp; // ���� ������ ��� ����
    public Light rearLeftHeadLamp; // �Ĺ� ���� ��� ����
    public Light rearRightHeadLamp; // �Ĺ� ������ ��� ����
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
        
        Debug.Log("�� �� ����");
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
        if (frontLeftHeadLamp != null) frontLeftHeadLamp.enabled = state;
        if (frontRightHeadLamp != null) frontRightHeadLamp.enabled = state;
        if (rearLeftHeadLamp != null) rearLeftHeadLamp.enabled = state;
        if (rearRightHeadLamp != null) rearRightHeadLamp.enabled = state;
    }
}
