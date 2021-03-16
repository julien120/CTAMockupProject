using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

public class MenuWindowView : MonoBehaviour
{
    private readonly Subject<Unit> restart = new Subject<Unit>();
    public IObservable<Unit> OnRestart => restart;

    private readonly Subject<Unit> keyOn = new Subject<Unit>();
    public IObservable<Unit> OnKeyOn => keyOn;


    [SerializeField] private GameObject menu;

    /// <summary>
    /// Windowを非表示にする
    /// </summary>
    public void CloseWindow()
    {
        gameObject.SetActive(false);
        keyOn.OnNext(Unit.Default);
    }

    /// <summary>
    /// メニューを開く
    /// </summary>
    public void OpenWindow()
    {
        menu.SetActive(true);
    }

    ///<summary>
    ///リスタート機能を実装する
    ///modelにはスタート時の描画とスコアを0にする処理を書く
    ///</summary>

    public void RestartGame()
    {
        //リスタート処理{再描画とスコア０が含まれているか？分割するか}
        Debug.Log("リスタートボタン押した");
        restart.OnNext(Unit.Default);


    }

}
