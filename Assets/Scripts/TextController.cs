using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI id, sampleNum, isRecording, yOffset, barHeight;
    [SerializeField] private ExperimentRecorder experimentRecorder;

    [SerializeField]private WorldController worldController;
    [SerializeField] private HeightChangeController heightChangeController;
    private string inputString = ""; // 入力された数字を保持する文字列
    private bool isInputMode = false; // 入力モードかどうかを判断するフラグ

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        id.text = "Id : " + experimentRecorder.id.ToString();
        sampleNum.text = "Sample : " + experimentRecorder.sampleNum.ToString();
        isRecording.text = "REC : " + (experimentRecorder.isRecording ? "ON" : "OFF");
        yOffset.text = "Y Offset : " + worldController.totalYChange.ToString("F2") + " m";
        barHeight.text = "Bar Height : " + heightChangeController.BarHeight.ToString("F2") + " m";

        // experimentRecord.idをスペースを押してからエンターを押すまでの入力数値として入力する
        // スペースキーで入力モードの切り替え
        if (Input.GetKeyDown(KeyCode.I))
        {
            isInputMode = !isInputMode;

            // 入力モードを抜ける時、入力文字列をリセット
            if (!isInputMode)
            {
                inputString = "";
            }
        }

        // 入力モードの時のみキー入力を受け付ける
        if (isInputMode)
        {
            foreach (char c in Input.inputString)
            {
                // 入力された文字が数字であるかどうかをチェック
                if (char.IsDigit(c))
                {
                    inputString += c; // 文字列に数字を追加
                }
            }

            // エンターキーが押されたら入力された数字を処理
            // エンターキーが押されたら
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (int.TryParse(inputString, out int number))
                {
                    experimentRecorder.id = number;
                }

                // 入力モードを終了
                isInputMode = false;
                inputString = ""; // 入力文字列をリセット
            }
        }

        //Dをおしたらテキスト全部の表示オンオフを切り替える
        if (Input.GetKeyDown(KeyCode.D))
        {
            // テキストの表示を切り替える
            id.enabled = !id.enabled;
            sampleNum.enabled = !sampleNum.enabled;
            isRecording.enabled = !isRecording.enabled;
            yOffset.enabled = !yOffset.enabled;
            barHeight.enabled = !barHeight.enabled;
        }
    }
}
