using UnityEngine;
using System.Collections;

public class EnemyMeleeAttack : MonoBehaviour
{
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _range;

    [SerializeField] private GameObject _attackPrefab;

    private bool _canAttack = true;
    private float _angle;
    private float _distanceToPlayer;

    private Collider2D _attackCollider;
    private GameObject _attackObject;
    private SpriteRenderer _spriteRenderer;
    private Transform _target;
    private Vector2 _direction;

    void Start()
    {
        _attackObject = Instantiate(_attackPrefab, transform.position, Quaternion.identity);
        _attackCollider = _attackObject.GetComponent<Collider2D>();
        _attackCollider.enabled = false;
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _spriteRenderer = _attackObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        _distanceToPlayer = Vector2.Distance(transform.position, _target.position);

        _direction = _target.position - transform.position;
        _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

        if (_direction.x < 0)
        {
            _spriteRenderer.flipX = true;
            _angle -= 180;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }

        _attackObject.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0f, 0f, _angle));

        if (_canAttack && Mathf.Abs(_distanceToPlayer) <= _range)
        {
            StartCoroutine(MeleeAttack());
        }
    }

    private void OnDestroy()
    {
        Destroy(_attackObject);
    }

    private IEnumerator MeleeAttack()
    {
        EnemyMeleeCollision EMC = _attackObject.GetComponent<EnemyMeleeCollision>();
        EMC.hasHit = false;
        _attackCollider.enabled = true;

        EMC.StartCoroutine(EMC.Animate());

        _canAttack = false;

        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;

        _attackCollider.enabled = false;
    }
}