using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuWindowModel : MonoBehaviour
{
    public event Action<bool> OnKeyOff;

    /// <summary>
    /// メニューを開いているときはユーザー入力があってもキー入力できないようにする
    /// </summary>
    public void CannotInputKey()
    {
        OnKeyOff(true);
    }
}
