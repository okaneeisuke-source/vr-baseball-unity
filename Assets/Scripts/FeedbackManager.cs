using System.Collections;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    [Header("Vibration Controller")]
    [SerializeField]
    private OVRInput.Controller vibrationController =
        OVRInput.Controller.RTouch;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip excellentSound;
    [SerializeField] private AudioClip goodSound;
    [SerializeField] private AudioClip farSound;
    [SerializeField] private AudioClip outsideSound;

    [Header("Audio Volume")]
    [SerializeField, Range(0f, 1f)]
    private float audioVolume = 1f;

    private Coroutine vibrationCoroutine;

    private void Awake()
    {
        // Inspectorで未設定の場合、同じオブジェクトから取得
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource == null)
        {
            Debug.LogWarning(
                "FeedbackManagerにAudioSourceが設定されていません。"
            );
        }
    }

    public void PlayExcellentVibration()
    {
        StartVibration(
            frequency: 1.0f,
            amplitude: 1.0f,
            duration: 0.25f
        );

        PlaySound(excellentSound, "とても良い");
    }

    public void PlayGoodVibration()
    {
        StartVibration(
            frequency: 0.7f,
            amplitude: 0.6f,
            duration: 0.18f
        );

        PlaySound(goodSound, "良い");
    }

    public void PlayFarVibration()
    {
        StartVibration(
            frequency: 0.4f,
            amplitude: 0.3f,
            duration: 0.10f
        );

        PlaySound(farSound, "中心から遠い");
    }

    public void PlayOutsideVibration()
    {
        StartVibration(
            frequency: 0.2f,
            amplitude: 0.15f,
            duration: 0.08f
        );

        PlaySound(outsideSound, "ターゲットの外");
    }

    private void PlaySound(
        AudioClip audioClip,
        string evaluationName)
    {
        if (audioSource == null)
        {
            Debug.LogWarning(
                $"AudioSourceがないため、音を再生できません。評価={evaluationName}"
            );
            return;
        }

        if (audioClip == null)
        {
            Debug.LogWarning(
                $"AudioClipが設定されていません。評価={evaluationName}"
            );
            return;
        }

        // 前の音を止めてから、新しい音を再生
        audioSource.Stop();
        audioSource.PlayOneShot(audioClip, audioVolume);

        Debug.Log(
            $"[音再生] 評価={evaluationName}, " +
            $"AudioClip={audioClip.name}, " +
            $"音量={audioVolume:F2}"
        );
    }

    private void StartVibration(
        float frequency,
        float amplitude,
        float duration)
    {
        if (vibrationCoroutine != null)
        {
            StopCoroutine(vibrationCoroutine);

            // 前の振動も止める
            StopVibration();
        }

        vibrationCoroutine = StartCoroutine(
            VibrationCoroutine(
                frequency,
                amplitude,
                duration
            )
        );
    }

    private IEnumerator VibrationCoroutine(
        float frequency,
        float amplitude,
        float duration)
    {
        OVRInput.SetControllerVibration(
            frequency,
            amplitude,
            vibrationController
        );

        Debug.Log(
            $"[振動開始] " +
            $"周波数={frequency:F2}, " +
            $"強さ={amplitude:F2}, " +
            $"時間={duration:F2}秒, " +
            $"コントローラー={vibrationController}"
        );

        yield return new WaitForSeconds(duration);

        StopVibration();
        vibrationCoroutine = null;
    }

    private void StopVibration()
    {
        OVRInput.SetControllerVibration(
            0f,
            0f,
            vibrationController
        );

        Debug.Log("[振動停止]");
    }

    private void OnDisable()
    {
        if (vibrationCoroutine != null)
        {
            StopCoroutine(vibrationCoroutine);
            vibrationCoroutine = null;
        }

        StopVibration();

        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}