using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool pauseOn = true;
    [SerializeField] Canvas canvas;
    [SerializeField] Canvas pauseCanvas;

    private void Start()
    {
        Resume();
    }

    void Update()
    {
        DoPauseOn();
    }

    public void DoPauseOn()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseOn)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Resume();
            }

            else if (!pauseOn)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Pause();
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseOn = false;
        canvas.enabled = true;
        pauseCanvas.enabled = false;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseOn = true;
        canvas.enabled = false;
        pauseCanvas.enabled = true;
    }
}