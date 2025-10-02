using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Data", menuName = "SO/Skill Data", order = 0)]
public class SkillDataSO : ScriptableObject
{
    public string id;
    public float baseDamage;
    public float range;
    public float cooldown;
    public Sprite icon;
    public GameObject effectPrefab;
    public SkillType skillType;
}

public enum SkillType
{
    NONE = 0,
    FIREBALL = 1,
    DEMACIA = 2,
    BUFF = 3,
    HEAL = 4,

}