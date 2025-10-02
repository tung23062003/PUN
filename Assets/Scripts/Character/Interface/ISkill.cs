using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    string Id { get; }
    Sprite Icon { get; }
    float Cooldown { get; }
    bool CanCast(ICharacter caster);
    void Cast(CharacterBase caster, Vector3 targetPosition);
}
