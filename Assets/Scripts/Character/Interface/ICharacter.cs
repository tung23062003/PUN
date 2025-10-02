using UnityEngine;
public interface ICharacter
{
    string Id { get; }
    Vector3 Position { get; }
    //Team Team { get; }
    //void ReceiveDamage(DamageInfo dmg);
    //void Heal(float amount);
    //void Move();
    //void Attack();
    //void Damaged();
}

public enum Team { Neutral, Player, Enemy }