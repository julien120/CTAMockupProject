using UnityEngine;

public class  InGamePresenter : MonoBehaviour
{
    private InGameModel inGameModel;
    private InGameView inGameView;

    /// <summary>
    /// 盤面の再描画を行う必要があるかのフラグ
    /// </summary>
    private bool isDirty;

    private void Start()
    {
        inGameModel = GetComponent<InGameModel>();
        inGameView = GetComponent<InGameView>();
        inGameModel.Initialize();


        // Modelの値の変更を監視する
        inGameModel.OnChangeScore += inGameView.SetScore;
        inGameModel.OnMoveCell += MoveCell;
        inGameView.OnCheckCell += inGameModel.CheckCell;
        inGameView.OnApplyGameOver += inGameModel.ApplyGameOverData;
       
        inGameView.RowStage = InGameModel.RowStage;
        inGameView.ColStage = InGameModel.ColStage;
        
        inGameView.ApplyStage(inGameModel.stageStates);
    }

    private void Update()
    {

        isDirty = false;
        inGameView.InputKey();
        if (isDirty)
        {
            inGameModel.CreateNewRandomCell();
            inGameView.ApplyUI(inGameModel.stageStates);
            
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
        var value = inGameModel.stageStates[row, column];
        var nextValue = inGameModel.stageStates[nextRow, nextCol];

        // 次の移動先のマスが0の場合は移動する
        if (nextValue == 0)
        {
            // 移動元のマスは空欄になるので0を埋める
            inGameModel.stageStates[row, column] = 0;

            // 移動先のマスに移動元のマスの値を代入する
            inGameModel.stageStates[nextRow, nextCol] = value;

            // 移動先のマスでさらに移動チェック
            MoveCell(nextRow, nextCol, horizontal, vertical);
        }
        // 同じ値のときは合成処理
        else if (value == nextValue)
        {
            inGameModel.stageStates[row, column] = 0;
            inGameModel.stageStates[nextRow, nextCol] = value * 2;
            inGameModel.SetScore(value);

        }
        // 異なる値のときは移動処理を終了
        else if (value != nextValue)
        {
            return;
        }

        isDirty = true;
    }

}