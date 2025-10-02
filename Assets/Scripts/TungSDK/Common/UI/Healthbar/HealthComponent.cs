using UnityEngine.Events;

/// <summary>
/// Abstract base class of the health component
/// </summary>
public abstract class HealthComponent : TickBehaviour
{
    public UnityAction<float> OnHealthChanged { get; set; }
    public UnityAction OnHealthEmpty { get; set; }
    public abstract float MaxHealth { get; set; }
    public abstract float CurrentHealth { get; set; }
    public abstract bool IsAlive { get; }

    public abstract void AddHealth(float amount);
    public abstract void TakeDamage(float damage);
    public abstract void SetHealth(float health);
}