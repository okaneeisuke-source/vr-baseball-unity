using UnityEngine;

public class PositionTracker : MonoBehaviour
{
    [SerializeField] private Transform headTransform; // 頭部（OVRCameraRig）
    [SerializeField] private Transform footTransform; // 足部（キャラクターベース）
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Canvas stroopCanvas; // ストループ課題表示用Canvas
    [SerializeField] private float raycastDistance = 100f;

    void Start()
    {
        // 自動検出
        if (headTransform == null)
        {
            OVRCameraRig ovrCameraRig = FindObjectOfType<OVRCameraRig>();
            if (ovrCameraRig != null)
            {
                headTransform = ovrCameraRig.trackingSpace;
                mainCamera = ovrCameraRig.GetComponent<Camera>();
            }
        }

        if (footTransform == null)
        {
            footTransform = GetComponent<Transform>();
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    /// <summary>
    /// 頭部位置取得（ワールド座標）
    /// </summary>
    public Vector3 GetHeadPosition()
    {
        return headTransform != null ? headTransform.position : Vector3.zero;
    }

    /// <summary>
    /// 足部位置取得（ワールド座標）
    /// </summary>
    public Vector3 GetFootPosition()
    {
        return footTransform != null ? footTransform.position : Vector3.zero;
    }

    /// <summary>
    /// 頭部のEuler角取得（度）
    /// </summary>
    public Vector3 GetHeadEulerAngles()
    {
        return headTransform != null ? headTransform.eulerAngles : Vector3.zero;
    }

    /// <summary>
    /// 視線がストループスクリーンと交差する点を取得（スクリーン上の2D座標）
    /// </summary>
    public Vector2 GetGazeScreenIntersection()
    {
        if (mainCamera == null || stroopCanvas == null)
        {
            return Vector2.zero;
        }

        // カメラの前方向でレイキャスト
        Ray gazeRay = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        
        // Canvas背面のレイキャスト判定
        RectTransform canvasRect = stroopCanvas.GetComponent<RectTransform>();
        Plane canvasPlane = new Plane(stroopCanvas.transform.forward, stroopCanvas.transform.position);

        if (canvasPlane.Raycast(gazeRay, out float enter))
        {
            Vector3 intersectionPoint = gazeRay.origin + gazeRay.direction * enter;
            
            // ワールド座標をCanvas上のローカル座標に変換
            Vector3 localPoint = stroopCanvas.transform.InverseTransformPoint(intersectionPoint);
            
            // RectTransformの中心を基準に上下・左右座標を計算
            float canvasWidth = canvasRect.rect.width;
            float canvasHeight = canvasRect.rect.height;
            
            float screenX = localPoint.x / canvasWidth * 100f; // -50 ～ 50 の範囲
            float screenY = localPoint.y / canvasHeight * 100f; // -50 ～ 50 の範囲
            
            return new Vector2(screenX, screenY);
        }

        return Vector2.zero;
    }

    /// <summary>
    /// 正規化された視線位置（0 ～ 1）を取得
    /// </summary>
    public Vector2 GetNormalizedGazePosition()
    {
        Vector2 gazePoint = GetGazeScreenIntersection();
        RectTransform canvasRect = stroopCanvas.GetComponent<RectTransform>();
        
        if (canvasRect == null)
        {
            return Vector2.zero;
        }

        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        float normalizedX = (gazePoint.x + canvasWidth / 2f) / canvasWidth;
        float normalizedY = (gazePoint.y + canvasHeight / 2f) / canvasHeight;

        return new Vector2(Mathf.Clamp01(normalizedX), Mathf.Clamp01(normalizedY));
    }

    /// <summary>
    /// 視線がストループスクリーン上にあるかどうか確認
    /// </summary>
    public bool IsGazeOnScreen()
    {
        Vector2 normalizedPos = GetNormalizedGazePosition();
        return normalizedPos.x >= 0 && normalizedPos.x <= 1 && normalizedPos.y >= 0 && normalizedPos.y <= 1;
    }

    /// <summary>
    /// デバッグ用：現在の位置情報を出力
    /// </summary>
    public void DebugPrintPositions()
    {
        Debug.Log("=== Position Information ===");
        Debug.Log($"Head Position: {GetHeadPosition()}");
        Debug.Log($"Foot Position: {GetFootPosition()}");
        Debug.Log($"Head Angles: {GetHeadEulerAngles()}");
        Debug.Log($"Gaze on Screen: {GetGazeScreenIntersection()}");
        Debug.Log($"Normalized Gaze: {GetNormalizedGazePosition()}");
        Debug.Log($"Is on Screen: {IsGazeOnScreen()}");
        Debug.Log("============================");
    }
}