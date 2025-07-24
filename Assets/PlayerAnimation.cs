using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Sprite SpriteUp, SpriteUp1, SpriteUp2, SpriteUp3, SpriteLeft, SpriteLeft1, SpriteLeft2, SpriteLeft3, SpriteDown, SpriteDown1, SpriteDown2, SpriteDown3, SpriteRight, SpriteRight1, SpriteRight2, SpriteRight3;
    private int number = -1;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            number++;
            if (number == 220)
                number = 0;
            switch (number)
            {
                case 0:
                    GetComponent<SpriteRenderer>().sprite = SpriteLeft;
                    break;
                case 55:
                    GetComponent<SpriteRenderer>().sprite = SpriteLeft1;
                    break;
                case 110:
                    GetComponent<SpriteRenderer>().sprite = SpriteLeft2;
                    break;
                case 165:
                    GetComponent<SpriteRenderer>().sprite = SpriteLeft3;
                    break;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            number++;
            if (number == 220)
                number = 0;
            switch (number)
            {
                case 0:
                    GetComponent<SpriteRenderer>().sprite = SpriteRight;
                    break;
                case 55:
                    GetComponent<SpriteRenderer>().sprite = SpriteRight1;
                    break;
                case 110:
                    GetComponent<SpriteRenderer>().sprite = SpriteRight2;
                    break;
                case 165:
                    GetComponent<SpriteRenderer>().sprite = SpriteRight3;
                    break;
            }
        }
        else if (Input.GetKey(KeyCode.W))
        {
            number++;
            if (number == 220)
                number = 0;
            switch (number)
            {
                case 0:
                    GetComponent<SpriteRenderer>().sprite = SpriteUp;
                    break;
                case 55:
                    GetComponent<SpriteRenderer>().sprite = SpriteUp1;
                    break;
                case 110:
                    GetComponent<SpriteRenderer>().sprite = SpriteUp2;
                    break;
                case 165:
                    GetComponent<SpriteRenderer>().sprite = SpriteUp3;
                    break;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            number++;
            if (number == 220)
                number = 0;
            switch (number)
            {
                case 0:
                    GetComponent<SpriteRenderer>().sprite = SpriteDown;
                    break;
                case 55:
                    GetComponent<SpriteRenderer>().sprite = SpriteDown1;
                    break;
                case 110:
                    GetComponent<SpriteRenderer>().sprite = SpriteDown2;
                    break;
                case 165:
                    GetComponent<SpriteRenderer>().sprite = SpriteDown3;
                    break;
            }
        }
    }
}
