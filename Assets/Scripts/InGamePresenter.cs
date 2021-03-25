using UnityEngine;
using System;
using UniRx;

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
    [SerializeField] private RankingWindowModel rankingWindowModel;
    [SerializeField] private RankingWindowPresenter rankingWindowPresenter;
    [SerializeField] private MenuWindowPresenter menuWindowPresenter;

    public event Action OnOpenMenu;

    private void Start()
    {
        inGameModel = GetComponent<InGameModel>();
        inGameView = GetComponent<InGameView>();
       // rankingWindowModel = GetComponent<RankingWindowModel>();

        // Modelの値の変更を監視する

        //キー入力の通知をmodelに引き渡し、通知の値によって描画するセルを決定する
        inGameView.InputKeySubject.Subscribe(InputKeySubject => inGameModel.ObserveInputKey(InputKeySubject));


        //modelのスコア判定をviewに伝え、描画する
        inGameModel.OnScore.Subscribe(OnScore => inGameView.SetScore(OnScore));
        inGameModel.OnHighScore.Subscribe(OnHighScore => inGameView.SetHighScore(OnHighScore));

        inGameModel.OnChangedState.Subscribe(OnChangedState => inGameView.Apply(OnChangedState.Item1, OnChangedState.Item2));

        inGameModel.Initialize();
        inGameView.SetHighScore(inGameModel.DataHighScore);

        //Restartmenuを開いたときの処理：キー入力禁止、リスタートボタンの実装        
        menuWindowPresenter.Initialize(); 
        menuWindowPresenter.OnKeyOn.Subscribe(_ => inGameModel.CanInputKey());
        menuWindowPresenter.OnRestart.Subscribe(_ => inGameModel.RestartGame());
        inGameView.OnOpenMenu.Subscribe(_ => OpenMenu());

        //rankingMenu
        rankingWindowPresenter.SetRankingScore(inGameModel.DataHighScore);

        rankingWindowModel.PostRankingScore(inGameModel.DataHighScore);
    }


    private void OpenMenu()
    {
        menuWindowPresenter.OpenMenu();
        inGameModel.CannotInputKey();
    }
}