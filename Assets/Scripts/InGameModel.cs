using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class InGameModel : MonoBehaviour
{

    private int score;
    public event Action<int> ChangeScore;
    public event Action<int,int,int,int> MoveCell;

    //生成割合のパラメーター
    public const float GenerationRate = 0.5f;

    public readonly int[,] stageState = new int[RowStage, ColStage];

    //行列の数
    public const int RowStage = 4;
    public const int ColStage = 4;




    private void Start()
    {
        ///<summary>
        ///画面に描画する処理：ステージの初期状態を生成
        ///</summary>
        for (var i = 0; i < RowStage; i++)
        {
            for (var j = 0; j < ColStage; j++)
            {
                stageState[i, j] = 0;
            }
        }
        var posA = new Vector2(Random.Range(0, RowStage), Random.Range(0, ColStage));
        var posB = new Vector2((posA.x + Random.Range(1, RowStage - 1)) % RowStage, (posA.y + Random.Range(1, ColStage - 1)) % ColStage);
        stageState[(int)posA.x, (int)posA.y] = 2;
        stageState[(int)posB.x, (int)posB.y] = Random.Range(0, 1.0f) < GenerationRate ? 2 : 4;
    }


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

    //判定系もmodel?
    public bool CheckBorder(int row, int column, int horizontal, int vertical)
    {

        // チェックマスが4x4外ならそれ以上処理を行わない
        if (row < 0 || row >= RowStage || column < 0 || column >= ColStage)
        {
            return false;
        }

        // 移動先が4x4外ならそれ以上処理は行わない
        var nextRow = row + vertical;
        var nextCol = column + horizontal;
        if (nextRow < 0 || nextRow >= RowStage || nextCol < 0 || nextCol >= ColStage)
        {
            return false;
        }

        return true;
    }

    public void CheckCell(int row, int column, int horizontal, int vertical)
    {
        // 4x4の境界線チェック
        if (CheckBorder(row, column, horizontal, vertical) == false)
        {
            return;
        }
        // 空欄マスは移動処理をしない
        if (stageState[row, column] == 0)
        {
            return;
        }
        // 移動可能条件を満たした場合のみ移動処理
        MoveCell(row, column, horizontal, vertical);
    }

    public void CreateNewRandomCell()
    {
        // ゲーム終了時はスポーンしない
        if (IsGameOver(stageState))
        {
            return;
        }
        var row = Random.Range(0, RowStage);
        var col = Random.Range(0, ColStage);
        while (stageState[row, col] != 0)
        {
            row = Random.Range(0, RowStage);
            col = Random.Range(0, ColStage);
        }
        stageState[row, col] = Random.Range(0, 1f) < InGameModel.GenerationRate ? 2 : 4;
    }

    public void ApplyGameOverData()
    {
        if (IsGameOver(stageState))
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.ScoreData, GetScore());
            SceneController.Instance.LoadResultScene();
        }
    }
}
