/*using UnityEngine;
using System.Collections.Generic;


public class BallController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();
    public Transform target;
     [Header("軌道表示設定")]
    public float visibleTimeAfterHit = 1.5f;
     private bool hasHit = false;
      private Rigidbody rb;
    private Collider col;
    private MeshRenderer meshRenderer;

   

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (!hasHit)
        {
            points.Add(transform.position);
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPositions(points.ToArray());
        }
    }



private void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject == target)
    {
        HandleHit();
    }
}


void HandleHit()
{ hasHit = true;

            // 指定秒数後に消す
            Destroy(gameObject, visibleTimeAfterHit);
}
}*/
using UnityEngine;
using System;

public class BallController : MonoBehaviour
{
    public Transform target;
    public float visibleTimeAfterHit = 1.5f;

    private bool hasHit = false;

    public static event Action<Transform> OnBallHit;

    void Start()
    {
        // 生成された瞬間にTrajectoryControllerへ登録
        TrajectoryController tc = FindObjectOfType<TrajectoryController>();

        if (tc != null)
        {
            tc.SetBall(transform);
        }
        else
        {
            Debug.LogError("TrajectoryControllerが見つかりません！");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasHit && collision.transform == target)
        {
            hasHit = true;

            OnBallHit?.Invoke(transform);

            Destroy(gameObject, visibleTimeAfterHit);
        }
    }
}