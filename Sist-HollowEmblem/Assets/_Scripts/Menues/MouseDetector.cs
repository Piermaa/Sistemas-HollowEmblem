using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseDetector : MonoBehaviour
{
    public PopUp popUp;

    private void OnMouseEnter()
    {
        if (popUp!=null)
        {
            popUp.ActivatePanel();
        }
    
    }

    private void OnMouseExit()
    {
        if (popUp != null)
        {
            popUp.DeactivatePanel();
        }
    }
}
