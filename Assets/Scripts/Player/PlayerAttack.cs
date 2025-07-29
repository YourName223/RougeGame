using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public float attackCooldown;
    public float attackSpeed;
    public GameObject attackPrefab; // Prefab reference
    private GameObject attackObject;
    private PlayerMovement playerMovement;
    private bool canAttack = true;
    private float originalSpeed;
    private Collider2D attackCollider;
    private Vector2 direction;
    SpriteRenderer spriteRenderer;



    void Start()
    {
        attackObject = Instantiate(attackPrefab, transform.position, Quaternion.identity);
        attackCollider = attackObject.GetComponent<Collider2D>();
        attackCollider.enabled = false; // Ensure off at start
        attackObject.SetActive(true); //Check if needed
        playerMovement = GetComponent<PlayerMovement>();
        originalSpeed = playerMovement.speed;
        spriteRenderer = attackObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Find the PlayerMovement component on the same GameObject
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        direction = mouseWorldPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

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

        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        PlayerAttackCollision pac = attackObject.GetComponent<PlayerAttackCollision>();
        pac.damagedEnemies.Clear();

        pac.StartCoroutine(pac.Animate());

        attackCollider.enabled = true;


        playerMovement.speed = originalSpeed / 2f;

        canAttack = false;

        
        yield return new WaitForSeconds(attackSpeed / 1.5f);

        if (playerMovement.inputMovement == Vector2.zero)
        {
            playerMovement.KnockBack(direction, 1.7f);
        }

        yield return new WaitForSeconds(attackSpeed / 3);
        
        playerMovement.speed = originalSpeed;

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        attackCollider.enabled = false;
    }
}
