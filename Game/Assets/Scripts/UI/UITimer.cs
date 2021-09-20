using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{

    public static UITimer main;
    [SerializeField]
    private Text txtTimer;
    public Timer timer {private get; set;}

    void Awake()
    {
        main = this;
    }

    void Start() {
        timer = new Timer();
    }

    public string GetFormattedTime() {
        return timer.GetString();
    }

    public void Pause() {
        timer.Pause();
    }

    public void Unpause() {
        timer.Unpause();
    }

    void Update()
    {
        if (timer != null) {
            txtTimer.text = timer.GetString();
        }
        if (!Application.isFocused && timer.IsRunning) {
            Debug.Log("Paused timer after unfocus.");
            timer.Pause();
        }
        else if (Application.isFocused && !timer.IsRunning && !PauseMenu.main.IsOpen && !GameOverMenu.main.IsOpen) {
            Debug.Log("Restarted timer after refocus.");
            timer.Unpause();
        }
    }
}
