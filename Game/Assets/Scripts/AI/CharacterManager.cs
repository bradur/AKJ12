using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Main
    {
        get; private set;
    }

    private Dictionary<Faction, List<Character>> characters = new Dictionary<Faction, List<Character>>();

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

    public List<Character> charactersBy(Faction faction)
    {
        return getCharactersOrAddFaction(faction);
    }

    public void register(Character character)
    {
        var charactersByFaction = getCharactersOrAddFaction(character.Faction);
        charactersByFaction.Add(character);
    }

    public void unregister(Character character)
    {
        var charactersByFaction = getCharactersOrAddFaction(character.Faction);
        charactersByFaction.Remove(character);
    }

    private List<Character> getCharactersOrAddFaction(Faction faction)
    {
        if (!characters.ContainsKey(faction))
        {
            characters.Add(faction, new List<Character>());
        }
        List<Character> charactersByFaction;
        characters.TryGetValue(faction, out charactersByFaction);
        return charactersByFaction;
    }

}
