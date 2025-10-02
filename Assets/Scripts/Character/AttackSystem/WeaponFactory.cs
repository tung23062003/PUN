using System.Collections;
using UnityEngine;

public class WeaponFactory : MonoBehaviour
{
    public static IWeapon Create(WeaponDataSO data)
    {
        switch (data.weaponType)
        {
            case WpnType.Melee:
                return new MeleeWeapon(data);
            case WpnType.Ranged:
                return new RangedWeapon(data);
                // thêm case khác khi mở rộng
        }
        return null;
    }
}