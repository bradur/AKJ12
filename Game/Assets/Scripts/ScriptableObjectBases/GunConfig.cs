using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunConfig", menuName = "New GunConfig")]
public class GunConfig : ScriptableObject
{
    public float MinDamage = 1;
    public float MaxDamage = 1;
    public float FireRate = 1;
    public float Range = 3;
    public float Inaccuracy = 10;
}
