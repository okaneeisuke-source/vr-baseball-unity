using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab; // 障害物プレハブ
    public float distance = 2.0f;     // カメラからの距離

    private GameObject currentObstacle;

    void Start()
    {
        SpawnObstacleInFront();
    }

    void SpawnObstacleInFront()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        // カメラの前方に配置
        Vector3 spawnPos = cam.transform.position + cam.transform.forward * distance;
        spawnPos.y = 0; // 床に置く場合

        currentObstacle = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);

        // 障害物の向きをカメラの向きに合わせる場合
        currentObstacle.transform.rotation = Quaternion.LookRotation(cam.transform.forward);
    }
}