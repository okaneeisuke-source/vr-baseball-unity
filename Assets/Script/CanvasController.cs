using UnityEngine;
using System.Collections;

public class CanvasController : MonoBehaviour
{
    public GameObject resultCanvas;
    public float delay = 1f;
    public float visibleDuration = 3f; 

    void OnEnable()
    {
        BallController.OnBallHit += HandleBallHit;
    }

    void OnDisable()
    {
        BallController.OnBallHit -= HandleBallHit;
    }

    void Start()
    {
        if (resultCanvas != null)
        {
            resultCanvas.SetActive(false); // 最初は非表示
        }
    }

    void HandleBallHit(Transform ball)
    {
        StartCoroutine(ShowCanvasAfterDelay());
    }

    IEnumerator ShowCanvasAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        
        if (resultCanvas != null)
        {
            resultCanvas.SetActive(true);
        }

         yield return new WaitForSeconds(visibleDuration);
           if (resultCanvas != null)
        {
            resultCanvas.SetActive(false); // 非表示
        }
   
    }
}
