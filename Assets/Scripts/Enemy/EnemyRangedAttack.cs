using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    private float timer;
    public Transform attackPoint;
    public float bulletForce;
    private Transform target;
    private float shootTimer = 0f;
    public float attackCooldown; // time between shots, in seconds
    public GameObject attack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer > attackCooldown)
        {
            shootTimer = 0f;
            RangedAttack();
        }
    }

    public void RangedAttack()
    {
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        attackPoint.rotation = Quaternion.Euler(0f, 0f, angle - 90);

        GameObject bulletInstance = Instantiate(attack, attackPoint.position, attackPoint.rotation);
        Rigidbody2D rb = bulletInstance.GetComponent<Rigidbody2D>();
        rb.AddForce(attackPoint.up * bulletForce, ForceMode2D.Impulse);
    }
}
