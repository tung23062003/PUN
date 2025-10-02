using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "SO/Weapon Data", order = 1)]
public class WeaponDataSO : ScriptableObject
{
    public string weaponId;
    public float damage;
    public float range;
    public float cooldown;
    public DamageType damageType;
    public WpnType weaponType;
}

public enum DamageType { Physical, Magical, True }
public enum WpnType { Melee, Ranged }