using Unity.VisualScripting;
using UnityEngine;

public class FollowEnemy : MonoBehaviour
{
    [SerializeField] private float aggroRange;
    [SerializeField] private float range;
    [SerializeField] private float speed;

    private float _distanceToPlayer;
    private Vector2 _direction;

    private Transform _target;
    private Rigidbody2D _rb;
    private HandleAnimation _animationHandler;
    private Vector2 _knockBack;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _animationHandler = GetComponent<HandleAnimation>();
    }

    void FixedUpdate()
    {
        if (_animationHandler.isDead)
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }

        _knockBack *= 0.89f;//Reduces _knockBack over time
        _distanceToPlayer = Vector2.Distance(transform.position, _target.position);

        _direction = _target.position - transform.position;

        _direction.Normalize();

        //Checks if enemy should move closer, futher away, or stand still
        if (_distanceToPlayer < range - 0.2f)
        {
            _direction = -_direction;
            _animationHandler.SetState(State.Walking);
        }
        else if (_distanceToPlayer > aggroRange + 0.2f)
        {
            _direction = Vector2.zero;
            _animationHandler.SetState(State.Idle);
        }
        else if (_distanceToPlayer > range + 0.2f)
        {
            _animationHandler.SetState(State.Walking);
        }
        else
        {
            _direction = Vector2.zero;
            _animationHandler.SetState(State.Idle);
        }

        _animationHandler.x = _direction.x;
        _rb.linearVelocity = _knockBack + _direction * speed;
    }
    public void KnockBack(Vector2 knockbackDirection, float knockbackSpeed)
    {
        _knockBack = knockbackDirection * knockbackSpeed;
    }
}