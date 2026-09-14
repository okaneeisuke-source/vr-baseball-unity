using UnityEngine;

public class TargetFeedbackController : MonoBehaviour
{
    [Header("Evaluation Settings")]
    [SerializeField, Range(0f, 1f)]
    private float goodThreshold = 0.1f;

    [SerializeField, Range(0f, 1f)]
    private float normalThreshold = 0.3f;

    [SerializeField]
    private FeedbackManager feedbackManager;

    public void EvaluateFootPosition(Vector3 footWorldPosition)
    {
        // 足のワールド座標をTarget基準のローカル座標に変換
        Vector3 localFootPosition =
            transform.InverseTransformPoint(footWorldPosition);

        // Unity標準Cylinderのローカル半径
        float targetRadius = 0.5f;

        // Targetの中心から足までのXZ平面上の距離
        float distanceFromCenter = new Vector2(
            localFootPosition.x,
            localFootPosition.z
        ).magnitude;

        // 0＝中心、1＝円の外周
        float normalizedDistance =
            distanceFromCenter / targetRadius;

       string feedback;

        if (normalizedDistance <= goodThreshold)
        {
            feedback = "とても良い";

            if (feedbackManager != null)
            {
                feedbackManager.PlayExcellentVibration();
            }
        }
        else if (normalizedDistance <= normalThreshold)
        {
            feedback = "良い";

            if (feedbackManager != null)
            {
                feedbackManager.PlayGoodVibration();
            }
        }
        else if (normalizedDistance <= 1f)
        {
            feedback = "中心から遠い";

            if (feedbackManager != null)
            {
                feedbackManager.PlayFarVibration();
            }
        }
        else
        {
            feedback = "ターゲットの外";

            if (feedbackManager != null)
            {
                feedbackManager.PlayOutsideVibration();
            }
        }
    }
}