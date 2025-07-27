using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public int speed = 5;
    private Rigidbody2D characterBody;
    private Vector2 velocity;
    private Vector2 inputMovement;


    private Sprite currentSprite;
    private bool canRoll = true;
    private string rollingDirection;
    public bool isRolling = false;
    public float rollingPower;
    public float rollingTime;
    public float rollingCooldown;
    public Sprite SpriteRight1, SpriteRight2, SpriteRight3, SpriteRight4, SpriteLeft1, SpriteLeft2, SpriteLeft3, SpriteLeft4, SpriteUp1, SpriteUp2, SpriteUp3, SpriteUp4, SpriteDown1, SpriteDown2, SpriteDown3, SpriteDown4;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        velocity = new Vector2(speed, speed);
        characterBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canRoll)
        {
            StartCoroutine(Roll());
        }
        inputMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (inputMovement.magnitude > 1)
        {
            inputMovement.Normalize();
        }
    }

    private void FixedUpdate()
    {
        if (isRolling == true) return; // Don't move normally while rolling

        Vector2 delta = inputMovement * velocity * Time.deltaTime;
        Vector2 newPosition = characterBody.position + delta;
        characterBody.MovePosition(newPosition);
    }

    private IEnumerator Roll()
    {
        currentSprite = GetComponent<SpriteRenderer>().sprite;

        canRoll = false;
        isRolling = true;

        Vector2 rollDirection = inputMovement.normalized;
        if (rollDirection == Vector2.zero)
            rollDirection = Vector2.right; // default direction if idle

        gameObject.layer = LayerMask.NameToLayer("RollingPlayer");

        // Determine direction for animation
        if (rollDirection.x < 0)
            rollingDirection = "left";
        else if (rollDirection.x > 0)
            rollingDirection = "right";
        else if (rollDirection.y > 0)
            rollingDirection = "up";
        else if (rollDirection.y < 0)
            rollingDirection = "down";

        float elapsed = 0f;
        float frameDuration = rollingTime / 4;
        int currentFrame = 0;

        while (elapsed < rollingTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rollingTime;

            // Speed curve (sinusoidal)
            float curve = Mathf.Sin(t * Mathf.PI);
            float currentSpeed = Mathf.Lerp(speed, rollingPower, curve);

            characterBody.linearVelocity = rollDirection * currentSpeed;

            // Change sprite every quarter duration
            if (elapsed > frameDuration * currentFrame)
            {
                switch (rollingDirection)
                {
                    case "left":
                        SetSpriteFrameLeft(currentFrame);
                        break;
                    case "right":
                        SetSpriteFrameRight(currentFrame);
                        break;
                    case "up":
                        SetSpriteFrameUp(currentFrame);
                        break;
                    case "down":
                        SetSpriteFrameDown(currentFrame);
                        break;
                }
                currentFrame++;
            }

            yield return null;
        }
        GetComponent<SpriteRenderer>().sprite = currentSprite;

        // Reset after rolling
        characterBody.linearVelocity = Vector2.zero;
        isRolling = false;
        gameObject.layer = LayerMask.NameToLayer("Player");

        yield return new WaitForSeconds(rollingCooldown);
        canRoll = true;
    }

    private void SetSpriteFrameLeft(int frame)
    {
        switch (frame)
        {
            case 0: GetComponent<SpriteRenderer>().sprite = SpriteLeft1; break;
            case 1: GetComponent<SpriteRenderer>().sprite = SpriteLeft2; break;
            case 2: GetComponent<SpriteRenderer>().sprite = SpriteLeft3; break;
            case 3: GetComponent<SpriteRenderer>().sprite = SpriteLeft4; break;
        }
    }

    private void SetSpriteFrameRight(int frame)
    {
        switch (frame)
        {
            case 0: GetComponent<SpriteRenderer>().sprite = SpriteRight1; break;
            case 1: GetComponent<SpriteRenderer>().sprite = SpriteRight2; break;
            case 2: GetComponent<SpriteRenderer>().sprite = SpriteRight3; break;
            case 3: GetComponent<SpriteRenderer>().sprite = SpriteRight4; break;
        }
    }

    private void SetSpriteFrameUp(int frame)
    {
        switch (frame)
        {
            case 0: GetComponent<SpriteRenderer>().sprite = SpriteUp1; break;
            case 1: GetComponent<SpriteRenderer>().sprite = SpriteUp2; break;
            case 2: GetComponent<SpriteRenderer>().sprite = SpriteUp3; break;
            case 3: GetComponent<SpriteRenderer>().sprite = SpriteUp4; break;
        }
    }

    private void SetSpriteFrameDown(int frame)
    {
        switch (frame)
        {
            case 0: GetComponent<SpriteRenderer>().sprite = SpriteDown1; break;
            case 1: GetComponent<SpriteRenderer>().sprite = SpriteDown2; break;
            case 2: GetComponent<SpriteRenderer>().sprite = SpriteDown3; break;
            case 3: GetComponent<SpriteRenderer>().sprite = SpriteDown4; break;
        }
    }
}
