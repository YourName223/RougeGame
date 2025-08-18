using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackMeleeEnemy : MonoBehaviour, IAttack
{
    private bool _hasHit;
    private int _damage;
    private float _knockbackPower;
    private float _attackTimer;
    private float _x;
    private float _angle;

    private Vector3 _position;
    private Collider2D _attackCollider;
    private SpriteRenderer _spriteRenderer;
    private Animator _anim;
    private Transform _player;

    void Start()
    {
        _hasHit = false;
        _anim = GetComponent<Animator>();
        _player = GameObject.FindWithTag("Player").transform;
        _attackCollider = transform.GetComponent<Collider2D>();
        _attackCollider.enabled = false;
        _spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }

    public void UpdateVariables(float knockbackPower, int dmg, Vector2 direction, float angle, float attackTimer, Vector3 position) 
    {
        _knockbackPower = knockbackPower;
        _damage = dmg;
        _position = position;
        _x = direction.x;
        _angle = angle;
        _attackTimer = attackTimer;

        if (_x < 0)
        {
            _angle -= 160;
            _spriteRenderer.flipX = true;
        }
        else
        {
            _angle -= 20;
            _spriteRenderer.flipX = false;
        }

        transform.SetPositionAndRotation(_position, Quaternion.Euler(0f, 0f, _angle));
    }

    public void Attack()
    {
        _hasHit = false;
        _attackCollider.enabled = true;

        StartCoroutine(Animate());
    }

    //Checks for collision, if player and havent hit, then hit
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !_hasHit)
        {
            HealthPlayer playerHP = collision.gameObject.GetComponent<HealthPlayer>();
            MovementPlayer playerMov = collision.gameObject.GetComponent<MovementPlayer>();

            if (playerHP != null)
            {
                playerMov.KnockBack((_player.position - transform.position).normalized, _knockbackPower);

                playerHP.TakeDamage(_damage);
                _hasHit = true;
            }
        }
    }

    public IEnumerator Animate()
    {
        _anim.Play("PlayerMeleeAnimation");

        yield return new WaitForSeconds(_attackTimer);

        _attackCollider.enabled = false;

        _anim.Play("IdleAnimation");
    }
}