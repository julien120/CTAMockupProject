using UnityEngine;
using UnityEngine.SceneManagement;

public class  InGamePresenter : MonoBehaviour
{
    private InGameModel inGameModel;
    private InGameView inGameView;


    /// <summary>
    /// 盤面の再描画を行う必要があるかのフラグ
    /// </summary>
    private bool isDirty;

    private int[,] stageState;

    private void Start()
    {
        
        inGameModel = GetComponent<InGameModel>();
        inGameView = GetComponent<InGameView>();

        // Modelの値の変更を監視する
        inGameModel.ChangeScore += inGameView.SetScore;
        inGameModel.MoveCell += MoveCell;
        inGameView.CheckCell += inGameModel.CheckCell;

        stageState = inGameModel.stageState;

        //これで参照元のinGameView.csの変数にも代入されるの？実感が湧かない
        inGameView.RowStage = InGameModel.RowStage;
        inGameView.ColStage = InGameModel.ColStage;
        inGameView.stageState = stageState;


    }

    private void Update()
    {

        isDirty = false;

        if (isDirty)
        {
            inGameModel.CreateNewRandomCell();
            ReflectUI();
        }

    }


    private void MoveCell(int row, int column, int horizontal, int vertical)
    {
        // 4x4境界線チェック
        // 再起呼び出し以降も毎回境界線チェックはするため冒頭で呼び出しておく
        if (inGameModel.CheckBorder(row, column, horizontal, vertical) == false)
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