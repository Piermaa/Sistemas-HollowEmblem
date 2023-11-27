using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public GameObject canvas;

    public GameObject panelMenu;
    public GameObject panelOptions;

    public GameManager gameManager;
    public static SceneChanger Instance;

    private void Awake()
    {
        Instance = this;
    }
    public void Start()
    {
        IniciarComponentes();
        canvas.SetActive(true);
        panelMenu.SetActive(true);
        if (panelOptions != null)
        {
            panelOptions.SetActive(false);
        }

        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        Pause();
    }

    public void IniciarComponentes()
    {
        canvas = GameObject.Find("Canvas");
    }

    public void Iniciar()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    public void ReturnToMainMenu()
    {
        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
        SceneManager.LoadScene(0, LoadSceneMode.Single);

    }

    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && panelOptions.activeInHierarchy == true)
        {
            Volver();
        }

        else if (Input.GetKeyDown(KeyCode.Escape) && panelOptions.activeInHierarchy == false)
        {
            Time.timeScale = 0f;
            Cursor.visible = true;
            panelOptions.SetActive(true);
        }
    }

    public void Volver()
    {
        Time.timeScale = 1f;
        panelMenu.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    public void Resume()
    {
        // GameManager has change
        gameManager.Resume();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#else
        Application.Quit();
#endif
    }

    public void GameOver()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}

