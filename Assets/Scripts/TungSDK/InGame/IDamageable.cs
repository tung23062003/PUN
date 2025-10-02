// Represents anything that can be damaged
namespace TungSDK
{
    public interface IDamageable
    {
        // Takes damage from source
        void TakeDamage(IDamageDealer source, Damage damage);
    }
}