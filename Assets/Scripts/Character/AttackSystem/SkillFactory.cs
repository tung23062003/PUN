using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFactory : MonoBehaviour
{
    public static ISkill Create(SkillDataSO data)
    {
        switch (data.skillType)
        {
            case SkillType.FIREBALL:
                return new FireballSkill(data);
            case SkillType.HEAL:
                return new HealSkill(data);
                // thêm case khác khi mở rộng
        }
        return null;
    }
}
