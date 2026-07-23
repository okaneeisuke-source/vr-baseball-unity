using UnityEngine;

public class RFootMarkerControler : MonoBehaviour
{
    [SerializeField] private GameObject Target;

    public bool isTargeted = false;

    private TargetFeedbackController targetFeedbackController;

    private void Start()
    {
        isTargeted = false;

        if (Target != null)
        {
            targetFeedbackController =
                Target.GetComponent<TargetFeedbackController>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != Target)
        {
            return;
        }

        // 同じ進入で複数回評価することを防ぐ
        if (isTargeted)
        {
            return;
        }

        isTargeted = true;

        Debug.Log("Right Foot Targeted");

        if (targetFeedbackController != null)
        {
            targetFeedbackController.EvaluateFootPosition(
                transform.position
            );
        }
        else
        {
            Debug.LogWarning(
                "TargetFeedbackControllerがTargetに設定されていません。"
            );
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Target)
        {
            isTargeted = false;
        }
    }
}