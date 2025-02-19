using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clockss : MonoBehaviour
{
    public Transform hours, minutes, seconds;
    public float normalSpeed = 100f; // õõ�� ȸ���ϴ� �ӵ�
    public float returnSpeed = 5000f; // ������ ������ ȸ���ϴ� �ӵ�
    public bool isReturning = false; // Returns �������� Ȯ���ϴ� ����

    private static Clockss instance; // �̱��� �ν��Ͻ�

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            Debug.Log("�ð� �ı� ����");
        }
        else
        {
            Destroy(gameObject); // �ߺ� ���� ����
            Debug.Log("�ߺ��� �ð� ������Ʈ ����");
        }
    }

    private void Update()
    {
        if (isReturning)
        {
            Returns();
        }
        else
        {
            MoveClockForward();
        }
    }

    private void MoveClockForward()
    {
        seconds.Rotate(0, 0, normalSpeed * Time.deltaTime);
        minutes.Rotate(0, 0, normalSpeed / 60 * Time.deltaTime);
        hours.Rotate(0, 0, normalSpeed / 360 * Time.deltaTime);
    }

    private void Returns()
    {
        seconds.Rotate(0, 0, -returnSpeed * Time.deltaTime);
        minutes.Rotate(0, 0, -returnSpeed / 60 * Time.deltaTime);
        hours.Rotate(0, 0, -returnSpeed / 360 * Time.deltaTime);
    }
}
