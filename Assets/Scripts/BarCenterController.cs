using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarCenterController : MonoBehaviour
{
    public Transform head;
    public MeshRenderer meshRenderer;

    void Update()
    {
        if (meshRenderer.enabled)
        {
            if (head.position.z > 0.3f)
            {
                DisableRenderer();
            }
        }
        else
        {
            if (head.position.z < 0)
            {
                ShowRenderer();
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            ShowRenderer();
        }
    }

    // バーを消す
    public void DisableRenderer()
    {
        meshRenderer.enabled = false;
    }

    // バーを表示する
    public void ShowRenderer()
    {
        meshRenderer.enabled = true;
    }
}
