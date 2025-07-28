using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    private HandleAnimation animationHandler;

    public int maxHealth;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        animationHandler = GetComponent<HandleAnimation>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        StartCoroutine(FlashDamageSprite());

        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator FlashDamageSprite()
    {
        animationHandler.SetState(State.Hurt);
        yield return new WaitForSeconds(0.20f);
    }


    private IEnumerator Die()
    {
        animationHandler.SetState(State.Dying);
        yield return new WaitForSeconds(1.20f);
        Destroy(gameObject);
    }
}