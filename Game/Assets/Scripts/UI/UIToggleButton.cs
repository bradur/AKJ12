using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggleButton : MonoBehaviour
{
    [SerializeField]
    private string ToggleName = "Music";
    [SerializeField]
    private string PlayerPrefName = "music";

    [SerializeField]
    private Text txtValue;
    [SerializeField]
    private Image imgValue;

    public void Refresh() {
        int isOn = PlayerPrefs.GetInt(PlayerPrefName, 1);
        if (isOn == 1) {
            txtValue.text = $"{ToggleName}: ON";
            imgValue.color = Color.green;
        } else {
            txtValue.text = $"{ToggleName}: OFF";
            imgValue.color = Color.red;
        }
    }
}
