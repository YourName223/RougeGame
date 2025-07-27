using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    public Sprite SpriteDmg;
    private Sprite currentSprite;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        StartCoroutine(FlashDamageSprite());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashDamageSprite()
    {
        currentSprite = GetComponent<SpriteRenderer>().sprite;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = SpriteDmg;
        yield return new WaitForSeconds(0.25f);
        sr.sprite = currentSprite;
    }


    void Die()
    {
        Destroy(gameObject);
    }
}