// Represents anything that can deal damage
namespace TungSDK
{
    public interface IDamageDealer
    {
        // Deals damage to target
        void DealDamage(IDamageable target, Damage damage);
    }
}