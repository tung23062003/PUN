using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : CharacterBase
{
    [SerializeField] WeaponDataSO startingWeapon;
    private IWeapon equippedWeapon;
    //private SkillSystem skillSystem;
    private AnimationEvents AnimationEvents;

    protected override void Awake()
    {
        base.Awake();
        AnimationEvents = GetComponent<AnimationEvents>();
        // equip default weapon via factory
        equippedWeapon = WeaponFactory.Create(startingWeapon);
        //skillSystem = new SkillSystem(new MonoBehaviourScheduler(this));
    }

    protected override void Start()
    {
        base.Start();
        AnimationEvents.OnEvent += OnAnimationEvent;
        NetworkLauncher.Instance.OnPlayerEntered += OnPlayerEntered;
    }

    public void OnDestroy()
    {
        AnimationEvents.OnEvent -= OnAnimationEvent;
        NetworkLauncher.Instance.OnPlayerEntered -= OnPlayerEntered;
    }

    private void OnPlayerEntered(Player newPlayer)
    {
        if (photonView.IsMine)
        {
            photonView.RPC(nameof(Health.SendHealthToNewPlayer), newPlayer, health.CurrentHealth);
        }
    }

    private void OnAnimationEvent(string eventName)
    {
        switch (eventName)
        {
            case "Hit":
                equippedWeapon.Attack(this, this, transform.position);
                Debug.Log($"{gameObject.name} attack");
                break;
            default: return;
        }
    }


    void Update()
    {
        // input handling is local - maps to high level commands
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    target.z = 0;
        //    if (equippedWeapon.CanAttack(this)) equippedWeapon.Attack(this, this, target);
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        // call skill by id - sample
        //skillSystem.TryUseSkill(new FireballSkill(new SkillData { skillId = "fireball", baseDamage = 40, range = 6, cooldown = 3f }), this, transform.position + transform.up * 1f);
        //}
    }
}
