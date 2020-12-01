﻿using UnityEngine;
using System;
using Random = UnityEngine.Random;

/// <summary>
/// 「modelはデータ管理・判定処理を担うクラス」
///  -セルのステート状況を管理
///  -スコアを管理
///  -移動できるか判定
///  -ゲームオーバーか判定
/// </summary>
public class InGameModel : MonoBehaviour
{
    //生成割合のパラメーター&行列
    public const float GenerationRate = 0.5f;
    public const int RowStage = 4;
    public const int ColStage = 4;

    /// <summary>
    /// -セルのステート状況を管理&移動できるか判定
    /// </summary>
    public readonly int[,] stageStates = new int[RowStage, ColStage];

    /// <summary>
    /// -スコアを管理
    /// </summary>
    public int Score { get; private set; }

    public event Action<int> OnChangeScore;

    //viewのSetScoreメソッドを引き渡し
    public event Action<int,int> OnChangedState;

    /// <summary>
    /// 盤面の再描画を行う必要があるかのフラグ
    /// </summary>
    private bool isDirty;

    ///<summary>
    ///画面に描画する処理：ステージの初期状態を生成
    ///</summary>
    public void Initialize()
    {
        for (var i = 0; i < RowStage; i++)
        {
            for (var j = 0; j < ColStage; j++)
            {
                stageStates[i, j] = 0;
            }
        }
        var posA = new Vector2(Random.Range(0, RowStage), Random.Range(0, ColStage));
        var posB = new Vector2((posA.x + Random.Range(1, RowStage - 1)) % RowStage, (posA.y + Random.Range(1, ColStage - 1)) % ColStage);
        stageStates[(int)posA.x, (int)posA.y] = 2;
        stageStates[(int)posB.x, (int)posB.y] = Random.Range(0, 1.0f) < GenerationRate ? 2 : 4;
        //TODO:最初の初期生成
        //4*4あるうちのひとます
        //OnChangedState(RowStage * RowStage + ColStage, stageStates[RowStage, ColStage]);
        for (var i = 0; i < RowStage; i++)
        {
            for (var j = 0; j < ColStage; j++)
            {
                OnChangedState(i * RowStage + j, stageStates[i, j]);
            }
        }ApplyGameOverData();
    }

    public void KeyRightValue()
    {
        isDirty = false;
        // TODO: 左方向の入力に対して全マスの移動を試みる
        for (var col = ColStage; col >= 0; col--)
        {
            for (var row = 0; row < RowStage; row++)
            {
                CheckCell(row, col, 1, 0);
            }
        }
        if (isDirty)
        {
            CreateNewRandomCell();
            //indexとstageValueに当たる引数は何か?
            for (var i = 0; i < RowStage; i++)
            {
                for (var j = 0; j < ColStage; j++)
                {
                    Debug.Log(i * RowStage + j +"と"+ stageStates[i, j]);
                    OnChangedState(i * RowStage + j, stageStates[i, j]);
                }
            }
            ApplyGameOverData();
        }

    }
    public void KeyleftValue()
    {
        isDirty = false;
        // TODO: 左方向の入力に対して全マスの移動を試みる
        for (var row = 0; row < RowStage; row++)
        {
            for (var col = 0; col < ColStage; col++)
            {
                CheckCell(row, col, -1, 0);
            }
        }
        if (isDirty)
        {
            CreateNewRandomCell();
            //indexとstageValueに当たる引数は何か?
            for (var i = 0; i < RowStage; i++)
            {
                for (var j = 0; j < ColStage; j++)
                {
                    OnChangedState(i * RowStage + j, stageStates[i, j]);
                }
            }
            ApplyGameOverData();
        }
    }
    public void KeyBottomValue()
    {
        isDirty = false;
        // TODO: 下方向の入力に対して全マスの移動を試みる
        for (var row = RowStage; row >= 0; row--)
        {
            for (var col = 0; col < ColStage; col++)
            {
                CheckCell(row, col, 0, 1);
            }
        }
        if (isDirty)
        {
            CreateNewRandomCell();
            //indexとstageValueに当たる引数は何か?
            for (var i = 0; i < RowStage; i++)
            {
                for (var j = 0; j < ColStage; j++)
                {
                    OnChangedState(i * RowStage + j, stageStates[i, j]);
                }
            }
            ApplyGameOverData();
        }
    }
    public void KeyFrontValue()
    {
        isDirty = false;
        // TODO: ステージの状態に変化があれば、OnChangedStateを呼び出してあげる
        // TODO: 上方向の入力に対して全マスの移動を試みる
        for (var row = 0; row < RowStage; row++)
        {
            for (var col = 0; col < ColStage; col++)
            {
                CheckCell(row, col, 0, -1);
                // TODO: 各マスに対してMoveCellしてあげる
            }
        }
        //もしisDirtyであれば、OnChangedState()する
        if (isDirty)
        {
            CreateNewRandomCell();
            //indexとstageValueに当たる引数は何か?
            for (var i = 0; i < RowStage; i++)
            {
                for (var j = 0; j < ColStage; j++)
                {
                    OnChangedState(i * RowStage + j, stageStates[i, j]);
                }
            }
            ApplyGameOverData();
        }
    }


