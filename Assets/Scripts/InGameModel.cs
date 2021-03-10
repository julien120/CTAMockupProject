﻿using UnityEngine;
using System;
using UniRx;
using System.IO;
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
    private const float generationRate = 0.5f;
    private const int rowStage = 4;
    private const int colStage = 4;

    /// <summary>
    /// -セルのステート状況を管理&移動できるか判定
    /// </summary>
    private readonly int[,] stageStates = new int[rowStage, colStage];

    /// <summary>
    /// -スコアを管理
    /// </summary>
    public int DataScore{ get; set; }
    public int DataHighScore = 0;

    /// <summary>
    /// スコア管理
    /// </summary>
    private readonly ReactiveProperty<int> score = new ReactiveProperty<int>();
    private readonly ReactiveProperty<int> highScore = new ReactiveProperty<int>();
    public IObservable<int> OnScore => score;
    public IObservable<int> OnHighScore => highScore;

    //viewのSetScoreメソッドを引き渡し
    //public event Action<int,int> OnChangedState;
    private readonly Subject<(int,int)> changedState = new Subject<(int,int)>();
    public IObservable<(int,int)> OnChangedState => changedState;

    /// <summary>
    /// 盤面の再描画を行う必要があるかのフラグ
    /// </summary>
    private bool isDirty;

    private bool isKeyOn = false;


    //highScoreDataをランキング化する
    [System.Serializable]
    private struct highScoreData
    {
        public int DataHighScore;
    }

    // ファイルパス
    private string _dataPath;

    ///<summary>
    ///画面に描画する処理：ステージの初期状態を生成
    ///</summary>
    public void Initialize()
    {
        // ファイルのパスを計算
        _dataPath = Path.Combine(Application.persistentDataPath, "highScore.json");

        for (var i = 0; i < rowStage; i++)
        {
            for (var j = 0; j < colStage; j++)
            {
                stageStates[i, j] = 0;
            }
        }
        var posA = new Vector2(Random.Range(0, rowStage), Random.Range(0, colStage));
        var posB = new Vector2((posA.x + Random.Range(1, rowStage - 1)) % rowStage, (posA.y + Random.Range(1, colStage - 1)) % colStage);
        stageStates[(int)posA.x, (int)posA.y] = 2;
        stageStates[(int)posB.x, (int)posB.y] = Random.Range(0, 1.0f) < generationRate ? 2 : 4;

        //4*4あるうちのひとます
        //OnChangedState(RowStage * RowStage + ColStage, stageStates[RowStage, ColStage]);
        for (var i = 0; i < rowStage; i++)
        {
            for (var j = 0; j < colStage; j++)
            {
                var indexAndstageStates = (i * rowStage + j, stageStates[i, j]);
                changedState.OnNext(indexAndstageStates);
            }
        }
        //TODO:デシリアライズする
        //DataHighScore = PlayerPrefs.GetInt(PlayerPrefsKeys.ScoreHighData);
        OnLoad();
        ApplyGameOverData();
    }



    private void OnLoad()
    {
        // 念のためファイルの存在チェック
        if (!File.Exists(_dataPath)) return;

        // JSONデータとしてデータを読み込む
        var json = File.ReadAllText(_dataPath);

        // JSON形式からオブジェクトにデシリアライズ
        var obj = JsonUtility.FromJson<highScoreData>(json);

        // Transformにオブジェクトのデータをセット
        DataHighScore = obj.DataHighScore;
    }

    public void ObserveInputKey(InputDirection direction)
    {
        switch (direction)
        {
            case InputDirection.Right:

                if (!isKeyOn)
                {
                    isDirty = false;

                    for (var col = colStage; col >= 0; col--)
                    {
                        for (var row = 0; row < rowStage; row++)
                        {
                            CheckCell(row, col, 1, 0);
                        }
                    }
                    if (isDirty)
                    {
                        CreateNewRandomCell();
                        DrawChangedStates();
                        ApplyGameOverData();
                    }
                }
                    break;

            case InputDirection.Left:

                if (!isKeyOn)
                {

                    isDirty = false;

                    for (var row = 0; row < rowStage; row++)
                    {
                        for (var col = 0; col < colStage; col++)
                        {
                            CheckCell(row, col, -1, 0);
                        }
                    }
                    if (isDirty)
                    {
                        CreateNewRandomCell();
                        DrawChangedStates();
                        ApplyGameOverData();
                    }
                }
                break;

            case InputDirection.Up:
                if (!isKeyOn)
                {
                    isDirty = false;

                    for (var row = 0; row < rowStage; row++)
                    {
                        for (var col = 0; col < colStage; col++)
                        {
                            CheckCell(row, col, 0, -1);
                        }
                    }
                    //もしisDirtyであれば、OnChangedState()する
                    if (isDirty)
                    {
                        CreateNewRandomCell();
                        DrawChangedStates();
                        ApplyGameOverData();
                    }
                }
                break;

            case InputDirection.Down:
                if (!isKeyOn)
                {
                    isDirty = false;

                    for (var row = rowStage; row >= 0; row--)
                    {
                        for (var col = 0; col < colStage; col++)
                        {
                            CheckCell(row, col, 0, 1);
                        }
                    }
                    if (isDirty)
                    {
                        CreateNewRandomCell();
                        DrawChangedStates();
                        ApplyGameOverData();
                    }
                }
                break;

            case InputDirection.None:
                break;
        }
    }


    ///<summary>
    ///</summary>
    private void DrawChangedStates()
    {
        for (var i = 0; i < rowStage; i++)
        {
            for (var j = 0; j < colStage; j++)
            {
                var indexAndStageStates = (i * rowStage + j, stageStates[i, j]);
                changedState.OnNext(indexAndStageStates);
            }
        }
    }


    /// <summary>
    /// スコアの計算ロジック
    /// </summary>
    /// <param name="cellValue">合成する数値マスの値</param>
    public void SetScore(int cellValue)
    {
        score.Value += cellValue * 2;
        DataScore = score.Value;
    }

    //TODO:スコアデータをランキングデータとしてjsonファイルに格納する。
    public void CheckHighScore(int score)
    {
        //if (score > DataHighScore) {
        //    DataHighScore = score;
        //    //PlayerPrefs.SetInt(PlayerPrefsKeys.ScoreHighData, DataHighScore);
        //    //highScore.Value = DataHighScore;
        //    OnSave(DataHighScore);
        //    highScore.Value = DataHighScore;
        //}
    }

    private void OnSave(int highScore)
    {
        // シリアライズするオブジェクトを作成
        var obj = new highScoreData
        {
            DataHighScore = highScore
        };

        // JSON形式にシリアライズ
        var json = JsonUtility.ToJson(obj, false);

        // JSONデータをファイルに保存
        File.WriteAllText(_dataPath, json);
    }



    private bool IsGameOver()
    {
        // 空いている場所があればゲームオーバーにはならない
        for (var i = 0; i < rowStage; i++)
        {
            for (var j = 0; j < colStage; j++)
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
        for (var i = 0; i < rowStage; i++)
        {
            for (var j = 0; j < colStage; j++)
            {
                var state = stageStates[i, j];
                var canMerge = false;
                if (i > 0)
                {
                    canMerge |= state == stageStates[i - 1, j];
                }

                if (i < rowStage - 1)
                {
                    canMerge |= state == stageStates[i + 1, j];
                }

                if (j > 0)
                {
                    canMerge |= state == stageStates[i, j - 1];
                }

                if (j < colStage - 1)
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

    private bool CheckBorder(int row, int column, int horizontal, int vertical)
    {

        // チェックマスが4x4外ならそれ以上処理を行わない
        if (row < 0 || row >= rowStage || column < 0 || column >= colStage)
        {
            return false;
        }

        // 移動先が4x4外ならそれ以上処理は行わない
        var nextRow = row + vertical;
        var nextCol = column + horizontal;
        if (nextRow < 0 || nextRow >= rowStage || nextCol < 0 || nextCol >= colStage)
        {
            return false;
        }

        return true;
    }

    private void CheckCell(int row, int column, int horizontal, int vertical)
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

    private void CreateNewRandomCell()
    {
        // ゲーム終了時はスポーンしない
        if (IsGameOver())
        {
            return;
        }
        var row = Random.Range(0, rowStage);
        var col = Random.Range(0, colStage);
        while (stageStates[row, col] != 0)
        {
            row = Random.Range(0, rowStage);
            col = Random.Range(0, colStage);
        }
        stageStates[row, col] = Random.Range(0, 1f) < InGameModel.generationRate ? 2 : 4;
    }

    private void ApplyGameOverData()
    {
        if (IsGameOver())
        {
            int score = DataScore;
            CheckHighScore(score);
            LoadRestartScene(score);
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
            CheckHighScore(DataScore);
        }
        // 異なる値のときは移動処理を終了
        else if (value != nextValue)
        {
            return;
        }
        isDirty = true;
    }

    public void CannotInputKey()
    {
        isKeyOn = true;
    }

    public void CanInputKey()
    {
        isKeyOn = false;
    }

    public void LoadRestartScene(int DataScore)
    {
        PlayerPrefs.SetInt(PlayerPrefsKeys.ScoreData, DataScore);
        SceneController.Instance.LoadResultScene();
    }

    public void RestartGame()
    {
        
        DataScore = 0;
        //TODO:
        PlayerPrefs.SetInt(PlayerPrefsKeys.ScoreData, DataScore);
        score.Value = DataScore;
        Initialize();
    }

   
}
