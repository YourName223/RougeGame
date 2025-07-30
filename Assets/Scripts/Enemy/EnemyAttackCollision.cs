using UnityEngine;

public class EnemyAttackCollision : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int knockbackPower;

    private bool _hasHit;

    private Rigidbody2D _body;

    void Start()
    {
        _hasHit = false;
        _body = GetComponent<Rigidbody2D>();
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
                playerMov.KnockBack(_body.linearVelocity.normalized, knockbackPower);

                playerHP.TakeDamage(damage);
                _hasHit = true;
            }
        }

        Destroy(gameObject); //Destroy bullet on collision
    }
}