using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActionSpaceController : MonoBehaviour
{
    public Timer timer;

    private bool userInActionSpace;

    void FixedUpdate()
    {
        if(userInActionSpace)
        {
            timer.MeasureTime();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "CenterEyeAnchor")
        {
            userInActionSpace = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.name == "CenterEyeAnchor")
        {
            userInActionSpace = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.name == "CenterEyeAnchor")
        {
            userInActionSpace = false;
        }
    }
}

