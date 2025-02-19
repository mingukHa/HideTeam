using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clockss : MonoBehaviour
{
    public Transform hours, minutes, seconds;
    public float normalSpeed = 100f; // 천천히 회전하는 속도
    public float returnSpeed = 5000f; // 빠르게 역방향 회전하는 속도
    public bool isReturning = false; // Returns 상태인지 확인하는 변수

    

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
