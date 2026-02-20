/*using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TrajectoryController : MonoBehaviour
{
    public Transform ball;
    public float trajectoryVisibleTime = 3f;

    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();

    private bool isTracking = false; 

    private Material baseMaterial;  
    public Color straightColor = Color.white;
    public Color curveColor = Color.red;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        isTracking = false;
        baseMaterial = lineRenderer.material;
    }

    void Update()
    {
       // if (!isTracking) return;   // ← これが重要
      if (!isTracking || ball == null) return;

        points.Add(ball.position);

       lineRenderer.positionCount = points.Count;
       lineRenderer.SetPositions(points.ToArray());
    }
    /* public void SetBall(Transform newBall)
    {
        ball = newBall;
        points.Clear();
        lineRenderer.positionCount = 0;
        isTracking = true;
    }
*/
/*
    public void SetBall(Transform newBall, bool isCurve)
{
    ball = newBall;
    points.Clear();
    lineRenderer.positionCount = 0;
    isTracking = true;
     
     lineRenderer.material = new Material(baseMaterial);
    if (isCurve)
    {
        lineRenderer.material.color = curveColor;
    }
    else
    {
        lineRenderer.material.color = straightColor;
    }
}

    public void StopTracking()
    {
        if (!isTracking) return;

        isTracking = false;

        StartCoroutine(HideTrajectoryAfterTime());
    }

    IEnumerator HideTrajectoryAfterTime()
    {
        yield return new WaitForSeconds(trajectoryVisibleTime);

        lineRenderer.positionCount = 0;
        points.Clear();
    }
      void OnEnable()
    {
        BallController.OnBallHit += HandleBallHit;
    }

    void OnDisable()
    {
        BallController.OnBallHit -= HandleBallHit;
    }

    private void HandleBallHit(Transform hitBall)
    {
            StopTracking();
    }
}*/
using UnityEngine;
using System.Collections.Generic;

public class TrajectoryController : MonoBehaviour
{
    [Header("LineRenderer 設定")]
    public LineRenderer baseLineRenderer; // 元になる LineRenderer (Inspectorで設定)
    public Color straightColor = Color.white;
    public Color curveColor = Color.red;

    private Material baseMaterial;

    // 1球ごとの軌道情報を保持するクラス
    private class TrajectoryData
    {
        public Transform ball;
        public List<Vector3> points = new List<Vector3>();
        public LineRenderer lineRenderer;
    }

    private List<TrajectoryData> trajectories = new List<TrajectoryData>();

    void Start()
    {
        if (baseLineRenderer == null)
        {
            Debug.LogError("Base LineRenderer が設定されていません！");
            return;
        }

        baseMaterial = baseLineRenderer.material;
        baseLineRenderer.gameObject.SetActive(false); // 元のLineRendererは非表示にして複製で使用
    }

    void Update()
    {
        foreach (var data in trajectories)
        {
            if (data.ball == null) continue;

            data.points.Add(data.ball.position);
            data.lineRenderer.positionCount = data.points.Count;
            data.lineRenderer.SetPositions(data.points.ToArray());
        }
    }

    /// <summary>
    /// 新しい球の軌道を追加
    /// </summary>
    /// <param name="newBall">軌道を描くボール</param>
    /// <param name="isCurve">曲がるボールかどうか</param>
    public void SetBall(Transform newBall, bool isCurve)
    {
        if (newBall == null)
        {
            Debug.LogWarning("SetBall に null が渡されました");
            return;
        }

        // 元のLineRendererをコピーして使用
        LineRenderer newLR = Instantiate(baseLineRenderer, transform);
        newLR.gameObject.SetActive(true);
        newLR.positionCount = 0;
        newLR.material = new Material(baseMaterial);
        newLR.material.color = isCurve ? curveColor : straightColor;

        TrajectoryData data = new TrajectoryData
        {
            ball = newBall,
            points = new List<Vector3>(),
            lineRenderer = newLR
        };

        trajectories.Add(data);
    }
}