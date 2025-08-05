using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackRangeEnemy : MonoBehaviour, IAttack
{
    private bool _hasHit;
    private int _damage;
    private float _knockbackPower;
    private float _angle;

    private Vector2 _direction;
    private Vector3 _position;
    private Rigidbody2D _rigidbody;
    private Collider2D _attackCollider;
    private SpriteRenderer _spriteRenderer;
    private TrailRenderer _trail;

    void Start()
    {
        _trail = GetComponent<TrailRenderer>();
        _hasHit = false;
        _attackCollider = transform.GetComponent<Collider2D>();
        _attackCollider.enabled = false;
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }

    public void UpdateVariables(float knockbackPower, int dmg, Vector2 direction, float angle, float attackTimer, Vector3 position)
    {
        _knockbackPower = knockbackPower;
        _damage = dmg;
        _position = position;
        _direction = direction;
        _angle = angle;
    }

    public void Attack()
    {
        transform.SetPositionAndRotation(_position, Quaternion.Euler(0f, 0f, _angle));
        _rigidbody.linearVelocity = _direction.normalized * 3;
        _spriteRenderer.sortingOrder = 2;
        _trail.sortingOrder = 2;
        _hasHit = false;
        _attackCollider.enabled = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !_hasHit)
        {
            HealthPlayer playerHP = collision.gameObject.GetComponent<HealthPlayer>();
            MovementPlayer playerMov = collision.gameObject.GetComponent<MovementPlayer>();

            if (playerHP != null)
            {
                playerMov.KnockBack(_direction.normalized, _knockbackPower);

                playerHP.TakeDamage(_damage);
                _hasHit = true;
            }
        }

        _spriteRenderer.sortingOrder = 0;
        _trail.sortingOrder = 0;
        _attackCollider.enabled = false;

        _rigidbody.linearVelocity = Vector2.zero;
        transform.SetPositionAndRotation(_position, Quaternion.Euler(0f, 0f, _angle));
    }
}
