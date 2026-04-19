using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Locomotion;
using UnityEngine;

public class PassthroughManager : MonoBehaviour
{
    // Start is called before the first frame update
    public OVRPassthroughLayer passthroughLayer;
    public Camera cam;
    [SerializeField] public GameObject Floor, Ceiling, LeftWall, RightWall, BackWall, FrontWall, LeftShose, RightShose;

    [SerializeField] private MeshRenderer rightShoeRenderer, leftShoeRenderer;
    void Start()
    {
            Floor.SetActive(false);
            Ceiling.SetActive(false);
            LeftWall.SetActive(false);
            RightWall.SetActive(false);
            BackWall.SetActive(false);
            FrontWall.SetActive(false);
            // LeftShose.SetActive(false);
            // RightShose.SetActive(false);
            leftShoeRenderer.enabled = false;
            rightShoeRenderer.enabled = false;
        }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            passthroughLayer.textureOpacity = 0f;
            cam.clearFlags = CameraClearFlags.Skybox;
            Floor.SetActive(true);
            Ceiling.SetActive(true);
            LeftWall.SetActive(true);
            RightWall.SetActive(true);
            BackWall.SetActive(true);
            FrontWall.SetActive(true);
            // LeftShose.SetActive(true);
            // RightShose.SetActive(true);
            leftShoeRenderer.enabled = true;
            rightShoeRenderer.enabled = true;

        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            passthroughLayer.textureOpacity = 1.0f;
            cam.clearFlags = CameraClearFlags.SolidColor;
             Floor.SetActive(false);
            Ceiling.SetActive(false);
            LeftWall.SetActive(false);
            RightWall.SetActive(false);
            BackWall.SetActive(false);
            FrontWall.SetActive(false);  
            // LeftShose.SetActive(false);
            // RightShose.SetActive(false);
            leftShoeRenderer.enabled = false;
            rightShoeRenderer.enabled = false;

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            passthroughLayer.textureOpacity = passthroughLayer.textureOpacity + 0.1f;

        }
    }
}
