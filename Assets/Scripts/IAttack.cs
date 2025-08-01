using UnityEngine;

public interface IAttack
{
    void Attack();
    void UpdateVariables(int dmg, float x, float angle, float attackTimer, Vector3 position);
}