using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttackCollision : MonoBehaviour
{
    private Animator anim;
    public Collider2D attackCollider;
    public int damage;
    public int knockbackPower;
    public HashSet<GameObject> damagedEnemies = new();

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D body = collision.GetComponent<Rigidbody2D>();
        GameObject other = collision.gameObject;

        EnemyHealth enemyHP = collision.gameObject.GetComponent<EnemyHealth>();
        EnemyFollow enemyMov = collision.gameObject.GetComponent<EnemyFollow>();

        if (other.layer == LayerMask.NameToLayer("Enemy") && !damagedEnemies.Contains(other))
        {
            if (enemyHP != null)
            {
                enemyMov.KnockBack(((Vector3)body.position - transform.position).normalized, knockbackPower);

                enemyHP.TakeDamage(damage);
                damagedEnemies.Add(other);
            }
        }
    }

    public IEnumerator Animate()    
    {
        anim.Play("PlayerMeleeAnimation");

        yield return new WaitForSeconds(0.6f);

        anim.Play("IdleAnimation");
    }
}
