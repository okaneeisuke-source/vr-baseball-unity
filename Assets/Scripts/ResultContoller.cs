using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class ResultContoller : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ExperimentRecorder experimentRecorder;
    [SerializeField] private GameObject targetbar;
    [SerializeField] private GameObject rightController;
    [SerializeField] private TextMeshProUGUI resultText;

    [Header("Settings")]
    [SerializeField] private float successThreshold = 0.05f; // ±0.05m
    [SerializeField] private bool hideOnStart = true;

    private CanvasGroup canvasGroup;
    private int previousId;
    private bool hasInitializedId = false;


    private struct ResultData
    {
        public int trialNumber;
        public string feedbackText;
        public bool isSuccess;
    }

    private readonly List<ResultData> results = new List<ResultData>();

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    private void Start()
    {
        
        if (experimentRecorder != null)
            {
                previousId = experimentRecorder.id;
                hasInitializedId = true;
            }

        UpdateResultText();

        if (hideOnStart)
        {
            HideCanvas();
        }
        else
        {
            ShowCanvas();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleCanvas();
        }
        CheckParticipantIdChanged();
    }


    private void CheckParticipantIdChanged()
    {
        if (experimentRecorder == null)
        {
            return;
        }

        if (!hasInitializedId)
        {
            previousId = experimentRecorder.id;
            hasInitializedId = true;
            return;
        }

        if (experimentRecorder.id != previousId)
        {
            ClearResults();
            previousId = experimentRecorder.id;

            Debug.Log($"被験者IDが変更されたため、ResultContollerの結果表示をリセットしました。現在のID: {previousId}");
        }
    }

    public void AddLatestTrialResult()
    {
        if (experimentRecorder == null)
        {
            Debug.LogWarning("ExperimentRecorderが設定されていません。");
            return;
        }

        if (targetbar == null)
        {
            Debug.LogWarning("targetbarが設定されていません。");
            return;
        }

        if (experimentRecorder.experimentData == null || experimentRecorder.experimentData.Count == 0)
        {
            Debug.LogWarning("experimentDataが空です。結果を追加できません。");
            return;
        }

        float targetY = targetbar.transform.position.y;
        float targetZ = targetbar.transform.position.z;

        ExperimentRecorder.ExperimentData evaluationData = GetEvaluationData(targetZ);

        float differenceY = evaluationData.RightControllerY - targetY;

        
        Debug.Log(
                $"[Result判定] " +
                $"targetY={targetY:F3}, targetZ={targetZ:F3}, " +
                $"RightControllerY={evaluationData.RightControllerY:F3}, " +
                $"RightControllerZ={evaluationData.RightControllerZ:F3}, " +
                $"differenceY={differenceY:F3}, " +
                $"threshold={successThreshold:F3}"
            );
            
        Debug.Log(
            $"[RightController確認] " +
            $"worldY={rightController.transform.position.y:F3}, " +
            $"localY={rightController.transform.localPosition.y:F3}, " +
            $"parentY={rightController.transform.parent.position.y:F3}"
        );
        
        Debug.Log(
            $"[TargetBar確認] " +
            $"worldY={targetbar.transform.position.y:F3}, " +
            $"localY={targetbar.transform.localPosition.y:F3}, " +
            $"parentY={targetbar.transform.parent.position.y:F3}"
        );




        string feedback;
        bool isSuccess = false;

        if (Mathf.Abs(differenceY) <= successThreshold)
        {
            feedback = "〇";
            isSuccess = true;
        }
        else if (differenceY > successThreshold)
        {
            feedback = "少し高いよ！";
        }
        else
        {
            feedback = "少し低いよ！";
        }

        // sampleNumが0開始の場合、表示は1回目からにする
        int trialNumber = experimentRecorder.sampleNum + 1;

        ResultData resultData = new ResultData
        {
            trialNumber = trialNumber,
            feedbackText = feedback,
            isSuccess = isSuccess
        };

        results.Add(resultData);
        UpdateResultText();
    }

    private ExperimentRecorder.ExperimentData GetEvaluationData(float targetZ)
    {
        List<ExperimentRecorder.ExperimentData> dataList = experimentRecorder.experimentData;

        // 基本：RightControllerZ が targetbar のZ座標に到達したフレームを探す
        for (int i = 1; i < dataList.Count; i++)
        {
            float previousZ = dataList[i - 1].RightControllerZ;
            float currentZ = dataList[i].RightControllerZ;

            // Zが小さい側から大きい側へ通過した場合
            if (previousZ < targetZ && currentZ >= targetZ)
            {
                return dataList[i];
            }

            // 念のため、逆方向に通過した場合にも対応
            if (previousZ > targetZ && currentZ <= targetZ)
            {
                return dataList[i];
            }
        }

        // 到達フレームが見つからなかった場合は、targetZに最も近いフレームを使う
        ExperimentRecorder.ExperimentData closestData = dataList[0];
        float closestDistance = Mathf.Abs(dataList[0].RightControllerZ - targetZ);

        for (int i = 1; i < dataList.Count; i++)
        {
            float distance = Mathf.Abs(dataList[i].RightControllerZ - targetZ);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestData = dataList[i];
            }
        }

        return closestData;
    }

    private void UpdateResultText()
    {
        if (resultText == null)
        {
            return;
        }

        StringBuilder builder = new StringBuilder();

        builder.AppendLine("結果");

        int successCount = 0;

        for (int i = 0; i < results.Count; i++)
        {
            builder.AppendLine($"{results[i].trialNumber}回目：{results[i].feedbackText}");

            if (results[i].isSuccess)
            {
                successCount++;
            }
        }

        builder.AppendLine();
        builder.AppendLine($"〇の数：{successCount}");

        resultText.text = builder.ToString();
    }

    private void ToggleCanvas()
    {
        if (canvasGroup.alpha > 0f)
        {
            HideCanvas();
        }
        else
        {
            ShowCanvas();
        }
    }

    private void ShowCanvas()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void HideCanvas()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void ClearResults()
    {
        results.Clear();
        UpdateResultText();
    }
}