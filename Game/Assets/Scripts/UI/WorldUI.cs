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

    [SerializeField]
    private Transform infoContainer;
    public void ShowPoppingText(PoppingTextOptions options)
    {
        PoppingText poppingText = Prefabs.Get<PoppingText>();
        poppingText.transform.SetParent(poppingTextContainer, false);
        poppingText.Initialize(options);
    }

    private Minigame minigame;

    public Minigame GetMinigame() {
        if (minigame == null) {
            minigame = Prefabs.Get<Minigame>();
            minigame.transform.SetParent(minigameContainer, false);
        }
        return minigame;
    }

    public MinigameInfo GetMinigameInfo(string text, Vector2 position, Transform target = null) {
        MinigameInfo info = Prefabs.Get<MinigameInfo>();
        info.SetText(text);
        info.SetTarget(target);
        info.transform.SetParent(infoContainer, false);
        info.transform.position = position;
        return info;
    }
}
