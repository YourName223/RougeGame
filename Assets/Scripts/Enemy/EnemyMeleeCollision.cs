using UnityEngine;
using System.Collections;

public class EnemyMeleeCollision : MonoBehaviour
{
    public int damage;
    private Transform player;
    public float attackTimer;
    public Sprite SpriteAttack, SpriteAttack1, SpriteAttack2, SpriteAttack3;
    private SpriteRenderer spriteRenderer;
    private bool hasHit = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player").transform;  // Find player by tag
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackTimer / 4);
        spriteRenderer.sprite = SpriteAttack1;

        yield return new WaitForSeconds(attackTimer / 4);
        spriteRenderer.sprite = SpriteAttack2;

        yield return new WaitForSeconds(attackTimer / 4);
        spriteRenderer.sprite = SpriteAttack3;

        yield return new WaitForSeconds(attackTimer / 4);
        spriteRenderer.sprite = SpriteAttack;
        Destroy(gameObject);
    }

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
    }
}