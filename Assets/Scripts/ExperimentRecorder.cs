using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Text;
using UnityEngine;

public class ExperimentRecorder : MonoBehaviour
{
    public int id;
    public int sampleNum;
    public bool isRecording;
    public List<ExperimentData> experimentData = new List<ExperimentData>();
    private int sampleID;

    [SerializeField] private ResultContoller resultContoller;

    public struct ExperimentData
    {
        // 現状のデータが何試行目であるか
        public int sampleID;
        // 計測開始から今までの時間
        public float trialTime;
        // スタートからゴールまでの時間
        public float startToGoalTime;

        // FootMarkerの中心の座標
        public float centerOfRightFootX;
        public float centerOfRightFootY;
        public float centerOfRightFootZ;

        public float rightFootFlectionAngle;
        public float rightFootHorizontalAngle;
        //LFootMarkerの中心の座標
        public float centerOfLeftFootX;
        public float centerOfLeftFootY;
        public float centerOfLeftFootZ;

        public float leftFootFlectionAngle;
        public float leftFootHorizontalAngle;
        //頭部(両目中心)の中心の座標
        public float centerEyeX;
        public float centerEyeY;
        public float centerEyeZ;
        // 頭部の垂直方向の屈曲角度
        public float headFlexionAngle;
        // 頭部の水平方向の屈曲角度
        public float headHorizontalAngle;
        //ターゲットへの接触判定
        public bool TouchTarget;
        // WorldのY座標
        public float World;
        // Barの高さ
        public float BarHeight;

        // 右コントローラーの座標
        public float RightControllerX;
        public float RightControllerY;
        public float RightControllerZ;

        // 左コントローラーの座標
        public float LeftControllerX;
        public float LeftControllerY;
        public float LeftControllerZ;

    }


    [SerializeField] private GameObject head, rightFoot, leftFoot;
    [SerializeField] private Timer timer;
    public bool TouchTarget = false;

    public GameObject rightController;
    public GameObject leftController;

    public RFootMarkerControler rFootMarkerControler;
    public WorldController worldController;
    public HeightChangeController heightChangeController;
    void FixedUpdate()
    {
        if (isRecording)
        {
            ExperimentData data = CollectData();
            experimentData.Add(data);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isRecording)
            {
                StopRecording();
            }
            else
            {
                StartRecording();
            }
        }

