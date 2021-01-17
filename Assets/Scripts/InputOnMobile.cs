using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class InputOnMobile : IInputInterface
{
    /// <summary>
    /// 押した場所と最後に話した場所の座標差で上下左右の出力を変更する
    /// </summary>
    /// <returns></returns>
    int IInputInterface.InputKey()
    {
        if (Input.touchCount > 0)
        {
            // タッチ情報の取得
            Touch touch = Input.GetTouch(0);

            Vector3 startPos = Input.GetTouch(0).position;
            if (touch.phase == TouchPhase.Began)
            {
                //押した時の値をどうしよう
                Debug.Log("押した瞬間");
            }
            
            if (touch.phase == TouchPhase.Ended)
            {
             Vector3 endPos = Input.GetTouch(0).position;
                //右
                if (endPos.x > startPos.x)
                {
                    return 1;
                }

                //左
                else if (endPos.x < startPos.x)
                {
                    return 2;
                }

                //上
                else if (endPos.y > startPos.y)
                {
                    return 3;
                }

                //下
                else if (endPos.y < startPos.y)
                {
                    return 4;
                }
            }

          
        }

        return 0;
    }
}