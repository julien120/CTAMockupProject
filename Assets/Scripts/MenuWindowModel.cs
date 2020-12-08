using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuWindowModel : MonoBehaviour
{
   public event Action OnKeyOff;
   public event Action OnKeyOn;
    public event Action OnRestart;


    /// <summary>
    /// メニューを開いているときはユーザー入力があってもキー入力できないようにする
    /// </summary>
    public void CannotInputKey()
    {
        OnKeyOff();
    }

    public void CanInputKey()
    {
        OnKeyOn();
    }

    public void Restart()
    {
        OnRestart();
    }
}
