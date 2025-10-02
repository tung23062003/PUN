using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageInfo
{
    public float Amount;
    public DamageType Type;
    public IAttackSource Source;
    public bool IsCritical;
    public DamageInfo(float amount, DamageType type, IAttackSource source, bool isCrit = false)
    {
        Amount = amount; Type = type; Source = source; IsCritical = isCrit;
    }
}
