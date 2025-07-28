using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public Transform attackPoint;
    public float attackCooldown;
    public float attackSpeed = 0.5f;
    public GameObject attack;
    private PlayerMovement playerMovement;
    private bool canAttack = true;
    private float originalSpeed;


    void Start()
    {
        // Find the PlayerMovement component on the same GameObject
        playerMovement = GetComponent<PlayerMovement>();
        originalSpeed = playerMovement.speed;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        playerMovement.speed = 3 * originalSpeed / 5;

        canAttack = false;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = mouseWorldPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        attackPoint.rotation = Quaternion.Euler(0f, 0f, angle);

        GameObject attackInstance = Instantiate(attack, attackPoint.position, attackPoint.rotation);

        yield return new WaitForSeconds(attackSpeed);

        playerMovement.speed = originalSpeed;

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
