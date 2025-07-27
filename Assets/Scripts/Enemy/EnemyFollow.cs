using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    private Animator anim;

    private Transform target;
    public float speed;
    public float range;
    public float aggroRange;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

        Vector2 direction = (target.position - transform.position);

        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }

        if (distanceToPlayer < range - 0.2f)
        {
            anim.SetFloat("x", -direction.x);
            anim.SetFloat("y", -direction.y);
            rb.MovePosition(rb.position + -direction * speed * Time.deltaTime);
        }
        else if (distanceToPlayer > aggroRange + 0.2f)
        {
            //set anim.SetFloat("x" and "y") to 0, and add idle animation for 0,0)
        }
        else if (distanceToPlayer > range + 0.2f)
        {
            anim.SetFloat("x", direction.x);
            anim.SetFloat("y", direction.y);
            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
        }
    }
}