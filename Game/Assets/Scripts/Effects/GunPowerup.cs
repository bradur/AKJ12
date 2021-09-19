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
    void Start()
    {
        playerGun = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Gun>();
    }

    // Update is called once per frame
    void Update()
    {
        if (requiredScore.Length <= level)
        {
            Destroy(gameObject);
        }

        if (canUpgrade())
        {
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
                var options = new PoppingTextOptions();
                options.Position = transform.position;
                options.Color = Color.red;
                options.Text = "Unlock with " + requiredScore[level] + " score";
                WorldUI.main.ShowPoppingText(options);
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
