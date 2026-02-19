using UnityEngine; 
public class Pitcher : MonoBehaviour 
{ 
    public GameObject ballPrefab; 
    public Transform releasePoint; 
    public Transform target; 
    public float flightTime = 0.5f; 
    private int pitchCount = 0; 

    public void ThrowBall() 
    { 
        pitchCount++; 
        GameObject ball = Instantiate(ballPrefab, releasePoint.position, 
        Quaternion.identity); 
        Rigidbody rb = ball.GetComponent<Rigidbody>(); 
        Vector3 toTarget = target.position - releasePoint.position; 
        Vector3 velocity = toTarget / flightTime; 
        TrajectoryController tc = FindObjectOfType<TrajectoryController>();
                if (tc != null)
                {
                    tc.SetBall(ball.transform, pitchCount == 2);
                }

        rb.velocity = velocity; 
        if (pitchCount == 1) 
        { 
            // ストレート 
            velocity.y += 0.7f * Mathf.Abs(Physics.gravity.y) * flightTime; 
            rb.velocity = velocity;
             }
        else if (pitchCount == 2) 
        { 
            velocity.y += 2.2f * Mathf.Abs(Physics.gravity.y) * flightTime; 
            velocity *= 0.45f;
            rb.velocity = velocity;
          BallMoment moment = ball.GetComponent<BallMoment>();

          if (moment != null)
          {
           moment.SetCurve();
           }
         } 
    } 
}