        // UpArrowでサンプル数を増やす
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            sampleNum++;
        }
        // DownArrowでサンプル数を減らす
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (sampleNum > 0)
            {
                sampleNum--;
            }
        }
    }
   

    
    private ExperimentData CollectData()
    {
        ExperimentData data = new ExperimentData();
        // サンプルIDと試行時間
        data.sampleID = sampleID++;
        data.trialTime = sampleID * Time.deltaTime;
        data.startToGoalTime = timer.GetTime();

        // WorldのY座標の変化量
        data.World = worldController.totalYChange;
        data.BarHeight = heightChangeController.BarHeight; 

        // 右足の中心座標と角度
        data.centerOfRightFootX = rightFoot.transform.position.x;
        data.centerOfRightFootY = rightFoot.transform.position.y - worldController.totalYChange;
        data.centerOfRightFootZ = rightFoot.transform.position.z;
        data.rightFootFlectionAngle = rightFoot.transform.localEulerAngles.x > 180 ? rightFoot.transform.localEulerAngles.x - 360 : rightFoot.transform.localEulerAngles.x;
        data.rightFootHorizontalAngle = rightFoot.transform.localEulerAngles.y > 180 ? rightFoot.transform.localEulerAngles.y - 360 : rightFoot.transform.localEulerAngles.y;

        // 左足の中心座標と角度
        data.centerOfLeftFootX = leftFoot.transform.position.x;
        data.centerOfLeftFootY = leftFoot.transform.position.y - worldController.totalYChange;
        data.centerOfLeftFootZ = leftFoot.transform.position.z;
        data.leftFootFlectionAngle = leftFoot.transform.localEulerAngles.x > 180 ? leftFoot.transform.localEulerAngles.x - 360 : leftFoot.transform.localEulerAngles.x;
        data.leftFootHorizontalAngle = leftFoot.transform.localEulerAngles.y > 180 ? leftFoot.transform.localEulerAngles.y - 360 : leftFoot.transform.localEulerAngles.y;

        // 頭部（両目中心）の座標と角度
        data.centerEyeX = head.transform.position.x;
        data.centerEyeY = head.transform.position.y - worldController.totalYChange;
        data.centerEyeZ = head.transform.position.z;
        data.headFlexionAngle = head.transform.localEulerAngles.x > 180 ? head.transform.localEulerAngles.x - 360 : head.transform.localEulerAngles.x;
        data.headHorizontalAngle = head.transform.localEulerAngles.y > 180 ? head.transform.localEulerAngles.y - 360 : head.transform.localEulerAngles.y;

        // ターゲットへの接触判定
        data.TouchTarget = rFootMarkerControler.isTargeted;

        // 右コントローラーの座標
        if (rightController != null)
        {
            data.RightControllerX = rightController.transform.position.x;
            data.RightControllerY = rightController.transform.position.y;
            data.RightControllerZ = rightController.transform.position.z;
        }

        // 左コントローラーの座標
        if (leftController != null)
        {
            data.LeftControllerX = leftController.transform.position.x;
            data.LeftControllerY = leftController.transform.position.y;
            data.LeftControllerZ = leftController.transform.position.z;
        }

        return data;
    }
    public void OnSave()
    {
        StringBuilder csv = new StringBuilder();
        // CSVヘッダー
        csv.AppendLine("SampleID,TrialTime,StartToGoalTime,RightFootX,RightFootY,RightFootZ,RightFootFlectionAngle,RightFootHorizontalAngle,LeftFootX,LeftFootY,LeftFootZ,LeftFootFlectionAngle,LeftFootHorizontalAngle,EyeX,EyeY,EyeZ,HeadFlexionAngle,HeadHorizontalAngle,TouchTarget,WorldYChange,BarHeight,RightControllerX,RightControllerY,RightControllerZ,LeftControllerX,LeftControllerY,LeftControllerZ");
        // データの追加
        foreach (var data in experimentData)
        {
            csv.AppendLine($"{data.sampleID},{data.trialTime},{data.startToGoalTime},{data.centerOfRightFootX},{data.centerOfRightFootY},{data.centerOfRightFootZ},{data.rightFootFlectionAngle},{data.rightFootHorizontalAngle},{data.centerOfLeftFootX},{data.centerOfLeftFootY},{data.centerOfLeftFootZ},{data.leftFootFlectionAngle},{data.leftFootHorizontalAngle},{data.centerEyeX},{data.centerEyeY},{data.centerEyeZ},{data.headFlexionAngle},{data.headHorizontalAngle},{data.TouchTarget},{data.World},{data.BarHeight},{data.RightControllerX},{data.RightControllerY},{data.RightControllerZ},{data.LeftControllerX},{data.LeftControllerY},{data.LeftControllerZ}");
        }
        // ファイルパスの生成
        string dataPath = Path.Combine(Application.persistentDataPath, id + "_experimentData_" + sampleNum + ".csv");
        // CSVファイルとして書き込み
        File.WriteAllText(dataPath, csv.ToString());
        sampleNum++;
    }
    public void StartRecording()
    {
        InitiateData();
        isRecording = true;
        // 施行ごとにTouchTargetをリセット
    if (rFootMarkerControler != null)
    {
        rFootMarkerControler.isTargeted = false;
    }
    }
    private void InitiateData()
    {
        sampleID = 0;
        timer.Initialize();
        experimentData = new List<ExperimentData>();
    }
    public void StopRecording()
    {
        isRecording = false;
        
        if (resultContoller != null)
        {
             resultContoller.AddLatestTrialResult();
        }
        OnSave();
    }
    
}
