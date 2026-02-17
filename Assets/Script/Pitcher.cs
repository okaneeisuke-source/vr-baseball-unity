using UnityEngine;

public class Pitcher : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform releasePoint;
    public Transform target;   
    public float throwForce = 1000f;
    public float flightTime = 0.3f;

    public void ThrowBall()
    {
        GameObject ball = Instantiate(ballPrefab, releasePoint.position, Quaternion.identity);
        Rigidbody rb = ball.GetComponent<Rigidbody>();

        Vector3 toTarget = target.position - releasePoint.position;

        Vector3 velocity = toTarget / flightTime;
        velocity.y += 0.5f * Mathf.Abs(Physics.gravity.y) * flightTime;

        rb.velocity = velocity;
    }
}