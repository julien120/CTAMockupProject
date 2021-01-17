using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ここでユーザー入力を取得してインターフェースを介してInGameViewに伝える
/// </summary>
class InputOnPC : IInputInterface
{
     int IInputInterface.InputKey()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            return 1;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            return 3;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            return 4;
        }

        return 0;
    }
}
