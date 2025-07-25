using UnityEngine;

public class OpenChest : MonoBehaviour
{

    public Sprite spriteChest;

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GetComponent<SpriteRenderer>().sprite = spriteChest;
        }
    }

}
