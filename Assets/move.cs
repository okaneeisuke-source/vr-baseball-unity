using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHeightController : MonoBehaviour
{
    public Transform player;       // カメラ（HMD）のTransform
    public GameObject Sphere;    // 対象の障害物
    public float triggerDistance = 1.0f; // Z軸での距離
    public float newHeight = 5.0f;       // 変更後の高さ

    private bool heightChanged = false;

    void Update()
    {
        if (heightChanged) return; // 1回だけ変更したい場合

        // Z軸方向の距離を計算（障害物から見たプレイヤー）
        float zDistance = player.position.z - Sphere.transform.position.z;

        if (zDistance >= -triggerDistance && zDistance <= triggerDistance)
        {
            // 高さを変更
            Vector3 pos = Sphere.transform.position;
            pos.y = newHeight;
            Sphere.transform.position = pos;

            heightChanged = true; // 1回だけ変更する場合
        }
    }
}