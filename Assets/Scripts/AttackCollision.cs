using UnityEngine;

public class attackCollision : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();


        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }

        Destroy(gameObject); // Destroy bullet on hit
    }
}