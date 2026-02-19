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
          TrajectoryController tc = FindObjectOfType<TrajectoryController>();
    if (tc != null)
    {
        BallMoment bm = GetComponent<BallMoment>();
        bool isCurve = bm != null && bm.IsCurve;

    
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