
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "InputConfig", menuName = "New InputConfig")]
public class InputConfig : ScriptableObject
{
    [SerializeField]
    private List<HotKey> hotKeys = new List<HotKey>();

    private HotKey FindKey(GameAction action)
    {
        HotKey hotKey = hotKeys.First(key => key.Action == action);
        if (hotKey == null)
        {
            throw new System.Exception($"Action '{action}' doesn't have a corresponding key!");
        }
        return hotKey;
    }

    public KeyCode GetKeyByAction(GameAction action) {
        return FindKey(action).Key;
    }

    public bool GetKeyUp(GameAction action)
    {
        return Input.GetKeyUp(FindKey(action).Key);
    }

    public bool GetKeyDown(GameAction action)
    {
        return Input.GetKeyDown(FindKey(action).Key);
    }
}

public enum GameAction
{
    None,
    MiniGameIndicatorStart,
    MiniGameIndicatorStop,
}

[System.Serializable]
public class HotKey
{
    public GameAction Action;
    public KeyCode Key;
}
