using UnityEngine;

public class BallMoment : MonoBehaviour
{
    private Rigidbody rb;

    [Header("カーブ設定")]
    public float curveAcceleration = 0.01f;   // 横加速度（Inspectorで調整）
    private bool isCurve = false;
    private Vector3 curveDirection;
    public bool IsCurve => isCurve;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // ← Pitcherから呼ぶだけでOK
    public void SetCurve()
    {
        isCurve = true;

        // 左方向に曲げる（ワールド基準ではなくボールの右を基準）
        curveDirection = -transform.right;
    }

    void FixedUpdate()
    {
        if (isCurve)
        {
            rb.AddForce((curveDirection *0.13f)* curveAcceleration, ForceMode.Acceleration);
        }
    }
}