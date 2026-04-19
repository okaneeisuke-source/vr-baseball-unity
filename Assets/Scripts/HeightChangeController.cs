using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeightChangeController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject barRight, barLeft, targetbar;
    [SerializeField] public WorldController worldController; // WorldControllerへの参照
   private string inputString = ""; // 入力された数字を保持する文字列
    private bool isInputMode = false; // 入力モードかどうかを判断するフラグ

    
    public float BarHeight { get; private set; } = 0f; // Y座標の総変化量を保持する変数
       void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
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

        // Enterキーで確定 → y座標を変更（inputString が既に蓄積されている想定）
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (float.TryParse(inputString, out float newY))
            {
                // WorldControllerの座標変化を基準として相対座標を計算
                float adjustedY = newY + worldController.totalYChange;
                BarHeight = newY;
                // barRight
                Vector3 posR = barRight.transform.position;
                posR.y = adjustedY;
                barRight.transform.position = posR;

                // barLeft
                Vector3 posL = barLeft.transform.position;
                posL.y = adjustedY;
                barLeft.transform.position = posL;

                // targetbar
                Vector3 posT = targetbar.transform.position;
                posT.y = adjustedY;
                targetbar.transform.position = posT;

                Debug.Log("BarHeight: " + BarHeight);
            }

            // 入力モード終了
            isInputMode = false;
            inputString = "";
        }
    }
}
