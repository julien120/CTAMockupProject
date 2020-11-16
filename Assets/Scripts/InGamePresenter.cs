using UnityEngine;
using UnityEngine.SceneManagement;

public class  InGamePresenter : MonoBehaviour
{
    private InGameModel inGameModel;
    private InGameView inGameView;

    [SerializeField] private Cell[] cells;
    private readonly int[,] stageState = new int[RowStage, ColStage];

    //行列の数
    private const int RowStage = 4;
    private const int ColStage = 4;

    /// <summary>
    /// 盤面の再描画を行う必要があるかのフラグ
    /// </summary>
    private bool isDirty;


    private void Start()
    {

        inGameModel = GetComponent<InGameModel>();
        inGameView = GetComponent<InGameView>();

        // Modelの値の変更を監視する
        inGameModel.changeScore += inGameView.SetScore;


        // ステージの初期状態を生成
        for (var i = 0; i < RowStage; i++)
        {
            for (var j = 0; j < ColStage; j++)
            {
                stageState[i, j] = 0;
            }
        }
        var posA = new Vector2(Random.Range(0, RowStage), Random.Range(0, ColStage));
        var posB = new Vector2((posA.x + Random.Range(1, RowStage-1)) % RowStage, (posA.y + Random.Range(1, ColStage-1)) % ColStage);
        stageState[(int)posA.x, (int)posA.y] = 2;
        stageState[(int)posB.x, (int)posB.y] = Random.Range(0, 1.0f) < 0.5f ? 2 : 4;

        // ステージの初期状態をViewに反映
        for (var i = 0; i < RowStage; i++)
        {
            for (var j = 0; j < ColStage; j++)
            {
                cells[i * ColStage + j].SetText(stageState[i, j]);
            }
        }
    }



    private void Update()
    {

        isDirty = false;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            for (var col = ColStage; col >= 0; col--)
            {
                for (var row = 0; row < RowStage; row++)
                {
                    CheckCell(row, col, 1, 0);
                }
            }
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            for (var row = 0; row < RowStage; row++)
            {
                for (var col = 0; col < ColStage; col++)
                {
                    CheckCell(row, col, -1, 0);
                }
            }

        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            for (var row = 0; row < RowStage; row++)
            {
                for (var col = 0; col < ColStage; col++)
                {
                    CheckCell(row, col, 0, -1);
                }
            }
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            for (var row = RowStage; row >= 0; row--)
            {
                for (var col = 0; col < ColStage; col++)
                {
                    CheckCell(row, col, 0, 1);
                }
            }
        }

        if (isDirty)
        {
            CreateNewRandomCell();
            for (var i = 0; i < RowStage; i++)
            {
                for (var j = 0; j < ColStage; j++)
                {
                    cells[i * ColStage + j].SetText(stageState[i, j]);
                }
            }

            if (IsGameOver(stageState))
            {
                PlayerPrefs.SetInt("SCORE", inGameModel.GetScore());
                LoadResultScene();
            }
        }

    }




    private bool CheckBorder(int row, int column, int horizontal, int vertical)
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

    private void CheckCell(int row, int column, int horizontal, int vertical)
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
        var value = stageState[row, column];
        var nextValue = stageState[nextRow, nextCol];

        // 次の移動先のマスが0の場合は移動する
        if (nextValue == 0)
        {
            // 移動元のマスは空欄になるので0を埋める
            stageState[row, column] = 0;

            // 移動先のマスに移動元のマスの値を代入する
            stageState[nextRow, nextCol] = value;

            // 移動先のマスでさらに移動チェック
            MoveCell(nextRow, nextCol, horizontal, vertical);
        }
        // 同じ値のときは合成処理
        else if (value == nextValue)
        {
            stageState[row, column] = 0;
            stageState[nextRow, nextCol] = value * 2;
            inGameModel.SetScore(value);

        }
        // 異なる値のときは移動処理を終了
        else if (value != nextValue)
        {
            return;
        }

        isDirty = true;
    }

    private void CreateNewRandomCell()
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

        stageState[row, col] = Random.Range(0, 1f) < 0.5f ? 2 : 4;
    }

    private bool IsGameOver(int[,] stageState)
    {
        // 空いている場所があればゲームオーバーにはならない
        for (var i = 0; i < stageState.GetLength(0); i++)
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
        for (var i = 0; i < stageState.GetLength(0); i++)
        {
            for (var j = 0; j < stageState.GetLength(1); j++)
            {
                var state = stageState[i, j];
                var canMerge = false;
                if (i > 0)
                {
                    canMerge |= state == stageState[i - 1, j];
                }

                if (i < stageState.GetLength(0) - 1)
                {
                    canMerge |= state == stageState[i + 1, j];
                }

                if (j > 0)
                {
                    canMerge |= state == stageState[i, j - 1];
                }

                if (j < stageState.GetLength(1) - 1)
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

    private void LoadResultScene()
    {
        SceneManager.LoadScene("ResultScene");
    }

}