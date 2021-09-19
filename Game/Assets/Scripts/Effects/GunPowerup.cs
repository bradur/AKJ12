using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPowerup : MonoBehaviour
{
    [SerializeField]
    int[] requiredScore;

    [SerializeField]
    Color unavailableColor = Color.red;
    [SerializeField]
    Color availableColor = Color.green;
    [SerializeField]
    SpriteRenderer rend;

    private Gun playerGun;

    int level = 0;
    // Start is called before the first frame update
    MinigameInfo powerupInfo;
    void Start()
    {
        playerGun = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Gun>();
        ShowPowerUpInfo();
    }

    private void ShowPowerUpInfo() {
        if (requiredScore.Length <= level) {
            return;
        }
        if (powerupInfo == null) {
            powerupInfo = WorldUI.main.GetMinigameInfo($"Unlock with {requiredScore[level]} score", transform.position + Vector3.one, transform);
        }
        else {
            powerupInfo.SetText($"Unlock with {requiredScore[level]} score");
            powerupInfo.transform.position = transform.position + Vector3.one;
        }
        if (!powerupInfo.IsShown) {
            powerupInfo.Show();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (requiredScore.Length <= level)
        {
            powerupInfo.Hide();
            Destroy(gameObject);
        }

        if (canUpgrade())
        {
            powerupInfo.SetText("Unlock available! Walk here to unlock!");
            rend.color = availableColor;
        }
        else
        {
            rend.color = unavailableColor;
        }
    }

    public void PowerUp()
    {
        level++;
        ShowPowerUpInfo();
        playerGun.SetBulletsPerShot(level * 2 + 1);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (requiredScore.Length <= level)
        {
            return;
        }

        if (collider.tag == "Player")
        {
            if (!canUpgrade())
            {
                return;
            }

            PowerUp();
            SoundManager.main.PlaySound(GameSoundType.Pickup);
            PoppingTextOptions poppingTextOptions = new PoppingTextOptions();
            poppingTextOptions.Position = transform.position;
            poppingTextOptions.Text = $"Weapon Boosted!";
            poppingTextOptions.Color = Color.green;
            WorldUI.main.ShowPoppingText(poppingTextOptions);
        }
    }

    private bool canUpgrade()
    {
        if (requiredScore.Length <= level)
        {
            return false;
        }
        return requiredScore[level] <= UIScore.main.Score;
    }
}
