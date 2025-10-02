using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : MonoBehaviour, IWeapon
{
    public string WeaponId => weaponData.weaponId;
    private WeaponDataSO weaponData;
    public RangedWeapon(WeaponDataSO data) { this.weaponData = data; }
    public bool CanAttack(ICharacter user) => true;
    public void Attack(CharacterBase user, IAttackSource src, Vector3 targetPosition)
    {
        // spawn projectile - in real project use pooling
        var go = new GameObject("Projectile");
        var proj = go.AddComponent<Projectile>();
        proj.Initialize(user, src, weaponData.damage, weaponData.range, weaponData.damageType);
        go.transform.position = user.Position;
        proj.LaunchTowards(targetPosition);
    }
}
