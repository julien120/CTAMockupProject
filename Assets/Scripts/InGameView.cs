using UnityEngine;
using UnityEngine.UI;
using System;

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
    private IInputInterface iInputInterface; //アタッチできない？

    public event Action OnInputKeyRight;
    public event Action OnInputKeyLeft;
    public event Action OnInputKeyBottom;
    public event Action OnInputKeyFront;

    public event Action OnOpenMenu;

    public event Action OnHighScoreData;

    
    

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
        iInputInterface.InputKey();


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
        int direction = iInputInterface.InputKey();
        switch (direction)
        {
            case 1:
                OnInputKeyRight();
                break;

            case 2:
                OnInputKeyLeft();
                break;

            case 3:
                OnInputKeyFront();
                break;

            case 4:
                OnInputKeyBottom();
                break;

            case 0:
                //int型関数の処理の都合で0を書いちゃったけどどうしようか
                Debug.Log("どれにも当てはまらない");
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
