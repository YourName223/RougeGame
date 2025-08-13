using Unity.VisualScripting;
using UnityEngine;

public class FollowEnemy : MonoBehaviour
{
    [SerializeField] private float acceleration;
    [SerializeField] private float aggroRange;
    [SerializeField] private float range;
    [SerializeField] private float speed;

    private float _distanceToPlayer;
    private Vector2 _direction;

    private Transform _target;
    private Rigidbody2D _rb;
    private HandleAnimation _animationHandler;
    private Vector2 _lastDirection;
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

        if (_direction == Vector2.zero)
        {
            if (_rb.linearVelocity.magnitude > 0.75f)
            {
                _rb.linearVelocity -= _lastDirection * acceleration;
            }
            else
            {
                _rb.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            if (_rb.linearVelocity.magnitude < (_direction * speed).magnitude)
            {
                _rb.linearVelocity += _direction * acceleration;
            }
            else
            {
                _rb.linearVelocity = _direction * speed + _knockBack;
            }
            _lastDirection = _direction;
        }
    }
    public void KnockBack(Vector2 knockbackDirection, float knockbackSpeed)
    {
        _knockBack = knockbackDirection * knockbackSpeed;
    }
}