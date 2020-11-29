using UnityEngine;

/// <summary>
/// 「Presenterはmodel-Viewクラス間の橋渡し、挙動の監視を行うクラス」
///  -ユーザーのキー入力をmodelクラスの判定に伝え、
///  -modelクラスのスコア判定結果をviewクラスのscoreへ
///  -ゲームオーバー判定時の入力処理をviewへ伝える
/// </summary>
public class  InGamePresenter : MonoBehaviour
{
    private InGameModel inGameModel;
    private InGameView inGameView;


    private void Start()
    {
        inGameModel = GetComponent<InGameModel>();
        inGameView = GetComponent<InGameView>();
        inGameModel.Initialize();


        // Modelの値の変更を監視する

        //キー入力の通知をmodelに引き渡し、通知の値によって描画するセルを決定する
        inGameView.OnInputKeyRight += inGameModel.KeyRightValue;
        inGameView.OnInputKeyLeft += inGameModel.KeyleftValue;
        inGameView.OnInputKeyBottom += inGameModel.KeyBottomValue;
        inGameView.OnInputKeyFront += inGameModel.KeyFrontValue;

        //modelのスコア判定をviewに伝え、描画する
        inGameModel.OnChangeScore += inGameView.SetScore;
        inGameModel.OnChangedState += inGameView.Apply;
    }

}