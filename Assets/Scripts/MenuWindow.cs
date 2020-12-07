﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuWindow : MonoBehaviour
{
    [SerializeField] Button closeButton;
    public event Action OnRestart;
    public  event Action<bool> OnKeyOff;

    /// <summary>
    /// Windowを表示する
    /// TODO:OpenWindow中はセルの移動入力は受け付けない。絶対false。
    /// </summary>
    public void OpenWindow()
    {
        
        gameObject.SetActive(true);

        //TODO:OpenWindow中はセルの移動入力は受け付けない。絶対false。
        OnKeyOff(false);
       
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
    ///TODO:menueindow.csでボタンを押すと実行される処理を書く
    ///TODO:この処理はMVPに沿って行うこと
    ///Action型変数をmodelに書いて、それに当たる処理をこっちに書く。
    ///リスタート機能とscene移動は異なるからダメ?
    ///modelにはスタート時の描画とスコアを0にする処理を書く
    ///</summary>
    
    public void RestartButton()
    {
        //リスタート処理{再描画とスコア０が含まれているか？分割するか}
        Debug.Log("リスタートボタン押した");
        OnRestart();
        

    }

}
