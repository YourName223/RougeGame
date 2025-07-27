using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttackCollision : MonoBehaviour
{
    public int damage;
    private Transform player;
    public float attackTimer;
    public Sprite SpriteAttack, SpriteAttack1, SpriteAttack2, SpriteAttack3;
    private SpriteRenderer spriteRenderer;
    private Vector3 offset;
    private HashSet<GameObject> damagedEnemies = new HashSet<GameObject>();

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();

        if (other.layer == LayerMask.NameToLayer("Enemy") && !damagedEnemies.Contains(other))
        {
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                damagedEnemies.Add(other);
            }
        }
    }
}
