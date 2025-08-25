using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _meleeEnemy;
    [SerializeField] private GameObject _rangedEnemy;

    public void SpawnEnemies(RoomLayout roomLayout) 
    {
        Instantiate(_rangedEnemy, Vector3.zero, Quaternion.identity);
        Instantiate(_meleeEnemy, Vector3.zero, Quaternion.identity);
    }
}
