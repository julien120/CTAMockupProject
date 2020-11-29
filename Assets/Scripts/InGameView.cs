using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 「Viewは画面の描画やユーザー操作を実装するクラス」
/// -セルの描画
/// -スコアの描画
/// -ユーザーのキー入力と通知
/// </summary>
public class InGameView : MonoBehaviour
{
    [SerializeField] private Cell[] cells;
    [SerializeField] private Text scoreText;

    public event Action<int,int,int,int> OnCheckCell;
    public event Action OnApplyGameOver;

    public event Action OnInputKeyRight;
    public event Action OnInputKeyLeft;
    public event Action OnInputKeyBottom;
    public event Action OnInputKeyFront;

    public int RowStage;
    public int ColStage;
   

    private void Update()
    {
        ObserveInputKey();
    }

    /// <summary>
    /// スコアの描画
    /// </summary>
    /// <param name="score"></param>
    public void SetScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }


    /// <summary>
    /// cells[index]にテキストを表示させる
    /// </summary>
    /// <param name="index"></param>
    /// <param name="stageValue"></param>
    public void Apply(int index ,int stageValue)
    {
        cells[index].SetText(stageValue);
    }

    /// <summary>
    /// ユーザーのキー入力
    /// </summary>
    ///
    public void ObserveInputKey()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnInputKeyRight();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

            OnInputKeyLeft();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnInputKeyFront();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnInputKeyBottom();
        }
    }

}
