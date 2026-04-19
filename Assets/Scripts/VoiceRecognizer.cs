using UnityEngine;
using System.Collections;

public class VoiceRecognizer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float listeningDuration = 3f; // 3秒間リッスン
    [SerializeField] private float confidenceThreshold = 0.5f; // 信頼度閾値
    
    private string[] keywords = { "赤", "青", "黒", "黄" };
    private float reactionTime = 0f;
    private string detectedColor = "";
    private bool isListening = false;
    private float startTime = 0f;

    void Start()
    {
        InitializeMicrophone();
    }

    /// <summary>
    /// マイク初期化（Meta Quest対応）
    /// </summary>
    private void InitializeMicrophone()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogWarning("No microphone device found!");
            return;
        }

        string micDevice = Microphone.devices[0];
        Debug.Log("Using microphone: " + micDevice);
        
        audioSource.clip = Microphone.Start(micDevice, false, 10, 44100);
        if (audioSource.clip == null)
        {
            Debug.LogError("Failed to start microphone!");
        }
    }

    /// <summary>
    /// 音声認識開始
    /// </summary>
    public void StartListening()
    {
        if (isListening) return;
        
        isListening = true;
        reactionTime = 0f;
        detectedColor = "";
        startTime = Time.time;
        
        // マイク再開
        if (Microphone.devices.Length > 0)
        {
            audioSource.clip = Microphone.Start(Microphone.devices[0], false, 10, 44100);
            audioSource.Play();
        }
        
        StartCoroutine(ListenForKeyword());
        Debug.Log("Started listening for voice input...");
    }

    /// <summary>
    /// キーワード検出コルーチン
    /// </summary>
    private IEnumerator ListenForKeyword()
    {
        float elapsedTime = 0f;
        
        while (isListening && elapsedTime < listeningDuration)
        {
            elapsedTime += Time.deltaTime;
            
            // 簡易音声認識：オーディオレベル確認
            float audioLevel = GetAudioLevel();
            
            // オーディオレベルが高い場合（音声あり）
            if (audioLevel > 0.1f)
            {
                // ダミー実装：ランダムに色を検知
                // 実際にはOculus Voice SDKやSpeech Recognition APIを使用
                if (Random.value < 0.05f)
                {
                    DetectColorVoice();
                }
            }
            
            yield return null;
        }
        
        StopListening();
    }

    /// <summary>
    /// オーディオレベル取得
    /// </summary>
    private float GetAudioLevel()
    {
        if (audioSource.clip == null || !Microphone.IsRecording(Microphone.devices[0]))
        {
            return 0f;
        }

        // 現在のマイク位置を取得
        int micPosition = Microphone.GetPosition(Microphone.devices[0]);
        
        if (micPosition == 0) return 0f;

        // オーディオサンプルを取得
        float[] samples = new float[256];
        audioSource.clip.GetData(samples, micPosition - 256);

        // RMS（二乗平均平方根）でレベル計算
        float sum = 0f;
        foreach (float sample in samples)
        {
            sum += sample * sample;
        }

        return Mathf.Sqrt(sum / samples.Length);
    }

    /// <summary>
    /// 色の音声認識（簡易実装）
    /// 実際にはOculus Voice SDKやGoogle Speech APIを使用
    /// </summary>
    private void DetectColorVoice()
    {
        // ダミー実装：ランダムに色を選択
        int randomIndex = Random.Range(0, keywords.Length);
        detectedColor = keywords[randomIndex];
        reactionTime = Time.time - startTime;
        isListening = false;
        
        Debug.Log($"Detected color: {detectedColor}, Reaction time: {reactionTime:F3}s");
    }

    /// <summary>
    /// リッスン停止
    /// </summary>
    public void StopListening()
    {
        isListening = false;
        
        if (Microphone.IsRecording(Microphone.devices[0]))
        {
            Microphone.End(Microphone.devices[0]);
        }
        
        Debug.Log("Stopped listening. Final reaction time: " + reactionTime);
    }

    /// <summary>
    /// 検出された色取得
    /// </summary>
    public string GetDetectedColor()
    {
        return detectedColor;
    }

    /// <summary>
    /// 反応時間取得
    /// </summary>
    public float GetReactionTime()
    {
        return reactionTime;
    }

    /// <summary>
    /// リッスン状態確認
    /// </summary>
    public bool IsListening => isListening;
}