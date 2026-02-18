using UnityEngine;
using System.Collections.Generic;

public class TrajectoryController : MonoBehaviour
{
    public Transform ball;

    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();
    private bool stopTracking = false;
    public void SetBall(Transform newBall)
{
    ball = newBall;

    points.Clear();
    lineRenderer.positionCount = 0;
    stopTracking = false;

    Debug.Log("新しいボールを追跡開始");
}

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 0;
        if (lineRenderer == null)
{
    Debug.LogError("LineRendererが取得できていません！");
}
    }
   

    void Update()
    {
        if (ball == null || stopTracking) return;

        AddPoint(ball.position);

          if (ball == null)
    {
        Debug.LogError("Ballが未設定です！");
        return;
        
    }

    if (stopTracking) return;

    AddPoint(ball.position);
    }

    void AddPoint(Vector3 newPoint)
    {
        points.Add(newPoint);

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, newPoint);
    }

    void OnEnable()
    {
        BallController.OnBallHit += HandleBallHit;
    }

    void OnDisable()
    {
        BallController.OnBallHit -= HandleBallHit;
    }

    void HandleBallHit(Transform hitBall)
    {
        if (hitBall == ball)
        {
            stopTracking = true;
        }
    }
}
