using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static PauseMenu main;

    private bool isOpen = false;
    public bool IsOpen { get { return isOpen; } }

    private void Awake()
    {
        main = this;
    }

    [SerializeField]
    private GameObject container;

    private InputConfig inputConfig;
    void Start()
    {
        inputConfig = Configs.main.InputConfig;
    }

    public void Close()
    {
        Crosshair.main.Lock();
        isOpen = false;
        container.SetActive(false);
    }
    public void Open()
    {
        Crosshair.main.Unlock();
        UITimer.main.Pause();
        isOpen = true;
        container.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("movement_and_stuff");
    }
    public void Continue()
    {
        UITimer.main.Unpause();
        Time.timeScale = 1f;
        Close();
    }
    public void OpenMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("mainMenu");
    }

    void Update()
    {
        if (isOpen)
        {
            if (inputConfig.GetKeyDown(GameAction.MenuContinue) || Input.GetKeyDown(KeyCode.Joystick1Button7))
            {
                Continue();
            }
            if (inputConfig.GetKeyDown(GameAction.MenuOpenMainMenu))
            {
                OpenMainMenu();
            }
            if (inputConfig.GetKeyDown(GameAction.MenuRestart))
            {
                Restart();
            }
        }
        else
        {
            bool openMenuKeyDown = Input.GetKeyDown(KeyCode.Joystick1Button7) || inputConfig.GetKeyDown(GameAction.OpenPauseMenu);
            if (!GameOverMenu.main.IsOpen && openMenuKeyDown && !isOpen)
            {
                Debug.Log("Opening menu!");
                Open();
            }
        }
    }
}
