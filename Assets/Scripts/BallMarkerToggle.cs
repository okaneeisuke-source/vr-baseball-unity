
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BallMarkerToggle : MonoBehaviour
{
    private static readonly List<BallMarkerToggle> markers = new List<BallMarkerToggle>();

    private static InputDevice rightController;
    private static bool previousBButtonState = false;
    private static bool isVisible = true;

    private Renderer[] markerRenderers;

    void Awake()
    {
        markerRenderers = GetComponentsInChildren<Renderer>(true);
    }

    void OnEnable()
    {
        if (!markers.Contains(this))
        {
            markers.Add(this);
        }

        ApplyVisibleState();
    }

    void OnDisable()
    {
        markers.Remove(this);
    }

    void Update()
    {
        // Bボタンの監視は、登録されている最初の1つだけが行う
        if (markers.Count == 0 || markers[0] != this)
        {
            return;
        }

        if (!rightController.isValid)
        {
            rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }

        if (rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool isPressed))
        {
            if (isPressed && !previousBButtonState)
            {
                ToggleAllMarkers();
            }

            previousBButtonState = isPressed;
        }
    }

    private static void ToggleAllMarkers()
    {
        isVisible = !isVisible;

        foreach (BallMarkerToggle marker in markers)
        {
            marker.ApplyVisibleState();
        }
    }

    private void ApplyVisibleState()
    {
        foreach (Renderer r in markerRenderers)
        {
            r.enabled = isVisible;
        }
    }
    
}
