using UnityEngine;

public class SpinVisualizer : MonoBehaviour
{
    [Header("References")]
    public Transform miniBall;
    public LineRenderer axisLine;

    [Header("Spin Settings")]
    public float rpm = 2200f;
    public Vector3 fakeAxis = new Vector3(1.0f, 0.0f, 0.0f);

    [Header("Visual Settings")]
    public float axisLength = 0.1f;

    void Start()
    {
       //fakeAxis = fakeAxis.normalized;

    if (axisLine != null)
    {
        axisLine.positionCount = 2;
        axisLine.useWorldSpace = false;

        axisLine.startWidth = 0.02f;
        axisLine.endWidth = 0.02f;
    }
    }

    void Update()
    {
        if (miniBall == null) return;

        float degreesPerSecond = rpm * 6f;

        // ミニボール回転
        miniBall.Rotate(fakeAxis * degreesPerSecond * Time.deltaTime, Space.Self);

        // 軸ライン描画
        if (axisLine != null)
        {
            axisLine.SetPosition(0, -fakeAxis * axisLength);
            axisLine.SetPosition(1,  fakeAxis * axisLength);
        }
    }
}