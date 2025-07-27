using UnityEngine;
using System.Collections;

public class OpenChest : MonoBehaviour
{
    public GameObject coin;
    public Sprite spriteChest;

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GetComponent<SpriteRenderer>().sprite = spriteChest;
            StartCoroutine(GiveLoot());
        }
    }

    private IEnumerator GiveLoot()
    {
        yield return new WaitForSeconds(0.25f);
        GameObject coinInstance = Instantiate(coin, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }
}
