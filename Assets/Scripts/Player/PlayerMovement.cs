using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public int speed = 5;
    private Rigidbody2D characterBody;
    private Vector2 velocity;
    private Vector2 inputMovement;

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
        canRoll = false;
        isRolling = true;

        Vector2 rollDirection = inputMovement.normalized;
        if (rollDirection == Vector2.zero)
            rollDirection = Vector2.right; // default roll direction if idle

        characterBody.linearVelocity = rollDirection * rollingPower;

        gameObject.layer = LayerMask.NameToLayer("RollingPlayer");


        if (rollDirection.x < 0)
        {
            rollingDirection = "left";
        }
        else if (rollDirection.x > 0)
        {
            rollingDirection = "right";
        }
        else if (rollDirection.y > 0)
        {
            rollingDirection = "up";
        }
        else if (rollDirection.y < 0)
        {
            rollingDirection = "down";
        }

        yield return new WaitForSeconds(rollingTime / 4);
        switch (rollingDirection)
        {
            case "left":
                GetComponent<SpriteRenderer>().sprite = SpriteLeft1; 
                break;
            case "right":
                GetComponent<SpriteRenderer>().sprite = SpriteRight1;
                break;
            case "down":
                GetComponent<SpriteRenderer>().sprite = SpriteDown1;
                break;
            case "up":
                GetComponent<SpriteRenderer>().sprite = SpriteUp1;
                break;
        }
        yield return new WaitForSeconds(rollingTime / 4);
        switch (rollingDirection)
        {
            case "left":
                GetComponent<SpriteRenderer>().sprite = SpriteLeft2;
                break;
            case "right":
                GetComponent<SpriteRenderer>().sprite = SpriteRight2;
                break;
            case "down":
                GetComponent<SpriteRenderer>().sprite = SpriteDown2;
                break;
            case "up":
                GetComponent<SpriteRenderer>().sprite = SpriteUp2;
                break;
        }
        yield return new WaitForSeconds(rollingTime / 4);
        switch (rollingDirection)
        {
            case "left":
                GetComponent<SpriteRenderer>().sprite = SpriteLeft3;
                break;
            case "right":
                GetComponent<SpriteRenderer>().sprite = SpriteRight3;
                break;
            case "down":
                GetComponent<SpriteRenderer>().sprite = SpriteDown3;
                break;
            case "up":
                GetComponent<SpriteRenderer>().sprite = SpriteUp3;
                break;
        }
        yield return new WaitForSeconds(rollingTime / 4);
        switch (rollingDirection)
        {
            case "left":
                GetComponent<SpriteRenderer>().sprite = SpriteLeft4;
                break;
            case "right":
                GetComponent<SpriteRenderer>().sprite = SpriteRight4;
                break;
            case "down":
                GetComponent<SpriteRenderer>().sprite = SpriteDown4;
                break;
            case "up":
                GetComponent<SpriteRenderer>().sprite = SpriteUp4;
                break;
        }

        isRolling = false;

        characterBody.linearVelocity = Vector2.zero;

        gameObject.layer = LayerMask.NameToLayer("Player");

        yield return new WaitForSeconds(rollingCooldown);
        canRoll = true;
    }
}
