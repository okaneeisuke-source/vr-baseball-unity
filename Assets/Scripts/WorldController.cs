using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldController : MonoBehaviour
{

    [SerializeField] public GameObject World;
    private string inputString = ""; // 入力された数字を保持する文字列
    private bool isInputMode = false; // 入力モードかどうかを判断するフラグ

    private float previousY = 0f; // 変更前のY座標を保持する変数
    public float totalYChange { get; private set; } = 0f; // Y座標の総変化量を保持する変数



    // Start is called before the first frame update
    void Start()
    {
        previousY = World.transform.position.y;// 初期のY座標を保存

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
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
                float currentY = World.transform.position.y;
                totalYChange += (newY - currentY);
                Vector3 posW = World.transform.position;
                posW.y = newY;
                World.transform.position = posW;
                previousY = newY;

                Debug.Log("totalYChange: " + totalYChange);
            }

            // 入力モード終了
            isInputMode = false;
            inputString = "";
        }


    }
}
