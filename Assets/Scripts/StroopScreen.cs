using UnityEngine;
using UnityEngine.UI;

public class StroopScreen : MonoBehaviour
{
    [SerializeField] private Canvas screenCanvas;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private float angleOffset = 25f; // 下方位置用オフセット（度）
    [SerializeField] private Color screenColor = Color.white;
    [SerializeField] private float transparency = 0.8f;
    [SerializeField] private float distance = 2f; // スクリーン距離（m）
    [SerializeField] private Vector2 screenSize = new Vector2(1024f, 768f); // スクリーンサイズ（ピクセル）
    [SerializeField] private float eyeLevelHeight = 1.6f; // 目線の高さ（m）
    [SerializeField] private float downwardOffsetDistance = 1.5f; // 下方オフセット距離

    private CanvasGroup canvasGroup;
    private RectTransform canvasRectTransform;
    private bool isActive = false;

    void Start()
    {
        canvasRectTransform = screenCanvas.GetComponent<RectTransform>();
        canvasGroup = screenCanvas.GetComponent<CanvasGroup>();
        
        if (canvasGroup == null)
        {
            canvasGroup = screenCanvas.gameObject.AddComponent<CanvasGroup>();
        }
        
        if (backgroundImage != null)
        {
            backgroundImage.color = screenColor;
        }
        
        UpdateScreen();
    }

    /// <summary>
    /// スクリーン位置を設定（目線高さまたは25°下方）
    /// </summary>
    public void SetPosition(bool eyeLevel)
    {
        if (eyeLevel)
        {
            // 目線高さ（視線と水平）
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.position = new Vector3(0, eyeLevelHeight, distance);
        }
        else
        {
            // 足元高さ（視線から25°下方）
            transform.rotation = Quaternion.Euler(-angleOffset, 0, 0);
            transform.position = new Vector3(0, eyeLevelHeight - downwardOffsetDistance, distance);
        }
    }

    /// <summary>
    /// スクリーン表示/非表示
    /// </summary>
    public void SetActive(bool active)
    {
        isActive = active;
        screenCanvas.enabled = active;
        canvasGroup.alpha = active ? transparency : 0;
    }

    /// <summary>
    /// スクリーン設定更新
    /// </summary>
    private void UpdateScreen()
    {
        // 透明度設定
        canvasGroup.alpha = transparency;
        
        // サイズ設定
        if (canvasRectTransform != null)
        {
            canvasRectTransform.sizeDelta = screenSize;
        }
        
        // 背景色設定
        if (backgroundImage != null)
        {
            backgroundImage.color = new Color(screenColor.r, screenColor.g, screenColor.b, transparency);
        }
    }

    /// <summary>
    /// 透明度変更
    /// </summary>
    public void SetTransparency(float value)
    {
        transparency = Mathf.Clamp01(value);
        canvasGroup.alpha = transparency;
    }

    /// <summary>
    /// スクリーンサイズ変更
    /// </summary>
    public void SetSize(Vector2 newSize)
    {
        screenSize = newSize;
        if (canvasRectTransform != null)
        {
            canvasRectTransform.sizeDelta = screenSize;
        }
    }

    /// <summary>
    /// スクリーン距離変更
    /// </summary>
    public void SetDistance(float newDistance)
    {
        distance = newDistance;
        transform.position = new Vector3(transform.position.x, transform.position.y, distance);
    }

    /// <summary>
    /// スクリーン色変更
    /// </summary>
    public void SetScreenColor(Color newColor)
    {
        screenColor = newColor;
        if (backgroundImage != null)
        {
            backgroundImage.color = new Color(screenColor.r, screenColor.g, screenColor.b, transparency);
        }
    }

    public bool IsActive => isActive;
    public float Distance => distance;
    public float Transparency => transparency;
}