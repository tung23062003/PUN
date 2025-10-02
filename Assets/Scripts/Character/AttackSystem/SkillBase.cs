using System.Collections;
using UnityEngine;

public class SkillBase : MonoBehaviour, ISkill
{
    public string Id => id;

    public Sprite Icon => icon;

    public float Cooldown => cooldown;

    protected string id;
    protected Sprite icon;
    protected float cooldown;


    public virtual bool CanCast(ICharacter caster)
    {
        return true;
    }

    public virtual void Cast(CharacterBase caster, Vector3 targetPosition)
    {
        
    }
}