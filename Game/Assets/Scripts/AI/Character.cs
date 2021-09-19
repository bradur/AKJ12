using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterConfig config;
    private float health;
    private bool targetable = true;

    public Faction Faction
    {
        get; private set;
    }

    public bool Dead
    {
        get; private set;
    }

    public void Init(Faction faction, CharacterConfig config)
    {
        this.config = config;
        health = config.Health;
        if (gameObject.tag == "Player") {
            Debug.Log($"Setting hp for player: {health}");
            UIHealth.main.SetHealth((int)health);
        }
        Faction = faction;
        CharacterManager.Main.register(this);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Hurt(float damage)
    {
        if(!targetable) return;

        AddHealth(-(int)damage);
        if (health <= 0)
        {
            Kill();
        }
    }

    public bool AddHealth(int addedHealth) {
        if (addedHealth > 0 && health >= config.Health) {
            return false;
        }
        health += addedHealth;
        health = Mathf.Clamp(health, 0, config.Health);
        if (gameObject.tag == "Player"){
            if (addedHealth > 0) {
                SoundManager.main.PlaySound(GameSoundType.Heal);
            }
            UIHealth.main.SetHealthAnimated((int)health);
        }
        return true;
    }

    void Kill()
    {
        Dead = true;
        CharacterManager.Main.unregister(this);
        if (gameObject.tag == "Player"){
            GameOverMenu.main.Open();
        }
    }

    public void ScaleHealth(float scale)
    {
        health = health * scale;
    }

    public void SetTargetable(bool t)
    {
        targetable = t;
    }

    public bool GetTargetable()
    {
        return targetable;
    }
}

public enum Faction
{
    PLAYER,
    ENEMY
}
