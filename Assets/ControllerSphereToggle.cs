using UnityEngine;

public class ControllerSphereToggle : MonoBehaviour
{
    [Header("表示を切り替える球")]
    [SerializeField] private GameObject leftTrackingSphere;
    [SerializeField] private GameObject rightTrackingSphere;

    [Header("表示切り替えキー")]
    [SerializeField] private KeyCode toggleKey = KeyCode.C;

    private bool isVisible = true;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            isVisible = !isVisible;

            if (leftTrackingSphere != null)
            {
                leftTrackingSphere.SetActive(isVisible);
            }

            if (rightTrackingSphere != null)
            {
                rightTrackingSphere.SetActive(isVisible);
            }
        }
    }
}