using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackMeleePlayer : MonoBehaviour, IAttack
{
    private bool _attacking;
    private int _damage;
    private float _knockbackPower;
    private float _attackTimer;
    private float _x;
    private float _angle;

    public HashSet<GameObject> _damagedEnemies;//List of enemies hit by this attack

    private Vector3 _position;
    private Collider2D _attackCollider;
    private MovementPlayer _movementPlayer;

    [SerializeField] private Collider2D attackCollider;


    void Start()
    {
        _movementPlayer = GameObject.Find("Player").GetComponent<MovementPlayer>();
        _attacking = false;
        _damagedEnemies = new();
        _attackCollider = transform.GetComponent<Collider2D>();
        _attackCollider.enabled = false;
    }

    public void UpdateVariables(float knockbackPower, int dmg, Vector2 direction, float angle, float attackTimer, Vector3 position)
    {
        _knockbackPower = knockbackPower;
        _damage = dmg;
        _position = position;
        _x = direction.x;
        _angle = angle;
        _attackTimer = attackTimer;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Sign(_x) * Mathf.Abs(scale.x);
        transform.localScale = scale;
        if (_x < 0)
        {
            _angle -= 160;
        }
        else
        {
            _angle -= 20;
        }

        if (!_attacking)
        {
            transform.SetPositionAndRotation(_position, Quaternion.Euler(0f, 0f, _angle));
        }
    }

    public void Attack()
    {
        StartCoroutine(Animate());
    }

    //Checks for collsion, if its an enemy and havent hit it, deal dmg to it
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
        _attacking = true;
        _attackCollider.enabled = true;

        _movementPlayer.attacking = true;
        float duration = 0;
        while (duration < _attackTimer)
        {
            duration += Time.deltaTime;
            if (_x < 0)
            {
                transform.SetPositionAndRotation(_position, Quaternion.Euler(0f, 0f, _angle + (180 * (duration / _attackTimer))));
            }
            else
            {
                transform.SetPositionAndRotation(_position, Quaternion.Euler(0f, 0f, _angle - (180 * (duration / _attackTimer))));
            }

            yield return null;
        }

        //_anim.Play("PlayerMeleeAnimation");

        _attackCollider.enabled = false;

        _damagedEnemies.Clear();

        _attacking = false;
        _movementPlayer.attacking = false;
        //_anim.Play("IdleAnimation");
    }
}
