using UnityEngine;

public class StatsPlayer : MonoBehaviour
{
    public int defence;
    public int evasion;
    public int vitality;
    public int power;
    public int attackSpeed;
    public int reloadSpeed;
    public int movementSpeed;

    private void Start()
    {
        defence = 0;
        evasion = 0;
        vitality = 3;
        power = 0;
        attackSpeed = 0;
        reloadSpeed = 0;
        movementSpeed = 0;
    } 
}


