using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuWindowView : MonoBehaviour
{
    [SerializeField] Button closeButton;
    public event Action OnRestart;
    public event Action OnKeyOff;

    /// <summary>
    /// Windowを表示する
    /// TODO:OpenWindow中はセルの移動入力は受け付けない。絶対false。
    /// </summary>
    public void OpenWindow()
    {
        gameObject.SetActive(true);
        //TODO:InGameModelで(!変数){入力}にリンケージするアクション
        OnKeyOff();
    }

    /// <summary>
    /// Windowを非表示にする
    /// </summary>
    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    ///<summary>
    ///リスタート機能を実装する
    ///modelにはスタート時の描画とスコアを0にする処理を書く
    ///</summary>

    public void RestartButton()
    {
        //リスタート処理{再描画とスコア０が含まれているか？分割するか}
        Debug.Log("リスタートボタン押した");
        //OnRestart();


    }

}
