using UnityEngine;


public class  InGamePresenter : MonoBehaviour
{

    private InGameModel inGameModel;
    private InGameView inGameView;

    private void Start()
    {

        inGameModel = GetComponent<InGameModel>();
        inGameView = GetComponent<InGameView>();

        // Viewのイベントの設定を行う
        inGameView.inputRihgt += inGameModel.InputRight;
        inGameView.inputLeft += inGameModel.InputLeft;
        inGameView.inputUp += inGameModel.InputUp;
        inGameView.inputDown += inGameModel.InputDown;


        // Modelの値の変更を監視する
        inGameModel.changedScore += inGameView.SetScore;

    }

}