using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuWindowPresenter : MonoBehaviour
{
    [SerializeField] MenuWindowView menuWindowView;

    public event Action OnKeyOn;
    public event Action OnRestart;

    public void Initialize()
    {
        //メニューを閉めている時
        menuWindowView.OnKeyOn +=()=> OnKeyOn?.Invoke();

        //リスタートボタンを押した時
        menuWindowView.OnRestart += () => OnRestart?.Invoke();
    }

    public void OpenMenu()
    {
        //メニューを開いている時
        menuWindowView.OpenWindow();
    }

}
