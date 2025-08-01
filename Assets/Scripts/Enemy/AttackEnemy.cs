using UnityEngine;
using System.Collections;

public class AttackEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _attackPrefab;
    [SerializeField] private int _dmg;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _attackTimer;
    [SerializeField] private float _range;


    private bool _canAttack = true;
    private float _angle;
    private float _distanceToPlayer;

    private IAttack _attackScript;
    private Transform _player;
    private Vector2 _direction;

    void Start()
    {
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

        _attackScript.UpdateVariables(_dmg, _direction.x, _angle, _attackTimer, transform.position);

        if (_canAttack && Mathf.Abs(_distanceToPlayer) <= _range)
        {
            StartCoroutine(WaitForAttack());
        }
    }

    private IEnumerator WaitForAttack() 
    {
        _attackScript.Attack();
        yield return new WaitForSeconds(_attackCooldown);
    }
}