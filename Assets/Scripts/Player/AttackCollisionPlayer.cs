using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackCollisionPlayer : MonoBehaviour, IAttack
{
    private int _damage;
    private int _knockbackPower;
    private float _attackTimer;
    private float _x;
    private float _angle;

    public HashSet<GameObject> _damagedEnemies;//List of enemies hit by this attack

    private Vector3 _position;
    private Collider2D _attackCollider;
    private SpriteRenderer _spriteRenderer;
    private Animator _anim;

    [SerializeField] private Collider2D attackCollider;


    void Start()
    {
        _damagedEnemies = new();
        _anim = GetComponent<Animator>();
        _attackCollider = transform.GetComponent<Collider2D>();
        _attackCollider.enabled = false;
        _spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }

    public void UpdateVariables(float knockbackPower, int dmg, Vector2 direction, float angle, float attackTimer, Vector3 position)
    {
        _damage = dmg;
        _position = position;
        _x = direction.x;
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
        _attackCollider.enabled = true;

        StartCoroutine(Animate());
    }

    //Checks for collsion, if enemy and havent hit it, deal dmg
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.layer == LayerMask.NameToLayer("Enemy") && !_damagedEnemies.Contains(other))
        {
            HealthEnemy enemyHP = collision.gameObject.GetComponent<HealthEnemy>();
            FollowEnemy enemyMov = collision.gameObject.GetComponent<FollowEnemy>();
            Rigidbody2D body = collision.GetComponent<Rigidbody2D>();

            if (enemyHP != null)
            {
                enemyMov.KnockBack((body.position - (Vector2)transform.position).normalized, _knockbackPower);
                enemyHP.TakeDamage(_damage);
                _damagedEnemies.Add(other);
            }
        }
    }

    public IEnumerator Animate()    
    {
        _anim.Play("PlayerMeleeAnimation");

        yield return new WaitForSeconds(_attackTimer);

        _attackCollider.enabled = false;

        _damagedEnemies.Clear();

        _anim.Play("IdleAnimation");
    }
}
