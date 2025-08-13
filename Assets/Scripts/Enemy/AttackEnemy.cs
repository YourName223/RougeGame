using UnityEngine;
using System.Collections;

public class AttackEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _attackPrefab;
    [SerializeField] private int _dmg;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _attackTimer;
    [SerializeField] private float _maxRange;
    [SerializeField] private float _minRange;
    [SerializeField] private float _knockbackPower;

    private float _timer;
    private bool _canAttack = true;
    private float _angle;
    private float _distanceToPlayer;

    private IAttack _attackScript;
    private Transform _player;
    private Vector2 _direction;

    void Start()
    {
        _timer = _attackCooldown;
        _attackPrefab = Instantiate(_attackPrefab, transform.position, Quaternion.identity);
        _player = GameObject.FindWithTag("Player").transform;
        _attackScript = _attackPrefab.GetComponent<IAttack>();
    }

    void Update()
    {
        _distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        _direction = _player.position - transform.position;
        _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

        if (_direction.x < 0)
        {
            _angle -= 180;
        }

        _attackScript.UpdateVariables(_knockbackPower, _dmg, _direction, _angle, _attackTimer, transform.position);

        if (_timer <= _attackCooldown)
        {
            _timer += Time.deltaTime;
        }
        else if (_canAttack && Mathf.Abs(_distanceToPlayer) >= _minRange && Mathf.Abs(_distanceToPlayer) <= _maxRange) 
        {
            _timer = 0;
            _attackScript.Attack();
        }
    }
}