using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public int speed = 5;
    private Rigidbody2D characterBody;
    private Vector2 velocity;
    private Vector2 inputputMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        velocity = new Vector2(speed, speed);
        characterBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        inputputMovement = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
            );
        if (inputputMovement.magnitude > 1)
        {
            inputputMovement.Normalize();
        }
    }

    private void FixedUpdate()
    {
        Vector2 delta = inputputMovement * velocity * Time.deltaTime;
        Vector2 newPosition = characterBody.position + delta;
        characterBody.MovePosition(newPosition);
    }
}
