using UnityEngine;

public class StatsPlayer : MonoBehaviour
{
    [HideInInspector] public int defence;
    [HideInInspector] public int evasion;
    [HideInInspector] public int vitality;
    [HideInInspector] public int power;
    [HideInInspector] public int attackSpeed;
    [HideInInspector] public int reloadSpeed;
    [HideInInspector] public int movementSpeed;

    private void Awake()
    {
        defence = 0;
        evasion = 0;
        vitality = 100;
        power = 0;
        attackSpeed = 0;
        reloadSpeed = 0;
        movementSpeed = 0;
    }
}


