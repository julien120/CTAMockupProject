using UnityEngine;
using System;

public class InGameModel : MonoBehaviour
{

    private int score;
    public event Action<int> ChangeScore;


    //生成割合のパラメーター
    public const float GenerationRate = 0.5f;

    public readonly int[,] stageState = new int[RowStage, ColStage];

    //行列の数
    public const int RowStage = 4;
    public const int ColStage = 4;





    /// <summary>
    /// スコアの計算ロジック
    /// </summary>
    /// <param name="cellValue">合成する数値マスの値</param>
    public void SetScore(int cellValue)
    {
        score += cellValue * 2;
        ChangeScore(score);
    }

    public int GetScore(){ return score; }

    public bool IsGameOver(int[,] stageState)
    {
        // 空いている場所があればゲームオーバーにはならない
        for (var i = 0; i < RowStage; i++)
        {
            for (var j = 0; j < stageState.GetLength(1); j++)
            {
                if (stageState[i, j] <= 0)
                {
                    return false;
                }
            }
        }

        // 合成可能なマスが一つでもあればゲームオーバーにはならない
        return IsSynthesizeCell(stageState);
    }

    private bool IsSynthesizeCell(int[,] stageState)
    {
        for (var i = 0; i < RowStage; i++)
        {
            for (var j = 0; j < ColStage; j++)
            {
                var state = stageState[i, j];
                var canMerge = false;
                if (i > 0)
                {
                    canMerge |= state == stageState[i - 1, j];
                }

                if (i < RowStage - 1)
                {
                    canMerge |= state == stageState[i + 1, j];
                }

                if (j > 0)
                {
                    canMerge |= state == stageState[i, j - 1];
                }

                if (j < ColStage - 1)
                {
                    canMerge |= state == stageState[i, j + 1];
                }

                if (canMerge)
                {
                    return false;
                }
            }
        }
        return true;
    }


}
