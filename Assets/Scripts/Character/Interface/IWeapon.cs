using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    string WeaponId { get; }
    bool CanAttack(ICharacter user);
    void Attack(CharacterBase user, IAttackSource src, Vector3 targetPosition);
}
