using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUI : MonoBehaviour
{
    public static WorldUI main;
    void Awake()
    {
        main = this;
    }

    [SerializeField]
    private Transform poppingTextContainer;

    [SerializeField]
    private Transform minigameContainer;
    public void ShowPoppingText(PoppingTextOptions options)
    {
        PoppingText poppingText = Prefabs.Get<PoppingText>();
        poppingText.transform.SetParent(poppingTextContainer, false);
        poppingText.Initialize(options);
    }

    public Minigame CreateMinigame(Vector2 gamePosition) {
        Minigame minigame = Prefabs.Get<Minigame>();
        minigame.transform.SetParent(minigameContainer, false);
        minigame.SetPosition(gamePosition);
        return minigame;
    }
}
