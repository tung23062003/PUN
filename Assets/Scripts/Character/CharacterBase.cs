using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour, ICharacter, IDamageable, IAttackSource
{
    [SerializeField] protected string id;
    //[SerializeField] protected Team team = Team.Neutral;
    public CharacterStats Stats;


    public string Id => id;
    public Vector3 Position => transform.position;
    //public Team Team => team;
    public Health health;
    public GameObject SourceObject => gameObject;
    public PhotonView photonView;


    protected virtual void Awake()
    {
        // default stats - in real project inject via factory or builder
        //stats = new CharacterStats(1, 100f, 5f, 10f, 0);
        photonView = GetComponent<PhotonView>();
    }

    protected virtual void Start()
    {
        gameObject.name = gameObject.name.Replace("(Clone)", "");
        gameObject.name = $"{gameObject.name}-{photonView.InstantiationId}";
    }


    public virtual void TakeDamage(DamageInfo dmg)
    {
        
    }

    public virtual void Heal(float amount)
    {
        //stats.CurrentHP = Mathf.Min(stats.MaxHP, stats.CurrentHP + amount);
    }


    protected virtual void Die()
    {
        // die logic - drop loot, network notify
        Debug.Log($"{name} died.");
        Destroy(gameObject);
    }

    public void Move()
    {
        
    }

    public void Attack()
    {
        
    }

    public void Damaged()
    {
        
    }
}
