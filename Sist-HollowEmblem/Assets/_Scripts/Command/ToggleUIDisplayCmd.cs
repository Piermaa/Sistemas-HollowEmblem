using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUIDisplayCmd : ICommand
{
    private UIDisplayer _uiDisplayer;
    
    public ToggleUIDisplayCmd(UIDisplayer uiDisplayer)
    {
        _uiDisplayer = uiDisplayer;
    }

    public void Do()
    {
       _uiDisplayer.ToggleUI();
    }
}
