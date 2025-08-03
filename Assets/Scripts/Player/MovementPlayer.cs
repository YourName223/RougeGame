using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class MovementPlayer : MonoBehaviour
{
    public float speed;

    [SerializeField] private float rollingPower;
    [SerializeField] private float rollingTime;
    [SerializeField] private float rollingCooldown;
    [SerializeField] private PauseScreen PauseScreen;

    private Vector2 inputMovement;

    private bool _canRoll;
    private bool isRolling;

    private Rigidbody2D _characterBody;
    private HandleAnimation _animationHandler;
    private Vector2 _knockBack;
    private Vector2 _direction;
    private Vector3 _mouseWorldPos;

    void Start()
    {
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
            StartCoroutine(Roll());
        }
    }
    private void FixedUpdate()
    {
        inputMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        _mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _direction = _mouseWorldPos - transform.position;

        _animationHandler.x = _direction.x;

        _knockBack *= 0.89f;//Reduces _knockback over time

        if (inputMovement.magnitude == 0)
        {
            _animationHandler.SetState(State.Idle);
        }
        else
        {
            _animationHandler.SetState(State.Walking);
        }

        if (inputMovement.magnitude > 1)
        {
            inputMovement.Normalize();
        }

        //Only move if arent rolling
        if (!isRolling)
        {
            _characterBody.linearVelocity = _knockBack + inputMovement * speed;
        }
    }

    private IEnumerator Roll()
    {
        _canRoll = false;
        isRolling = true;

        if (inputMovement == Vector2.zero)
            inputMovement = Vector2.right;

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

            _characterBody.linearVelocity = rollingSpeed * inputMovement;

            yield return null;
        }

        _characterBody.linearVelocity = Vector2.zero;
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
