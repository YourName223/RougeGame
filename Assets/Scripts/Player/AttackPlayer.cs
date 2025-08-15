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
    private MovementPlayer _movementPlayer;

    private float _attackCooldownTimer;
    private bool _canAttack;
    private float _attackAngle;


    private IAttack _attackScript;
    private Collider2D _attackCollider;
    private GameObject _attackObject;
    private Vector2 _attackDirection;
    private Vector3 _mouseWorldPos;


    void Start()
    {
        _movementPlayer = GetComponent<MovementPlayer>();
        _canAttack = true;
        _attackObject = Instantiate(_attackPrefab, transform.position, Quaternion.identity); //Adds _attackObject to the world
        _attackCollider = _attackObject.GetComponent<Collider2D>();
        _attackCollider.enabled = false;
        _attackScript = _attackObject.GetComponent<IAttack>();
    }
    void Update()
    {
        _mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _attackDirection = _mouseWorldPos - transform.position;
        _attackAngle = Mathf.Atan2(_attackDirection.y, _attackDirection.x) * Mathf.Rad2Deg;

        _attackScript.UpdateVariables(_knockbackPower, _dmg, _attackDirection, _attackAngle, _attackTimer, transform.position);

        if (_attackCooldownTimer <= _attackCooldown)
        {
            _attackCooldownTimer += Time.deltaTime;
        }
        else if (_canAttack && Input.GetMouseButtonDown(0) && !_movementPlayer.isRolling)
        {
            _attackCooldownTimer = 0;
            _movementPlayer.MeleeKnockback(_attackDirection, 1.7f);
            _attackScript.Attack();
        }
    }
}
