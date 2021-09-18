using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIConfig", menuName = "New AIConfig")]
public class AIConfig : ScriptableObject
{
    public IdleRoutine.Type IdleType = IdleRoutine.Type.SEEK_CENTER;
    public AttackRoutine.Type AttackType = AttackRoutine.Type.DUMB_BLASTER;
    public Faction Faction = Faction.ENEMY;
    public float AggroRange = 3;
    public float MoveSpeed = 5;
    public float TurnSpeed = 180;
    public float FireRate = 2;
    public GunConfig GunConfig;
}
