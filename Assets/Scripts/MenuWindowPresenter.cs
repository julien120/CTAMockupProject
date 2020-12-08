using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuWindowPresenter : MonoBehaviour
{
    [SerializeField] MenuWindowView menuWindowView;
    [SerializeField] MenuWindowModel menuWindowModel;
    public event Action OnKeyOff;
    public event Action OnKeyOn;
    public event Action OnRestart;

    public void Initialize()
    {
        //メニューを開いているとき
        menuWindowView.OnKeyOff += menuWindowModel.CannotInputKey;
        menuWindowModel.OnKeyOff +=()=> OnKeyOff?.Invoke();

        //メニューを閉めている時
        menuWindowView.OnKeyOn += menuWindowModel.CanInputKey;
        menuWindowModel.OnKeyOn  +=()=> OnKeyOn?.Invoke();

        //リスタートボタンを押した時
        menuWindowView.OnRestart += menuWindowModel.Restart;
        menuWindowModel.OnRestart += () => OnRestart?.Invoke();


    }


}