    /// <summary>
    /// スコアの計算ロジック
    /// </summary>
    /// <param name="cellValue">合成する数値マスの値</param>
    public void SetScore(int cellValue)
    {
        Score += cellValue * 2;
        OnChangeScore(Score);
    }
    
 

    private bool IsGameOver()
    {
        // 空いている場所があればゲームオーバーにはならない
        for (var i = 0; i < RowStage; i++)
        {
            for (var j = 0; j < ColStage; j++)
            {
                if (stageStates[i, j] <= 0)
                {
                    return false;
                }
            }
        }

        // 合成可能なマスが一つでもあればゲームオーバーにはならない
        return IsSynthesizeCell(stageStates);
    }

    ///<summary>
    ///セルを合成する
    ///</summary>
    private bool IsSynthesizeCell(int[,] stageStates)
    {
        for (var i = 0; i < RowStage; i++)
        {
            for (var j = 0; j < ColStage; j++)
            {
                var state = stageStates[i, j];
                var canMerge = false;
                if (i > 0)
                {
                    canMerge |= state == stageStates[i - 1, j];
                }

                if (i < RowStage - 1)
                {
                    canMerge |= state == stageStates[i + 1, j];
                }

                if (j > 0)
                {
                    canMerge |= state == stageStates[i, j - 1];
                }

                if (j < ColStage - 1)
                {
                    canMerge |= state == stageStates[i, j + 1];
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
        if (stageStates[row, column] == 0)
        {
            return;
        }
        // 移動可能条件を満たした場合のみ移動処理
        MoveCell(row, column, horizontal, vertical);

    }

    public void CreateNewRandomCell()
    {
        // ゲーム終了時はスポーンしない
        if (IsGameOver())
        {
            return;
        }
        var row = Random.Range(0, RowStage);
        var col = Random.Range(0, ColStage);
        while (stageStates[row, col] != 0)
        {
            row = Random.Range(0, RowStage);
            col = Random.Range(0, ColStage);
        }
        stageStates[row, col] = Random.Range(0, 1f) < InGameModel.GenerationRate ? 2 : 4;
    }

    public void ApplyGameOverData()
    {
        if (IsGameOver())
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.ScoreData, Score);
            SceneController.Instance.LoadResultScene();
        }
    }

    private void MoveCell(int row, int column, int horizontal, int vertical)
    {
        // 4x4境界線チェック
        // 再起呼び出し以降も毎回境界線チェックはするため冒頭で呼び出しておく
        if (CheckBorder(row, column, horizontal, vertical) == false)
        {
            return;
        }
        // 移動先の位置を計算
        var nextRow = row + vertical;
        var nextCol = column + horizontal;

        // 移動元と移動先の値を取得
        var value = stageStates[row, column];
        var nextValue = stageStates[nextRow, nextCol];

        // 次の移動先のマスが0の場合は移動する
        if (nextValue == 0)
        {
            // 移動元のマスは空欄になるので0を埋める
            stageStates[row, column] = 0;

            // 移動先のマスに移動元のマスの値を代入する
            stageStates[nextRow, nextCol] = value;

            // 移動先のマスでさらに移動チェック
            MoveCell(nextRow, nextCol, horizontal, vertical);
        }
        // 同じ値のときは合成処理
        else if (value == nextValue)
        {
            stageStates[row, column] = 0;
            stageStates[nextRow, nextCol] = value * 2;
            SetScore(value);

        }
        // 異なる値のときは移動処理を終了
        else if (value != nextValue)
        {
            return;
        }
        isDirty = true;
    }
}
