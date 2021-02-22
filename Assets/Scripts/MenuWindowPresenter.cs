using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class MenuWindowPresenter : MonoBehaviour
{
    [SerializeField] MenuWindowView menuWindowView;

    private readonly Subject<Unit> keyOn = new Subject<Unit>();
    public IObservable<Unit> OnKeyOn => keyOn;

    private readonly Subject<Unit> restart = new Subject<Unit>();
    public IObservable<Unit> OnRestart => restart;

    public void Initialize()
    {
        //メニューを閉めている時
        menuWindowView.OnKeyOn.Subscribe(_ => keyOn.OnNext(Unit.Default));

        //リスタートボタンを押した時
        menuWindowView.OnRestart.Subscribe(_ => restart.OnNext(Unit.Default));
    }

    public void OpenMenu()
    {
        //メニューを開いている時
        menuWindowView.OpenWindow();
    }

}
