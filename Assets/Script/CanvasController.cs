/*using UnityEngine;
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
}*/


using UnityEngine;
using System.Collections;

public class CanvasController : MonoBehaviour
{
    public GameObject canvas1; // 1球目用
    public GameObject canvas2; // 2球目用
    public float delay = 1f; // 表示前の遅延
    public float visibleDuration = 3f; // 表示時間

    void Start()
    {
        if (canvas1 != null) canvas1.SetActive(false);
        if (canvas2 != null) canvas2.SetActive(false);
    }

    public void ShowCanvas(int pitchNumber)
    {
        GameObject canvasToShow = null;

        if (pitchNumber == 1) canvasToShow = canvas1;
        else if (pitchNumber == 2) canvasToShow = canvas2;
        else return;

        StartCoroutine(ShowCanvasCoroutine(canvasToShow));
    }

    private IEnumerator ShowCanvasCoroutine(GameObject canvas)
    {
        yield return new WaitForSeconds(delay);
        canvas.SetActive(true);

        yield return new WaitForSeconds(visibleDuration);
        canvas.SetActive(false);
    }
}