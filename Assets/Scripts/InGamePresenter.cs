using UnityEngine;
using UnityEngine.SceneManagement;

public class  InGamePresenter : MonoBehaviour
{
    private InGameModel inGameModel;
    private InGameView inGameView;

   
    private int RowStage = InGameView.RowStage;
    private int ColStage = InGameView.ColStage;

    /// <summary>
    /// 盤面の再描画を行う必要があるかのフラグ
    /// </summary>
    private bool isDirty;

    private int[,] stageState;

    private void Start()
    {
        
        inGameModel = GetComponent<InGameModel>();
        inGameView = GetComponent<InGameView>();

        stageState = inGameView.stageState;

        // Modelの値の変更を監視する
        inGameModel.changeScore += inGameView.SetScore;

        inGameView.CheckCell += CheckCell;

 
    }

    private void Update()
    {

        isDirty = false;

        if (isDirty)
        {
            CreateNewRandomCell();
            ReflectUI();
        }

    }

    //判定系もmodel?
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
        if (inGameModel.IsGameOver(stageState))
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



    ///<summary>
    ///セルの移動
    ///</summary>




    /// <summary>
    /// セルをUIに反映する処理
    /// </summary>
    private void ReflectUI()
    {
        inGameView.ReflectStage();

        if (inGameModel.IsGameOver(stageState))
        {
            //score保存自体もmodel?
            PlayerPrefs.SetInt(PlayerPrefsKeys.ScoreData, inGameModel.GetScore());
            SceneController.Instance.LoadResultScene();
        }
    }

    
   

    




    


}