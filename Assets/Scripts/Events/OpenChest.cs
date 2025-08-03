using UnityEngine;
using System.Collections;

public class OpenChest : MonoBehaviour
{
    [SerializeField] private GameObject coin;
    [SerializeField] private Sprite OpenChestSprite;

    private GameObject coinInstance;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GetComponent<SpriteRenderer>().sprite = OpenChestSprite;
            StartCoroutine(GiveLoot());
        }
    }

    private IEnumerator GiveLoot()
    {
        yield return new WaitForSeconds(0.25f);
        coinInstance = Instantiate(coin, transform.position, transform.rotation);
        Destroy(gameObject, 0.25f);
    }
}
