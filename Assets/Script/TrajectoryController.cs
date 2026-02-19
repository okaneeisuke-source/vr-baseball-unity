using UnityEngine;
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
    }*/

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
}