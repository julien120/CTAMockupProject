using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuWindowPresenter : MonoBehaviour
{
    private MenuWindowView menuWindowView;
    private MenuWindowModel menuWindowModel;

    void Start()
    {
        menuWindowView = GetComponent<MenuWindowView>();
        menuWindowModel = GetComponent<MenuWindowModel>();

        //bool型を使おうとするとエラーが
        //menuWindowView.OnKeyOff += menuWindowModel.CannotInputKey;
    }


}
