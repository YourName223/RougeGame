using UnityEngine;
using System.Collections;

public class EnemyMeleeCollision : MonoBehaviour
{
    private Animator anim;
    public int damage;
    private Transform player;
    public int knockbackPower;
    public float attackTimer;
    public bool hasHit = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;  // Find player by tag
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit)
            return;

        PlayerHealth playerHP = collision.gameObject.GetComponent<PlayerHealth>();
        PlayerMovement playerMov = collision.gameObject.GetComponent<PlayerMovement>();

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (playerHP != null)
            {
                playerMov.KnockBack((player.position - transform.position).normalized, knockbackPower);

                playerHP.TakeDamage(damage);
                hasHit = true;
            }
        }
    }

    public IEnumerator Animate()
    {
        anim.Play("PlayerMeleeAnimation");

        yield return new WaitForSeconds(attackTimer);

        anim.Play("IdleAnimation");
    }
}