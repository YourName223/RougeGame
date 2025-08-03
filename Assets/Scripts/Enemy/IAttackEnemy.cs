using UnityEngine;

public interface IAttackEnemy
{
    void Attack();
    void UpdateVariables(float knockbackPower, int dmg, Vector2 direction, float angle, float attackTimer, Vector3 position);
}