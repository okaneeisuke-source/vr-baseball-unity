using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StroopTaskManager : MonoBehaviour
{
    [SerializeField] private StroopScreen screen;
    [SerializeField] private StroopStimulus stimulus;
    [SerializeField] private VoiceRecognizer voiceRecognizer;
    [SerializeField] private StroopDataRecorder dataRecorder;
    [SerializeField] private WorldController worldController;
    [SerializeField] private PositionTracker positionTracker;
    [SerializeField] private ExperimentRecorder experimentRecorder;
    
    [SerializeField] private float startDistance = 4f; // 4m歩行で開始
    [SerializeField] private int sets = 5; // 5セット
    [SerializeField] private bool useEyeLevel = true; // 目線高さ使用フラグ

    private bool taskActive = false;
    private bool taskStarted = false;
    private int currentSet = 0;
    private List<StroopTrialData> trialDataList = new List<StroopTrialData>();

    void Update()
    {
        // スタートライン越えで課題開始
        if (!taskStarted && !taskActive && worldController.totalYChange >= startDistance)
        {
            taskStarted = true;
            StartCoroutine(RunTask());
        }
    }

    private IEnumerator RunTask()
    {
        taskActive = true;
        currentSet = 0;
        trialDataList.Clear();
        
        screen.SetPosition(useEyeLevel);
        screen.SetActive(true);
        dataRecorder.InitializeRecording();
        
        for (int set = 0; set < sets; set++)
        {
            currentSet = set;
            yield return StartCoroutine(RunSet(set));
        }
        
        taskActive = false;
        screen.SetActive(false);
        dataRecorder.SaveToCSV(trialDataList, experimentRecorder.id);
        
        Debug.Log("Stroop Task Completed. Sets: " + sets);
    }

    private IEnumerator RunSet(int setIndex)
    {
        stimulus.GenerateSet();
        List<(string character, Color color, string correctColor)> setData = stimulus.GetCurrentSet();
        
        for (int i = 0; i < setData.Count; i++)
        {
            var (character, color, correctColor) = setData[i];
            
            // 刺激表示開始
            stimulus.DisplayCharacter(i);
            voiceRecognizer.StartListening();
            
            float startTime = Time.time;
            float displayDuration = stimulus.GetDisplayTime();
            
            yield return new WaitForSeconds(displayDuration);
            
            // 刺激消去
            stimulus.HideCharacter();
            
            // 音声認識完了待機
            float reactionTime = voiceRecognizer.GetReactionTime();
            string spokenColor = voiceRecognizer.GetDetectedColor();
            bool isCorrect = spokenColor == correctColor;
            
            // データ記録
            Vector3 headPos = positionTracker.GetHeadPosition();
            Vector3 footPos = positionTracker.GetFootPosition();
            Vector3 headAngles = positionTracker.GetHeadEulerAngles();
            Vector2 gazePoint = positionTracker.GetGazeScreenIntersection();
            
            StroopTrialData trialData = new StroopTrialData(
                setIndex, i, character, correctColor, spokenColor, isCorrect,
                headPos, footPos, headAngles, gazePoint, reactionTime
            );
            trialDataList.Add(trialData);
            dataRecorder.LogData(trialData);
            
            yield return new WaitForSeconds(stimulus.GetBlankTime());
        }
    }

    public bool IsTaskActive => taskActive;
    public int CurrentSet => currentSet;
}

// ストループ試行データ
public class StroopTrialData
{
    public int SetIndex { get; set; }
    public int TrialIndex { get; set; }
    public string Character { get; set; }
    public string CorrectColor { get; set; }
    public string SpokenColor { get; set; }
    public bool IsCorrect { get; set; }
    public Vector3 HeadPosition { get; set; }
    public Vector3 FootPosition { get; set; }
    public Vector3 HeadEulerAngles { get; set; }
    public Vector2 GazeScreenIntersection { get; set; }
    public float ReactionTime { get; set; }

    public StroopTrialData(int setIndex, int trialIndex, string character, string correctColor,
        string spokenColor, bool isCorrect, Vector3 headPos, Vector3 footPos,
        Vector3 headAngles, Vector2 gazePoint, float reactionTime)
    {
        SetIndex = setIndex;
        TrialIndex = trialIndex;
        Character = character;
        CorrectColor = correctColor;
        SpokenColor = spokenColor;
        IsCorrect = isCorrect;
        HeadPosition = headPos;
        FootPosition = footPos;
        HeadEulerAngles = headAngles;
        GazeScreenIntersection = gazePoint;
        ReactionTime = reactionTime;
    }
}