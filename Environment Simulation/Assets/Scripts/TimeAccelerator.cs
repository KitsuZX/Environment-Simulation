using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAccelerator : MonoBehaviour
{

    [SerializeField, Range(0.1f, 5)] private float timeScale = 1;

    private void FixedUpdate()
    {
        Time.timeScale = timeScale;
    }

    private void Awake()
    {
        Time.timeScale = 1;
    }
}
