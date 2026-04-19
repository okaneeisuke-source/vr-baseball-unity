using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class StroopStimulus : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stimulusText;
    [SerializeField] private float displayTime = 1.5f; // 1.5秒表示
    [SerializeField] private float offTime = 0.5f; // 0.5秒消去
    
    private string[] characters = { "赤", "青", "黒", "黄" };
    private List<(string character, Color color, string colorName)> currentSet = new List<(string, Color, string)>();

    void Start()
    {
        if (stimulusText == null)
        {
            stimulusText = GetComponent<TextMeshProUGUI>();
        }
        stimulusText.enabled = false;
    }

    /// <summary>
    /// 4文字の不一致ストループセットを生成（ランダム組み合わせ）
    /// </summary>
    public void GenerateSet()
    {
        currentSet.Clear();
        
        // 色リスト（赤、青、黒、黄）
        List<(Color, string)> colors = new List<(Color, string)>
        {
            (Color.red, "赤"),
            (new Color(0, 0, 1), "青"),
            (Color.black, "黒"),
            (new Color(1, 1, 0), "黄")
        };
        
        // 各文字に異なる色をランダム割り当て
        List<string> charList = new List<string>(characters);
        List<(Color, string)> colorList = new List<(Color, string)>(colors);
        
        for (int i = 0; i < characters.Length; i++)
        {
            // ランダムに文字選択
            int charIndex = Random.Range(0, charList.Count);
            string chara = charList[charIndex];
            charList.RemoveAt(charIndex);
            
            // ランダムに色選択
            int colorIndex = Random.Range(0, colorList.Count);
            Color col = colorList[colorIndex].Item1;
            string colorName = colorList[colorIndex].Item2;
            colorList.RemoveAt(colorIndex);
            
            // 文字と色が一致しないことを確認
            if (chara != colorName)
            {
                currentSet.Add((chara, col, colorName));
            }
            else
            {
                // 一致した場合は別の色を選択
                int altColorIndex = Random.Range(0, colorList.Count);
                col = colorList[altColorIndex].Item1;
                colorName = colorList[altColorIndex].Item2;
                colorList.RemoveAt(altColorIndex);
                currentSet.Add((chara, col, colorName));
            }
        }
        
        Debug.Log("Generated Stroop Set: " + string.Join(", ", currentSet));
    }

    /// <summary>
    /// 指定インデックスの文字を表示
    /// </summary>
    public void DisplayCharacter(int index)
    {
        if (index >= 0 && index < currentSet.Count)
        {
            stimulusText.text = currentSet[index].character;
            stimulusText.color = currentSet[index].color;
            stimulusText.enabled = true;
            
            Debug.Log($"Displaying: {currentSet[index].character} in color {currentSet[index].colorName}");
        }
    }

    /// <summary>
    /// 文字を隠す
    /// </summary>
    public void HideCharacter()
    {
        stimulusText.enabled = false;
    }

    /// <summary>
    /// 現在のセット取得
    /// </summary>
    public List<(string character, Color color, string colorName)> GetCurrentSet()
    {
        return currentSet;
    }

    /// <summary>
    /// 表示時間取得
    /// </summary>
    public float GetDisplayTime() => displayTime;

    /// <summary>
    /// 消去時間取得
    /// </summary>
    public float GetBlankTime() => offTime;

    /// <summary>
    /// 表示時間設定（可変）
    /// </summary>
    public void SetDisplayTime(float time)
    {
        displayTime = Mathf.Max(0.1f, time);
    }

    /// <summary>
    /// 消去時間設定（可変）
    /// </summary>
    public void SetBlankTime(float time)
    {
        offTime = Mathf.Max(0.1f, time);
    }
}