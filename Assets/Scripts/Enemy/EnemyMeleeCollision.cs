using UnityEngine;
using System.Collections;

public class EnemyMeleeCollision : MonoBehaviour
{
    public int damage;
    private Transform player;
    private Transform enemy;
    public int knockbackPower;
    public float attackTimer;
    public Sprite SpriteAttack, SpriteAttack1, SpriteAttack2, SpriteAttack3;
    private SpriteRenderer spriteRenderer;
    private bool hasHit = false;
    private Vector3 offset;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player").transform;  // Find player by tag
        StartCoroutine(Attack());
    }

    void Update()
    {
        transform.position = enemy.position + offset;  // Follow player position with offset
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

    public void Init(Transform enemyTransform)
    {
        enemy = enemyTransform;
        offset = transform.position - enemy.position;
    }
}