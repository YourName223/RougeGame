using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameOverScreen GameOverScreen;

    private int _currentHealth;

    private HandleAnimation _animationHandler;

    void Start()
    {
        _currentHealth = maxHealth;
        _animationHandler = GetComponent<HandleAnimation>();
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        healthBar.SetHealth(_currentHealth);

        if (_currentHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(FlashDamageSprite());
    }

    private IEnumerator FlashDamageSprite()
    {
        _animationHandler.SetState(State.Hurt);
        yield return new WaitForSeconds(0.20f);
    }

    void Die()
    {
        _animationHandler.SetState(State.Dying);

        GameOverScreen.gameObject.SetActive(true);
        GameOverScreen.Set();
    }
}