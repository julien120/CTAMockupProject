using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuWindow : MonoBehaviour
{
    [SerializeField] Button closeButton;

    /// <summary>
    /// Windowを表示する
    /// TODO:OpenWindow中はセルの移動入力は受け付けない。絶対false。
    /// </summary>
    public void OpenWindow()
    {
        //ボタンを押すと、このスクリプトがアタッチされているgameObjectが表示されるだけ
        gameObject.SetActive(true);
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
    ///Action型変数をここに書いて、それに当たる処理をmodelに書く。
    ///リスタート機能とscene移動は異なるからダメ?
    ///modelにはスタート時の描画とスコアを0にする処理を書く
    ///</summary>

}
