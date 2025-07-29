using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public Rigidbody2D characterBody;
    public Vector2 inputMovement;
    private HandleAnimation animationHandler;
    private bool canRoll = true;
    public bool isRolling = false;
    public float rollingPower;
    public float rollingTime;
    public float rollingCooldown;
    private Vector2 _knockBack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterBody = GetComponent<Rigidbody2D>();
        animationHandler = GetComponent<HandleAnimation>();
    }

    private void Update() 
    {
        animationHandler.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - characterBody.position.x;
        if (Input.GetKeyDown(KeyCode.LeftShift) && canRoll)
        {
            StartCoroutine(Roll());
        }
    }

    private void FixedUpdate()
    {
        inputMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _knockBack *= 0.89f;

        if (inputMovement.magnitude == 0)
        {
            animationHandler.SetState(State.Idle);
        }
        else
        {
            animationHandler.SetState(State.Walking);
        }

        if (inputMovement.magnitude > 1)
        {
            inputMovement.Normalize();
        }

        if (isRolling == true) return; // Don't move normally while rolling

        characterBody.linearVelocity = _knockBack + inputMovement * speed;
    }

    private IEnumerator Roll()
    {
        canRoll = false;
        isRolling = true;

        Vector2 rollDirection = inputMovement.normalized;
        if (rollDirection == Vector2.zero)
            rollDirection = Vector2.right; // default direction if idle

        animationHandler.SetState(State.Rolling);

        gameObject.layer = LayerMask.NameToLayer("RollingPlayer");

        float elapsed = 0f;

        while (elapsed < rollingTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rollingTime;

            float curve = Mathf.Sin(t * Mathf.PI);
            float currentSpeed = Mathf.Lerp(speed, rollingPower, curve);

            characterBody.linearVelocity = rollDirection * currentSpeed;

            yield return null;
        }

        characterBody.linearVelocity = Vector2.zero;
        isRolling = false;
        gameObject.layer = LayerMask.NameToLayer("Player");

        yield return new WaitForSeconds(rollingCooldown);
        canRoll = true;
    }

    public void KnockBack(Vector2 knockbackDirection, float knockbackSpeed)
    {
        _knockBack = knockbackDirection* knockbackSpeed;
    }
}
