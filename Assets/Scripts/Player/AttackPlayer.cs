using UnityEngine;
using System.Collections;

public class AttackPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _attackPrefab;
    [SerializeField] private int _dmg;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _attackTimer;
    [SerializeField] private float _range;
    [SerializeField] private float _knockbackPower;

    private float _timer;
    private bool _canAttack;
    private float _angle;

    private IAttack _attackScript;
    private Collider2D _attackCollider;
    private GameObject _attackObject;
    private Vector2 _direction;
    private Vector3 _mouseWorldPos;

    void Start()
    {
        _canAttack = true;
        _attackObject = Instantiate(_attackPrefab, transform.position, Quaternion.identity); //Adds _attackObject to the world
        _attackCollider = _attackObject.GetComponent<Collider2D>();
        _attackCollider.enabled = false;
        _attackScript = _attackObject.GetComponent<IAttack>();
    }

    void Update()
    {
        _mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _direction = _mouseWorldPos - transform.position;
        _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

        _attackScript.UpdateVariables(_knockbackPower, _dmg, _direction, _angle, _attackTimer, transform.position);

        if (_timer <= _attackCooldown)
        {
            _timer += Time.deltaTime;
        }
        else if (_canAttack && Input.GetMouseButtonDown(0))
        {
            _timer = 0;
            _attackScript.Attack();
        }
    }
}
