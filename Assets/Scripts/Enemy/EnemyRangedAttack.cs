using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float bulletForce;
    [SerializeField] private float minRange;
    [SerializeField] private float maxRange;

    public GameObject attack;

    private float _angle;
    private float _shootCooldown;
    private float _distanceToPlayer;

    private Transform _target;
    private Vector2 _direction;
    private GameObject _bulletInstance;
    private Rigidbody2D _rb;

    void Start()
    {
        _shootCooldown = 0;
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        //Checks if distance to player is within range and if entity can shoot
        _distanceToPlayer = Vector2.Distance(transform.position, _target.position);
        if (Mathf.Abs(_distanceToPlayer) <= maxRange && Mathf.Abs(_distanceToPlayer) >= minRange)
        {
            _shootCooldown += Time.deltaTime;
            if (_shootCooldown > attackCooldown)
            {
                _shootCooldown = 0f;
                RangedAttack();
            }
        }
    }

    public void RangedAttack()
    {
        _direction = _target.position - transform.position;
        _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

        _bulletInstance = Instantiate(attack, transform.position, Quaternion.Euler(0f, 0f, _angle - 90));//Adds bullet to world
        _rb = _bulletInstance.GetComponent<Rigidbody2D>();
        _rb.AddForce(_direction.normalized * bulletForce, ForceMode2D.Impulse);
    }
}
