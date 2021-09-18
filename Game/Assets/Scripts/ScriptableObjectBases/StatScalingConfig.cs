using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatScalingConfig", menuName = "New StatScalingConfig")]
public class StatScalingConfig : ScriptableObject
{
    public float Health;
    public float Damage;
    public float Speed;

    public ScalingStat getRandomStat()
    {
        ScalingStat stat = (ScalingStat)Mathf.FloorToInt(Random.Range(0, 3));
        return stat;
    }

    public float getStatScale(ScalingStat stat)
    {
        if (stat == ScalingStat.Health)
        {
            return Health;
        }
        else if(stat == ScalingStat.Speed) {
            return Speed;
        }
        else if(stat == ScalingStat.Damage) {
            return Damage;
        }

        return 0f;
    }
}

public enum ScalingStat
{
    Health, Damage, Speed
}