using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class StroopDataRecorder : MonoBehaviour
{
    [SerializeField] private ExperimentRecorder baseRecorder;
    [SerializeField] private string outputDirectory = "StroopData"; // 出力ディレクトリ
    
    private StringBuilder csvData = new StringBuilder();
    private List<StroopTrialData> recordedData = new List<StroopTrialData>();

    /// <summary>
    /// 記録初期化
    /// </summary>
    public void InitializeRecording()
    {
        csvData.Clear();
        recordedData.Clear();
        
        // CSVヘッダー定義
        string headerLine = "SetIndex,TrialIndex,Character,CorrectColor,SpokenColor,IsCorrect," +
                           "HeadPosX,HeadPosY,HeadPosZ,FootPosX,FootPosY,FootPosZ," +
                           "HeadAngleX,HeadAngleY,HeadAngleZ," +
                           "GazeScreenX,GazeScreenY,ReactionTime";
        
        csvData.AppendLine(headerLine);
        Debug.Log("Recording initialized. Headers added.");
    }

    /// <summary>
    /// 試行データをログに追加
    /// </summary>
    public void LogData(StroopTrialData trialData)
    {
        recordedData.Add(trialData);
        
        // CSVデータ追加
        string dataLine = string.Format(
            "{0},{1},{2},{3},{4},{5}," +
            "{6:F3},{7:F3},{8:F3},{9:F3},{10:F3},{11:F3}," +
            "{12:F1},{13:F1},{14:F1}," +
            "{15:F2},{16:F2},{17:F3}",
            trialData.SetIndex,
            trialData.TrialIndex,
            trialData.Character,
            trialData.CorrectColor,
            trialData.SpokenColor,
            trialData.IsCorrect ? "1" : "0",
            trialData.HeadPosition.x,
            trialData.HeadPosition.y,
            trialData.HeadPosition.z,
            trialData.FootPosition.x,
            trialData.FootPosition.y,
            trialData.FootPosition.z,
            trialData.HeadEulerAngles.x,
            trialData.HeadEulerAngles.y,
            trialData.HeadEulerAngles.z,
            trialData.GazeScreenIntersection.x,
            trialData.GazeScreenIntersection.y,
            trialData.ReactionTime
        );
        
        csvData.AppendLine(dataLine);
    }

    /// <summary>
    /// CSVファイルに保存
    /// </summary>
    public void SaveToCSV(List<StroopTrialData> trialDataList, int participantId)
    {
        try
        {
            // ディレクトリ作成
            string persistentPath = Path.Combine(Application.persistentDataPath, outputDirectory);
            if (!Directory.Exists(persistentPath))
            {
                Directory.CreateDirectory(persistentPath);
            }

            // ファイル名（参加者ID + タイムスタンプ）
            string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = $"StroopData_ID{participantId}_{timestamp}.csv";
            string filePath = Path.Combine(persistentPath, fileName);

            // CSVファイル書き込み
            File.WriteAllText(filePath, csvData.ToString(), Encoding.UTF8);
            
            Debug.Log("CSV saved successfully to: " + filePath);
            Debug.Log($"Total trials recorded: {recordedData.Count}");

            // 統計情報をコンソール出力
            PrintStatistics(trialDataList);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to save CSV: " + ex.Message);
        }
    }

    /// <summary>
    /// 統計情報を計算して出力
    /// </summary>
    private void PrintStatistics(List<StroopTrialData> trialDataList)
    {
        if (trialDataList.Count == 0)
        {
            Debug.LogWarning("No trial data to calculate statistics.");
            return;
        }

        int correctCount = 0;
        float totalReactionTime = 0f;
        float minReactionTime = float.MaxValue;
        float maxReactionTime = float.MinValue;

        foreach (StroopTrialData trial in trialDataList)
        {
            if (trial.IsCorrect)
                correctCount++;

            totalReactionTime += trial.ReactionTime;
            minReactionTime = Mathf.Min(minReactionTime, trial.ReactionTime);
            maxReactionTime = Mathf.Max(maxReactionTime, trial.ReactionTime);
        }

        float accuracy = (float)correctCount / trialDataList.Count * 100f;
        float avgReactionTime = totalReactionTime / trialDataList.Count;

        Debug.Log("========== Stroop Task Statistics ==========");
        Debug.Log($"Total Trials: {trialDataList.Count}");
        Debug.Log($"Correct Answers: {correctCount}");
        Debug.Log($"Accuracy: {accuracy:F1}%");
        Debug.Log($"Average Reaction Time: {avgReactionTime:F3}s");
        Debug.Log($"Min Reaction Time: {minReactionTime:F3}s");
        Debug.Log($"Max Reaction Time: {maxReactionTime:F3}s");
        Debug.Log("=============================================");
    }

    /// <summary>
    /// 記録済みデータ取得
    /// </summary>
    public List<StroopTrialData> GetRecordedData()
    {
        return recordedData;
    }
}