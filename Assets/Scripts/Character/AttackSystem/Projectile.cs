using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private IAttackSource source;
    private ICharacter character;
    private float damage;
    private float maxDistance;
    private DamageType damageType;
    private Vector3 origin;
    private Vector3 dir;

    public void Initialize(ICharacter character, IAttackSource src, float damage, float maxDistance, DamageType dt)
    {
        this.character = character; this.source = src; this.damage = damage; this.maxDistance = maxDistance; this.damageType = dt;
    }
    public void LaunchTowards(Vector3 target)
    {
        origin = transform.position;
        dir = (target - origin).normalized;
    }
    void Update()
    {
        transform.position += dir * Time.deltaTime * 8f;
        if (Vector3.Distance(origin, transform.position) > maxDistance) Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        var dmgable = other.GetComponent<IDamageable>();
        var charBase = other.GetComponent<CharacterBase>();
        if (dmgable != null && charBase != null)
        {
            dmgable.TakeDamage(new DamageInfo(damage, damageType, source, charBase));
            Destroy(gameObject);
        }
    }
}
