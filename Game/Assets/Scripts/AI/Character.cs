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

    // Start is called before the first frame update
    void Start()
    {
        CharacterManager.Main.register(this);
    }

    // Update is called once per frame
    void Update()
    {
        
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
