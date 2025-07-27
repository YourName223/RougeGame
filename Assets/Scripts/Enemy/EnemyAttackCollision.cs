using UnityEngine;

public class EnemyAttackCollision : MonoBehaviour
{
    public int damage;
    private bool hasHit = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit)
            return;

        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();


        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (player != null)
            {
                player.TakeDamage(damage);
                hasHit = true;
            }
        }

        Destroy(gameObject); // Destroy bullet on hit
    }
}