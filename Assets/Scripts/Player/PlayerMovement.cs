using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private Animator anim;

    public int speed = 5;
    private Rigidbody2D characterBody;
    private Vector2 velocity;
    private Vector2 inputMovement;

    private Sprite currentSprite;
    private float x;
    private float y;
    private bool canRoll = true;
    private string rollingDirection;
    public bool isRolling = false;
    public float rollingPower;
    public float rollingTime;
    public float rollingCooldown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        velocity = new Vector2(speed, speed);
        characterBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isRolling", isRolling);

        if (Input.GetKeyDown(KeyCode.LeftShift) && canRoll)
        {
            StartCoroutine(Roll());
        }

        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        anim.SetFloat("x", x);
        anim.SetFloat("y", y);

        inputMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (inputMovement.magnitude > 1)
        {
            inputMovement.Normalize();
        }
    }

    private void FixedUpdate()
    {
        anim.SetBool("isRolling", isRolling);

        if (Input.GetKeyDown(KeyCode.LeftShift) && canRoll)
        {
            StartCoroutine(Roll());
        }

        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        anim.SetFloat("x", x);
        anim.SetFloat("y", y);

        if (isRolling == true) return; // Don't move normally while rolling

        Vector2 delta = inputMovement * velocity * Time.deltaTime;
        Vector2 newPosition = characterBody.position + delta;
        characterBody.MovePosition(newPosition);
    }

    private IEnumerator Roll()
    {
        canRoll = false;
        isRolling = true;

        Vector2 rollDirection = inputMovement.normalized;
        if (rollDirection == Vector2.zero)
            rollDirection = Vector2.right; // default direction if idle

        gameObject.layer = LayerMask.NameToLayer("RollingPlayer");

        float elapsed = 0f;
        float frameDuration = rollingTime / 4;

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
}
