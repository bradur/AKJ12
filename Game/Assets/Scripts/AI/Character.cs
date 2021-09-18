using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Faction Faction
    {
        get; private set;
    }

    public bool Dead
    {
        get; private set;
    }

    public void Init(Faction faction)
    {
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
        Debug.Log("OUCH");
    }

    void Kill()
    {
        Dead = true;
        CharacterManager.Main.unregister(this);
    }
}

public enum Faction
{
    PLAYER,
    ENEMY
}
