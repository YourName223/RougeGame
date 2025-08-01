using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackMeleeEnemy : MonoBehaviour, IAttack
{
    private bool _hasHit;
    private int _damage;
    private int _knockbackPower;
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

    public void UpdateVariables(int dmg, float x, float angle, float attackTimer, Vector3 position) 
    {
        _damage = dmg;
        _position = position;
        _x = x;
        _angle = angle;
        _attackTimer = attackTimer;

        if (_x < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
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
            PlayerHealth playerHP = collision.gameObject.GetComponent<PlayerHealth>();
            PlayerMovement playerMov = collision.gameObject.GetComponent<PlayerMovement>();

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