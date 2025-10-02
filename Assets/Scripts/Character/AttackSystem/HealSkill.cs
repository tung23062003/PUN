using System.Collections;
using UnityEngine;

public class HealSkill : SkillBase
{
    private SkillDataSO data;


    public HealSkill(SkillDataSO data)
    {
        this.data = data;
    }

    public override bool CanCast(ICharacter caster)
    {
        return true;// check mana, etc
    }

    public override void Cast(CharacterBase caster, Vector3 targetPosition)
    {
        // Lấy base damage từ SkillData
        float baseDamage = data.baseDamage;

        // Lấy stat từ caster (VD cộng AttackPower)
        float totalDamage = baseDamage + caster.Stats.AttackPower;

        // Spawn projectile từ pool
        //var go = ProjectilePool.Instance.Spawn(caster.Position, Quaternion.identity);
        //if (go == null) return;

        //var proj = go.GetComponent<Projectile>();
        //proj.Initialize(caster, totalDamage, data.range, data.skillType);
        //proj.LaunchTowards(targetPosition);
    }
}