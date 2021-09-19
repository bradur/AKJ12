using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOptions : MonoBehaviour
{

    [SerializeField]
    private UIToggleButton musicToggleButton;
    [SerializeField]
    private UIToggleButton screenFlashToggleButton;
    [SerializeField]
    private UIToggleButton screenShakeToggleButton;

    void Start()
    {
        musicToggleButton.Refresh();
        screenShakeToggleButton.Refresh();
        screenFlashToggleButton.Refresh();
    }

    public void ToggleKey(string key)
    {
        int toggleValue = PlayerPrefs.GetInt(key, 1);
        PlayerPrefs.SetInt(key, toggleValue == 1 ? 0 : 1);
    }

    public void ToggleMusic()
    {
        ToggleKey("music");
        musicToggleButton.Refresh();
        if (SoundManager.main != null)
        {
            SoundManager.main.Refresh();
        }
    }
    public void ToggleScreenShake()
    {
        ToggleKey("screenShake");
        screenShakeToggleButton.Refresh();
        if (ScreenShakeEffect.main != null)
        {
            ScreenShakeEffect.main.Refresh();
        }
    }
    public void ToggleScreenFlash()
    {
        ToggleKey("screenFlash");
        screenFlashToggleButton.Refresh();
        if (ScreenFlashEffect.main != null)
        {
            ScreenFlashEffect.main.Refresh();
        }
    }
}
