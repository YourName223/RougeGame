using Unity.VisualScripting;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    private Transform target;
    public float speed;
    public float range;
    public float aggroRange;
    private Rigidbody2D rb;
    private HandleAnimation animationHandler;
    private Vector2 _knockBack;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animationHandler = GetComponent<HandleAnimation>();
    }

    void FixedUpdate()
    {
        if (animationHandler.isDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        _knockBack *= 0.89f;
        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

        Vector2 direction = target.position - transform.position;

        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }

        if (distanceToPlayer < range - 0.2f)
        {
            direction = -direction;
            animationHandler.SetState(State.Walking);
        }
        else if (distanceToPlayer > aggroRange + 0.2f)
        {
            direction = Vector2.zero;
            animationHandler.SetState(State.Idle);
        }
        else if (distanceToPlayer > range + 0.2f)
        {
            animationHandler.SetState(State.Walking);
        }
        else
        {
            direction = Vector2.zero;
            animationHandler.SetState(State.Idle);
        }



        animationHandler.x = direction.x;
        rb.linearVelocity = _knockBack + direction * speed;
    }
    public void KnockBack(Vector2 knockbackDirection, float knockbackSpeed)
    {
        _knockBack = knockbackDirection * knockbackSpeed;
    }
}