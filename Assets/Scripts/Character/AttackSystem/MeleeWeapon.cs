using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IWeapon
{
    public string WeaponId => data != null ? data.weaponId : "melee";
    private WeaponDataSO data;
    public MeleeWeapon(WeaponDataSO data) { this.data = data; }

    public bool CanAttack(ICharacter user)
    {
        // can add stamina/cooldown checks
        return true;
    }


    public void Attack(CharacterBase user, IAttackSource src, Vector3 targetPosition)
    {
        DrawDebugCircle(user.Position, data.range, 32, Color.red, 0.5f);

        // simple hit-scan: find nearest enemy in range
        Collider2D[] hits = Physics2D.OverlapCircleAll(user.Position, data.range);
        foreach (var hit in hits)
        {
            var h = hit;
            var dmgable = h.GetComponent<IDamageable>();
            var charSource = h.GetComponent<CharacterBase>();
            if (dmgable != null && charSource != null && charSource != user)
            {
                var dmg = new DamageInfo(data.damage + user.Stats.AttackPower, data.damageType, src);
                //dmgable.TakeDamage(dmg);
                //charSource.health.TakeDamage(dmg.Amount);
                if (user.photonView.IsMine)
                {
                    charSource.health.TakeDamage(dmg.Amount);

                    charSource.photonView.RPC(nameof(Health.TakeDamage), RpcTarget.Others, dmg.Amount);
                }
                Debug.Log($"{charSource.gameObject.name} takes {dmg.Amount} damage from {user.gameObject.name}");
                //break; // one-hit
            }
        }
    }

    private void DrawDebugCircle(Vector2 center, float radius, int segments, Color color, float duration = 0.2f)
    {
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector2(Mathf.Cos(0), Mathf.Sin(0)) * radius;

        for (int i = 1; i <= segments; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector3 nextPoint = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            Debug.DrawLine(prevPoint, nextPoint, color, duration);
            prevPoint = nextPoint;
        }
    }
}
