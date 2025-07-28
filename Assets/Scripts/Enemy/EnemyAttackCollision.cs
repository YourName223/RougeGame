using UnityEngine;

public class EnemyAttackCollision : MonoBehaviour
{
    public int damage;
    public int knockbackPower;
    private Rigidbody2D body;
    private bool hasHit = false;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
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
                playerMov.KnockBack(body.linearVelocity.normalized, knockbackPower);

                playerHP.TakeDamage(damage);
                hasHit = true;
            }
        }

        Destroy(gameObject); // Destroy bullet on hit
    }
}