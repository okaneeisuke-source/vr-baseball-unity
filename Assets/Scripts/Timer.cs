using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Timer : MonoBehaviour
{
    public float ActionTime;

    public void MeasureTime()
    {
        ActionTime += Time.deltaTime;
    }

    public void Initialize()
    {
        ActionTime = 0;
    }
    
    public float GetTime()
    {
        return ActionTime;
    }
}
