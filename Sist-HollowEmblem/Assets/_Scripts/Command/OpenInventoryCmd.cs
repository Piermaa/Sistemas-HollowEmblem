using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInventoryCmd : ICommand
{
    private UIDisplayer _uiDisplayer;
    
    public OpenInventoryCmd(UIDisplayer uiDisplayer)
    {
        _uiDisplayer = uiDisplayer;
    }
    public void Do()
    {
        _uiDisplayer.SetDisplayState(DisplayState.Inventory);
    }
}
