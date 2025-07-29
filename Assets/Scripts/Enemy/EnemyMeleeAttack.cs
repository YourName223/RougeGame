using UnityEngine;
using System.Collections;

public class EnemyMeleeAttack : MonoBehaviour
{
    public float attackCooldown;
    public float meleeRange;
    private float distanceToPlayer;
    public GameObject attackPrefab; // Prefab reference
    private GameObject attackObject;
    private Transform target;
    private Vector2 direction;
    private Collider2D attackCollider;
    private bool canAttack = true;
    private float angle;
    SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackObject = Instantiate(attackPrefab, transform.position, Quaternion.identity);
        attackCollider = attackObject.GetComponent<Collider2D>();
        attackCollider.enabled = false; // Ensure off at start
        attackObject.SetActive(true); //Check if needed
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        spriteRenderer = attackObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, target.position);

        direction = target.position - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
            angle -= 180;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        attackObject.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0f, 0f, angle));

        if (canAttack && Mathf.Abs(distanceToPlayer) <= meleeRange) 
        {
            StartCoroutine(MeleeAttack());
        }
    }

    private IEnumerator MeleeAttack()
    {
        EnemyMeleeCollision Emc = attackObject.GetComponent<EnemyMeleeCollision>();
        Emc.hasHit = false;
        attackCollider.enabled = true;

        Emc.StartCoroutine(Emc.Animate());

        canAttack = false;

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;

        attackCollider.enabled = false;
    }
}