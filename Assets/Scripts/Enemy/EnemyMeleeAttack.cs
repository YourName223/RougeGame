using UnityEngine;
using System.Collections;

public class EnemyMeleeAttack : MonoBehaviour
{
    public Transform attackPoint;
    public float attackCooldown;
    public float meleeRange;
    public GameObject attack;
    private Transform target;
    private bool canAttack = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = target.position - transform.position;
        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

        if (canAttack && Mathf.Abs(distanceToPlayer) <= meleeRange) 
        {
            StartCoroutine(MeleeAttack());
        }
    }

    private IEnumerator MeleeAttack()
    {
        canAttack = false;

        yield return new WaitForSeconds(attackCooldown / 10);
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        attackPoint.rotation = Quaternion.Euler(0f, 0f, angle);

        GameObject attackInstance = Instantiate(attack, attackPoint.position, attackPoint.rotation);
        Rigidbody2D rb = attackInstance.GetComponent<Rigidbody2D>();
        attackInstance.transform.parent = transform;

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}