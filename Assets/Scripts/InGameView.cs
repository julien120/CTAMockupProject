using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

/// <summary>
/// 「Viewは画面の描画やユーザー操作を実装するクラス」
/// -セルの描画
/// -スコアの描画
/// -ユーザーのキー入力と通知
/// </summary>
public class InGameView : MonoBehaviour
{
    [SerializeField] private Cell[] cells;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;
    private IInputInterface iInputInterface; 

    public event Action OnOpenMenu;

    private Subject<Unit> inputKeyRightSubject = new Subject<Unit>();
    private Subject<Unit> inputKeyLeftSubject = new Subject<Unit>();
    private Subject<Unit> inputKeyBottomSubject = new Subject<Unit>();
    private Subject<Unit> inputKeyFrontSubject = new Subject<Unit>();

    //こっちをpresenterが操作する.IObservalだとOnNextを発行できない
    public IObservable<Unit> OnInputKeyRight => inputKeyRightSubject;
    public IObservable<Unit> OnInputKeyLeft => inputKeyLeftSubject;
    public IObservable<Unit> OnInputKeyBottom => inputKeyBottomSubject;
    public IObservable<Unit> OnInputKeyFront => inputKeyFrontSubject;



    /// <summary>
    /// プラットフォーム判断し、インターフェースの実装しているスクリプトでそれぞれの挙動を記述する
    /// </summary>
    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            iInputInterface = new InputOnMobile();
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            iInputInterface = new InputOnPC();
        }
        else
        {
            iInputInterface = new InputOnPC();
        }
    }

    /// <summary>
    /// PCでもmobileでもOnInputKeyRight();
    /// </summary>
    private void Update()
    {
        //pcでもmobileでもInputKey()内の上下左右によって変わる出力をInGameViewに伝え、
        //それぞれに対応するアクション型を出力する
        ObserveInputKey();
    }

    /// <summary>
    /// スコアの描画
    /// </summary>
    /// <param name="score"></param>
    public void SetScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

   /// <summary>
   /// ハイスコアの描画
   /// </summary>
    public void SetHighScore(int highScore)
    {
        
        highScoreText.text = $"HiScore: {highScore}";
        
    }


    /// <summary>
    /// cells[index]にテキストを表示させる
    /// </summary>
    /// <param name="index"></param>
    /// <param name="stageValue"></param>
    public void Apply(int index ,int stageValue)
    {
        cells[index].SetText(stageValue);
    }

    /// <summary>
    /// ユーザーのキー入力
    /// </summary>

    public void ObserveInputKey()
    {
        InputDirection direction = iInputInterface.InputKey();
        switch (direction)
        {
            case InputDirection.Right:
                inputKeyRightSubject.OnNext(Unit.Default);
                break;

            case InputDirection.Left:
                inputKeyLeftSubject.OnNext(Unit.Default);
                break;

            case InputDirection.Up:
                inputKeyFrontSubject.OnNext(Unit.Default);
                break;

            case InputDirection.Down:
                inputKeyBottomSubject.OnNext(Unit.Default);
                break;

            case InputDirection.None:
                break;
        }
    }

    /// <summary>
    /// Windowを表示する
    /// TODO:OpenWindow中はセルの移動入力は受け付けない。絶対false。
    /// </summary>
    public void OpenWindow()
    {
        //メニューを開く:MenuWindow.Viewに繋がっている
        OnOpenMenu();
    }

}
