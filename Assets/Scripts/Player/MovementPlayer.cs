using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using Unity.VisualScripting;

public class MovementPlayer : MonoBehaviour
{
    [SerializeField] private float acceleration;
    [SerializeField] private float speed;
    [SerializeField] private float rollingPower;
    [SerializeField] private float rollingTime;
    [SerializeField] private float rollingCooldown;
    [SerializeField] private PauseScreen PauseScreen;

    [HideInInspector] public Vector2 inputMovement;

    private bool _canRoll;
    private bool isRolling;

    private Rigidbody2D _characterBody;
    private HandleAnimation _animationHandler;
    private Vector2 _lastDirection;
    private Vector2 _knockBack;
    private Vector2 _direction;
    private Vector2 _finalVelocity;
    private Vector3 _mouseWorldPos;

    void Start()
    {
        _finalVelocity = Vector2.zero;
        _canRoll = true;
        isRolling = false;
        _characterBody = GetComponent<Rigidbody2D>();
        _animationHandler = GetComponent<HandleAnimation>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PauseScreen.gameObject.SetActive(true);
            PauseScreen.Set(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseScreen.Set(false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && _canRoll)
        {
            isRolling = true;
            StartCoroutine(Roll());
        }
    }
    private void FixedUpdate()
    {
        _knockBack *= 0.9f;//Reduces _knockback over time

        if (isRolling)
        {
            _animationHandler.SetState(State.Rolling);
            return;
        }

        _mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _direction = _mouseWorldPos - transform.position;

        _animationHandler.x = _direction.x;

        inputMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        inputMovement.Normalize();

        if (inputMovement.magnitude == 0)
        {
            // Decay player's own velocity separately (only player movement velocity)
            Vector2 playerMovementVelocity = _characterBody.linearVelocity - _knockBack;

            if (playerMovementVelocity.magnitude > 0.75f && _characterBody.linearVelocity.normalized == _lastDirection)
            {
                _finalVelocity -= _lastDirection * acceleration;
            }
            else
            {
                _finalVelocity = Vector2.zero;
            }

            // Final velocity = player's slowed velocity + knockback velocity

            _animationHandler.SetState(State.Idle);
        }
        else
        {
            if (_characterBody.linearVelocity.magnitude < (inputMovement * speed).magnitude)
            {
                _finalVelocity = _characterBody.linearVelocity + inputMovement * acceleration;
            }
            else
            {
                _finalVelocity = inputMovement * speed;
            }
            _lastDirection = inputMovement;
            _animationHandler.SetState(State.Walking);
        }

        _characterBody.linearVelocity = _finalVelocity + _knockBack;
    }

    private IEnumerator Roll()
    {
        _canRoll = false;

        _animationHandler.SetState(State.Rolling);

        gameObject.layer = LayerMask.NameToLayer("RollingPlayer");

        float elapsed = 0f;

        //Doing roll, movement speed is a sine curve
        while (elapsed < rollingTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rollingTime;

            float curve = Mathf.Sin(t * Mathf.PI);
            float rollingSpeed = Mathf.Lerp(speed, rollingPower, curve);

            _characterBody.linearVelocity = rollingSpeed * _lastDirection;

            yield return null;
        }


        _finalVelocity = _characterBody.linearVelocity;
        isRolling = false;
        gameObject.layer = LayerMask.NameToLayer("Player");

        yield return new WaitForSeconds(rollingCooldown);
        _canRoll = true;
    }

    public void KnockBack(Vector2 knockbackDirection, float knockbackSpeed)
    {
        _knockBack = knockbackDirection * knockbackSpeed;
    }

    public void MeleeKnockback(Vector2 direction, float Knockback) 
    {
        if (inputMovement == Vector2.zero)
        {
            KnockBack(direction, Knockback);
        }
    }
}
