using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform shotPoint;
    public GameObject bullet;
    public float bulletForce;
    private float shootTimer = 0f;
    public float shotCooldown; // time between shots, in seconds
    private float timer;
    private enum EnemyState { Idle, MoveCloser, MoveAway, Attack }
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

        // Decide behavior state
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
        else
        {
            currentState = EnemyState.Attack;
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

            case EnemyState.Attack:
                shootTimer -= Time.deltaTime;
                if (shootTimer <= 0f)
                {
                    Shoot();
                    shootTimer = shotCooldown; // reset the cooldown
                }
                break;
        }
    }

    void MoveInDirection(Vector2 direction)
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    void Shoot()
    {
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        shotPoint.rotation = Quaternion.Euler(0f, 0f, angle - 90);
        GameObject bulletInstance = Instantiate(bullet, shotPoint.position, shotPoint.rotation);
        Rigidbody2D rb = bulletInstance.GetComponent<Rigidbody2D>();
        rb.AddForce(shotPoint.up * bulletForce, ForceMode2D.Impulse);
    }
}