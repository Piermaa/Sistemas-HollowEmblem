using UnityEngine;
public enum DisplayState
{
    None, Map, Inventory
}

public class UIDisplayer : MonoBehaviour
{
    public bool IsDisplayOpen => _currentDisplayState != DisplayState.None;
    
    private DisplayState _currentDisplayState;
    private DisplayState _previousDisplayState=DisplayState.Inventory;
    private Animator _cortanaAnimator;
    private void Awake()
    {
        _cortanaAnimator = GetComponent<Animator>();
    }

    private void ChangeState(DisplayState newDisplayState)
    {
        _previousDisplayState = _currentDisplayState;
        _currentDisplayState = newDisplayState;
    }

    public void SetDisplayState(DisplayState newDisplayState)
    {
        if (newDisplayState==_currentDisplayState)
        {
            CloseDisplay();
        }
        else
        {
            if (_currentDisplayState==DisplayState.None)
            {
                _cortanaAnimator.SetTrigger("Appear");
                ChangeState(newDisplayState);
            }
            else
            {
                ChangeState(newDisplayState);
                ShowDisplay();
            }
        }
    }

    public void ToggleUI()
    {
        if (_currentDisplayState==DisplayState.None)
        {
            if (_previousDisplayState==DisplayState.None)
            {
                return;
            }
            SetDisplayState(_previousDisplayState);
        }
        else
        {
            CloseDisplay();
        }
    }

    public void ShowDisplay()
    {
        switch (_currentDisplayState)
        {
            case DisplayState.Inventory:
                UIManager.Instance.OpenInventory();
                break;
            case DisplayState.Map:
                UIManager.Instance.OpenMap();
                break;
        }
        _cortanaAnimator.ResetTrigger("Disappear");
        _cortanaAnimator.ResetTrigger("Appear");
    }

    public void CloseDisplay()
    {
        UIManager.Instance.CloseUI();
        _cortanaAnimator.SetTrigger("Disappear");
        ChangeState(DisplayState.None);
    }
}
