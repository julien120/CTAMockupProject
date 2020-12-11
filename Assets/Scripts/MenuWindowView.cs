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
    public event Action OnKeyOn;

    /// <summary>
    /// Windowを非表示にする
    /// </summary>
    public void CloseWindow()
    {
        gameObject.SetActive(false);
        OnKeyOn();
    }

    ///<summary>
    ///リスタート機能を実装する
    ///modelにはスタート時の描画とスコアを0にする処理を書く
    ///</summary>

    public void RestartGame()
    {
        //リスタート処理{再描画とスコア０が含まれているか？分割するか}
        Debug.Log("リスタートボタン押した");
        OnRestart();


    }

}
