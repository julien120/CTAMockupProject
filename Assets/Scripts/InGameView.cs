using UnityEngine;
using UnityEngine.UI;
using System;

public class InGameView : MonoBehaviour
{
    [SerializeField] private Cell[] cells;
    [SerializeField] private Text scoreText;


    public event Action<int,int,int,int> OnCheckCell;
    public event Action OnApplyGameOver;

    public int RowStage;
    public int ColStage;
   

    private void Update()
    {
        InputKey();
    }
    
    public void SetScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    /// <summary>
    /// stageを描画する
    /// </summary>
    public void ApplyStage(int[,] stageStates)
    {
        for (var i = 0; i < RowStage; i++)
        {
            for (var j = 0; j < ColStage; j++)
            {
                cells[i * RowStage + j].SetText(stageStates[i, j]);
            }
        }
    }

    /// <summary>
    /// セルをUIに反映する処理
    /// </summary>
    public void ApplyUI(int[,] stageStates)
    {
        ApplyStage(stageStates);
        OnApplyGameOver();
    }

    /// <summary>
    /// userによる入力
    /// </summary>
    public void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            for (var col = ColStage; col >= 0; col--)
            {
                for (var row = 0; row < RowStage; row++)
                {
                    OnCheckCell(row, col, 1, 0);
                }
            }
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            for (var row = 0; row < RowStage; row++)
            {
                for (var col = 0; col < ColStage; col++)
                {
                    OnCheckCell(row, col, -1, 0);
                }
            }

        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            for (var row = 0; row < RowStage; row++)
            {
                for (var col = 0; col < ColStage; col++)
                {
                    OnCheckCell(row, col, 0, -1);
                }
            }
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            for (var row = RowStage; row >= 0; row--)
            {
                for (var col = 0; col < ColStage; col++)
                {
                    OnCheckCell(row, col, 0, 1);
                }
            }
        }
    }


}
