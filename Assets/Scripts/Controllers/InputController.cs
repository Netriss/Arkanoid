using System;
using UnityEngine;

public class InputController : MonoBehaviour, IInputProvider
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region IInputProvider
    
    public float InputX { get; private set; }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region public members
    
    public Action OnFire;
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    #region MonoDevelop

    private void FixedUpdate()
    {
        float mouse = Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1);
        float keyboard = Input.GetAxis("Horizontal");

        InputX = mouse;
        if(Mathf.Abs(keyboard) > Mathf.Abs(mouse))
        {
            InputX = keyboard;
        }
        
        if(Input.GetButton("Fire"))
        {
            Utils.InvokeAction(OnFire);
        }
    }

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}
