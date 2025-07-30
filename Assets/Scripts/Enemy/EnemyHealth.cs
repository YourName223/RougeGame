using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;

    private int currentHealth;

    private HandleAnimation _animationHandler;

    void Start()
    {
        currentHealth = maxHealth;
        _animationHandler = GetComponent<HandleAnimation>();
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
        _animationHandler.SetState(State.Hurt);
        yield return new WaitForSeconds(0.20f);
    }
    private void Die()
    {
        _animationHandler.SetState(State.Dying);
        Destroy(gameObject, 1.2f);
    }
}