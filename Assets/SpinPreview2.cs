using UnityEngine;

public class SpinVisualizer2 : MonoBehaviour
{
    [Header("References")]
    public Transform miniBall2;
    public LineRenderer axisLine2;

    [Header("Spin Settings")]
    public float rpm = 2200f;
    public Vector3 fakeAxis = new Vector3(1.0f, 0.0f, 0.0f);

    [Header("Visual Settings")]
    public float axisLength = 0.1f;

    void Start()
    {
       //fakeAxis = fakeAxis.normalized;

    if (axisLine2 != null)
    {
        axisLine2.positionCount = 2;
        axisLine2.useWorldSpace = false;

        axisLine2.startWidth = 0.02f;
        axisLine2.endWidth = 0.02f;
    }
    }

    void Update()
    {
        if (miniBall2 == null) return;

        float degreesPerSecond = rpm * 6f;

        // ミニボール回転
        miniBall2.Rotate(fakeAxis * degreesPerSecond * Time.deltaTime, Space.Self);

        // 軸ライン描画
        if (axisLine2 != null)
        {
            axisLine2.SetPosition(0, -fakeAxis * axisLength);
            axisLine2.SetPosition(1,  fakeAxis * axisLength);
        }
    }
}