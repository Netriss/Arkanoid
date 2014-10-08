﻿using UnityEngine;
using System;

[RequireComponent(typeof(SpriteRenderer))]
public class Brick : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region public methods
    
    public void Init(Action<GameObject> onDestroy)
    {
        _onDestroy = onDestroy;
    }
    
    public void Destroy()
    {
        Utils.InvokeAction<GameObject>(_onDestroy, gameObject);
        GameObject.Destroy(gameObject);
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region private members
    
    private Action<GameObject> _onDestroy;
    
    #endregion

    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}
