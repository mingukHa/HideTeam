using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clockss : MonoBehaviour
{
    public Transform hours, minutes, seconds;
    public float rotationSpeed = 5000f; 

    void Update()
    {
        seconds.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        minutes.Rotate(0, 0, -rotationSpeed / 60 * Time.deltaTime);
        hours.Rotate(0, 0, -rotationSpeed / 360 * Time.deltaTime);
    }
}
