using Photon.Pun;
using UnityEngine;

public class Health : HealthComponent, IPunObservable
{
    [Tooltip("The max amount of health that can be assigned")]
    [SerializeField]
    private float maxHealth = 100.0f;

    public override float MaxHealth { get => maxHealth; set => maxHealth = value; }

    public override float CurrentHealth { get; set; } = 0.0f;

    public override bool IsAlive => CurrentHealth > 0.0f;
    private bool healthDirty = false;

    protected override void Awake()
    {
        base.Awake();
        SetHealth(maxHealth);

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

    }

    [PunRPC]
    public override void SetHealth(float health)
    {
        UpdateHealth(Mathf.Clamp(health, 0, MaxHealth));
    }

    [PunRPC]
    public override void AddHealth(float amount)
    {
        if (!IsAlive) return;
        UpdateHealth(Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth));
    }

    [PunRPC]
    public override void TakeDamage(float damage)
    {
        if (!IsAlive) return;
        UpdateHealth(Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth));
    }

    private void UpdateHealth(float newHealth)
    {
        if (Mathf.Approximately(CurrentHealth, newHealth)) return;

        float previous = CurrentHealth;
        CurrentHealth = newHealth;
        float diff = CurrentHealth - previous;

        OnHealthChanged?.Invoke(diff);

        if (CurrentHealth <= 0.0f)
        {
            OnHealthEmpty?.Invoke();
        }

        //healthDirty = true;
    }

    [PunRPC]
    public void SendHealthToNewPlayer(float health)
    {
        CurrentHealth = health;
        UpdateHealth(CurrentHealth);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //if (stream.IsWriting)
        //{
        //    //if (healthDirty)
        //    //{
        //        stream.SendNext(CurrentHealth);
        //        //healthDirty = false;
        //    //}
        //}
        //else
        //{
        //    float newHealth = (float)stream.ReceiveNext();
        //    UpdateHealth(newHealth);
        //}
    }
}