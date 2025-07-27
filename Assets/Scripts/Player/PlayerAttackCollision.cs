using UnityEngine;
using System.Collections;

public class PlayerAttackCollision : MonoBehaviour
{
    public int damage;
    private Transform player;
    public float attackTimer;
    public Sprite SpriteAttack, SpriteAttack1, SpriteAttack2, SpriteAttack3;
    private SpriteRenderer spriteRenderer;
    private Vector3 offset;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player").transform;  // Find player by tag
        offset = transform.position - player.position;  // Store relative offset
        StartCoroutine(Attack());
    }


    void Update()
    {
        transform.position = player.position + offset;  // Follow player position with offset
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
