using UnityEngine;

[ExecuteAlways]
public class TargetCircleLines : MonoBehaviour
{
    [Header("円の半径")]
    [Min(0.001f)]
    public float innerRadius = 0.1f;

    [Min(0.001f)]
    public float outerRadius = 0.2f;

    [Header("線の設定")]
    [Min(3)]
    public int segments = 100;

    [Min(0.001f)]
    public float lineWidth = 0.01f;

    public Color lineColor = Color.white;

    [Tooltip("オブジェクト表面との重なりを防ぐための距離")]
    public float surfaceOffset = -0.005f;

    private LineRenderer innerLine;
    private LineRenderer outerLine;

    private void OnEnable()
    {
        CreateLines();
        UpdateLines();
    }

    private void OnValidate()
    {
        CreateLines();
        UpdateLines();
    }

    private void CreateLines()
    {
        innerLine = GetOrCreateLine("InnerCircle");
        outerLine = GetOrCreateLine("OuterCircle");
    }

    private LineRenderer GetOrCreateLine(string objectName)
    {
        Transform child = transform.Find(objectName);

        if (child == null)
        {
            GameObject lineObject = new GameObject(objectName);
            lineObject.transform.SetParent(transform);
            lineObject.transform.localPosition = Vector3.zero;
            lineObject.transform.localRotation = Quaternion.identity;
            lineObject.transform.localScale = Vector3.one;

            child = lineObject.transform;
        }

        LineRenderer line = child.GetComponent<LineRenderer>();

        if (line == null)
        {
            line = child.gameObject.AddComponent<LineRenderer>();
        }

        line.useWorldSpace = false;
        line.loop = true;

        Shader shader = Shader.Find("Sprites/Default");

        if (shader != null && line.sharedMaterial == null)
        {
            line.sharedMaterial = new Material(shader);
        }

        return line;
    }

    private void UpdateLines()
    {
        if (innerLine == null || outerLine == null)
        {
            return;
        }

        DrawCircle(innerLine, innerRadius);
        DrawCircle(outerLine, outerRadius);
    }

    private void DrawCircle(LineRenderer line, float radius)
    {
        int pointCount = Mathf.Max(segments, 3);

        line.positionCount = pointCount;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.startColor = lineColor;
        line.endColor = lineColor;

        for (int i = 0; i < pointCount; i++)
        {
            float angle = i * Mathf.PI * 2f / pointCount;

            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            // ターゲットのローカルXY平面上に円を描く
            line.SetPosition(i, new Vector3(x, y, surfaceOffset));
        }
    }
}