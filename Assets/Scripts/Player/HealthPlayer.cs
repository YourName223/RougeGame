using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class HealthPlayer : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBar; 
    [SerializeField] private StatsPlayer _playerStats;
    [SerializeField] private GameOverScreen _gameOverScreen;

    private int maxHealth;
    private int _currentHealth;

    private HandleAnimation _animationHandler;

    void Start()
    {
        maxHealth = _playerStats.vitality;
        _currentHealth = maxHealth;
        _animationHandler = GetComponent<HandleAnimation>();
        _healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (Random.Range(0,1) > Mathf.Pow(0.99f,_playerStats.evasion))
        {
            return;
        }
        _currentHealth -= Mathf.RoundToInt(amount * Mathf.Pow(0.99f, _playerStats.defence));
        _healthBar.SetHealth(_currentHealth);

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

        _gameOverScreen.gameObject.SetActive(true);
        _gameOverScreen.Set();
    }
}