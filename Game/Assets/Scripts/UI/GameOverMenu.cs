using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{

    public static GameOverMenu main;

    private void Awake()
    {
        main = this;
    }


    [SerializeField]
    private GameObject container;
    [SerializeField]
    private GameObject hud;
    [SerializeField]
    private Text txtScore;

    [SerializeField]
    private Text txtTimer;

    public bool IsOpen { get; private set; }

    private InputConfig inputConfig;

    void Start() {
        inputConfig = Configs.main.InputConfig;
        container.SetActive(false);
    }


    public void Open()
    {
        UITimer.main.Pause();
        IsOpen = true;
        PauseMenu.main.Close();
        Crosshair.main.Unlock();
        Time.timeScale = 0f;
        txtScore.text = $"Score: {UIScore.main.Score.ToString()}";
        txtTimer.text = $"Time: {UITimer.main.GetFormattedTime()}";
        hud.SetActive(false);
        container.SetActive(true);
    }

    public void OpenMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("mainMenu");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("movement_and_stuff");
    }

    private void Update() {
        if (IsOpen) {
            if (inputConfig.GetKeyDown(GameAction.MenuOpenMainMenu)) {
                OpenMainMenu();
            }
            if (inputConfig.GetKeyDown(GameAction.MenuRestart)) {
                Restart();
            }
        }
    }
}
