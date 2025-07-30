using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttackCollision : MonoBehaviour
{
    public HashSet<GameObject> damagedEnemies;//List of enemies hit by this attack

    [SerializeField] private int damage;
    [SerializeField] private int knockbackPower;

    [SerializeField] private Collider2D attackCollider;

    private Animator _anim;

    void Start()
    {
        damagedEnemies = new();
        _anim = GetComponent<Animator>();
    }

    //Checks for collsion, if enemy and havent hit it, deal dmg
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.layer == LayerMask.NameToLayer("Enemy") && !damagedEnemies.Contains(other))
        {
            EnemyHealth enemyHP = collision.gameObject.GetComponent<EnemyHealth>();
            EnemyFollow enemyMov = collision.gameObject.GetComponent<EnemyFollow>();
            Rigidbody2D body = collision.GetComponent<Rigidbody2D>();

            if (enemyHP != null)
            {
                enemyMov.KnockBack((body.position - (Vector2)transform.position).normalized, knockbackPower);
                enemyHP.TakeDamage(damage);
                damagedEnemies.Add(other);
            }
        }
    }

    public IEnumerator Animate()    
    {
        _anim.Play("PlayerMeleeAnimation");
        yield return new WaitForSeconds(0.6f);
        _anim.Play("IdleAnimation");
    }
}
