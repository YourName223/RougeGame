using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    private enum EnemyState { Idle, MoveCloser, MoveAway}
    private EnemyState currentState = EnemyState.Idle;

    private Transform target;
    public float speed = 3f;

    public float range = 3f;    // Move away if closer than this
    public float aggroRange = 6f;   // Move closer if farther than this

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

        if (distanceToPlayer < range - 0.1f)
        {
            currentState = EnemyState.MoveAway;
        }
        else if (distanceToPlayer > aggroRange + 0.1f)
        {
            currentState = EnemyState.Idle;
        }
        else if (distanceToPlayer > range + 0.1f)
        {
            currentState = EnemyState.MoveCloser;
        }
        else if (distanceToPlayer < aggroRange)
        {
            currentState = EnemyState.Idle;
        }

        Vector2 direction = (target.position - transform.position).normalized;

        // Act based on range
        switch (currentState)
        {
            case EnemyState.MoveCloser:
                MoveInDirection(direction);
                break;

            case EnemyState.MoveAway:
                MoveInDirection(-direction);
                break;
        }
    }

    void MoveInDirection(Vector2 direction)
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }
}