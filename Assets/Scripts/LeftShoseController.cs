using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeftShoseController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject LeftShose;
    
   private string inputString = ""; // 入力された数字を保持する文字列
    private bool isInputMode = false; // 入力モードかどうかを判断するフラグ
     private bool isXMode = true; // trueならX, falseならY


    public float LeftShoseX { get; private set; } = 0f; // X座標を保持する変数
    public float LeftShoseY { get; private set; } = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { if (Input.GetKeyDown(KeyCode.S))
        {
            isInputMode = !isInputMode;

            // 入力モードを抜ける時は文字列リセット
            if (!isInputMode)
            {
                inputString = "";

            }

        }

        // 入力モード中は毎フレームキー入力を収集して inputString に蓄える
        if (isInputMode)
        {
            foreach (char c in Input.inputString)
            {
                if (char.IsDigit(c))
                {
                    inputString += c;
                }
                else if (c == '.')
                {
                    if (!inputString.Contains("."))
                        inputString += c;
                }
                else if (c == '-')
                {
                    if (inputString.Length == 0)
                        inputString += c;
                }
            }
        }

        // Enterキーで確定 → x座標を変更（inputString が既に蓄積されている想定）
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (float.TryParse(inputString, out float newX))
            {
                LeftShoseX = newX;
                // barRight
                Vector3 posX = LeftShose.transform.position;
                posX.x = newX;
                LeftShose.transform.position = posX;

                

                Debug.Log("LeftShoseX: " + LeftShoseX);
            }

            // 入力モード終了
            isInputMode = false;
            inputString = "";
        }
    }
}
