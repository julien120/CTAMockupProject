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

    public void Initialize()
    {        
        menuWindowView.OnKeyOff += menuWindowModel.CannotInputKey;
        menuWindowModel.OnKeyOff +=()=> OnKeyOff?.Invoke();
        menuWindowView.OnKeyOn += menuWindowModel.CanInputKey;
        menuWindowModel.OnKeyOn  +=()=> OnKeyOn?.Invoke();
    }


}
