using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMapCmd : ICommand
{
    private UIDisplayer _uiDisplayer;
    
    public OpenMapCmd(UIDisplayer uiDisplayer)
    {
        _uiDisplayer = uiDisplayer;
    }
    public void Do()
    {
        _uiDisplayer.SetDisplayState(DisplayState.Map);
    }
}
