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
        
        health -= damage;
        if (health <= 0)
        {
            Kill();
        }
    }

    void Kill()
    {
        Dead = true;
        CharacterManager.Main.unregister(this);
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
