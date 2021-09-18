using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public static AIManager Main
    {
        get; private set;
    }

    public Transform CenterOfArena;

    private Transform _player;
    public Transform Player
    {
        get
        {
            setupPlayer();
            return _player;
        }
    }

    void Awake()
    {
        if (Main != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Main = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        setupPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void setupPlayer()
    {
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
}
