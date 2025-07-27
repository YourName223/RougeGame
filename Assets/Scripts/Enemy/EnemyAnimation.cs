using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public Sprite SpriteUp, SpriteUp1, SpriteUp2, SpriteUp3, SpriteLeft, SpriteLeft1, SpriteLeft2, SpriteLeft3, SpriteDown, SpriteDown1, SpriteDown2, SpriteDown3, SpriteRight, SpriteRight1, SpriteRight2, SpriteRight3;
    private int number = -1;
    private Transform target;

    public float range = 3f;    // Move away if closer than this
    public float aggroRange = 6f;   // Move closer if farther than this

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, target.position);
        Vector2 direction = target.position - transform.position;

        if (distanceToPlayer < range - 0.1f)
        {
            direction = -direction;
            Move(direction);
        }
        else if (distanceToPlayer > aggroRange + 0.1f)
        {

        }
        else if (distanceToPlayer > range + 0.1f)
        {
            Move(direction);
        }
        else if (distanceToPlayer < aggroRange)
        {
            Look(direction);
        }
    }

    void Look(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Horizontal movement is dominant
            if (direction.x > 0)
            {
                GetComponent<SpriteRenderer>().sprite = SpriteRight;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = SpriteLeft;
            }
        }
        else
        {
            // Vertical movement is dominant
            if (direction.y > 0)
            {
                GetComponent<SpriteRenderer>().sprite = SpriteUp;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = SpriteDown;
            }
        }
    }

    void Move(Vector2 direction) 
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Horizontal movement is dominant
            if (direction.x > 0)
            {
                number++;
                if (number == 32)
                    number = 0;
                switch (number)
                {
                    case 0:
                        GetComponent<SpriteRenderer>().sprite = SpriteRight;
                        break;
                    case 8:
                        GetComponent<SpriteRenderer>().sprite = SpriteRight1;
                        break;
                    case 16:
                        GetComponent<SpriteRenderer>().sprite = SpriteRight2;
                        break;
                    case 24:
                        GetComponent<SpriteRenderer>().sprite = SpriteRight3;
                        break;
                }
            }
            else
            {
                number++;
                if (number == 32)
                    number = 0;
                switch (number)
                {
                    case 0:
                        GetComponent<SpriteRenderer>().sprite = SpriteLeft;
                        break;
                    case 8:
                        GetComponent<SpriteRenderer>().sprite = SpriteLeft1;
                        break;
                    case 16:
                        GetComponent<SpriteRenderer>().sprite = SpriteLeft2;
                        break;
                    case 24:
                        GetComponent<SpriteRenderer>().sprite = SpriteLeft3;
                        break;
                }
            }
        }
        else
        {
            // Vertical movement is dominant
            if (direction.y > 0)
            {
                number++;
                if (number == 32)
                    number = 0;
                switch (number)
                {
                    case 0:
                        GetComponent<SpriteRenderer>().sprite = SpriteUp;
                        break;
                    case 8:
                        GetComponent<SpriteRenderer>().sprite = SpriteUp1;
                        break;
                    case 16:
                        GetComponent<SpriteRenderer>().sprite = SpriteUp2;
                        break;
                    case 24:
                        GetComponent<SpriteRenderer>().sprite = SpriteUp3;
                        break;
                }
            }
            else
            {
                number++;
                if (number == 32)
                    number = 0;
                switch (number)
                {
                    case 0:
                        GetComponent<SpriteRenderer>().sprite = SpriteDown;
                        break;
                    case 8:
                        GetComponent<SpriteRenderer>().sprite = SpriteDown1;
                        break;
                    case 16:
                        GetComponent<SpriteRenderer>().sprite = SpriteDown2;
                        break;
                    case 24:
                        GetComponent<SpriteRenderer>().sprite = SpriteDown3;
                        break;
                }
            }
        }
    }
}
