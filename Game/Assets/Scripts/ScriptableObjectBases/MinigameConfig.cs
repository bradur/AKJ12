
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "MinigameConfig", menuName = "New MinigameConfig")]
public class MinigameConfig : ScriptableObject
{
    public MinigameTriggerOptions Options;
}