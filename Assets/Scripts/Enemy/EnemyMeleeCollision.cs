using UnityEngine;
using System.Collections;

public class EnemyMeleeCollision : MonoBehaviour
{
    [HideInInspector] public bool hasHit;

    [SerializeField] private int damage;
    [SerializeField] private int knockbackPower;
    [SerializeField] private float attackTimer;

    private Animator _anim;
    private Transform _player;

    void Start()
    {
        hasHit = false;
        _anim = GetComponent<Animator>();
        _player = GameObject.FindWithTag("Player").transform;
    }

    //Checks for collision, if player and havent hit, then hit
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !hasHit)
        {
            PlayerHealth playerHP = collision.gameObject.GetComponent<PlayerHealth>();
            PlayerMovement playerMov = collision.gameObject.GetComponent<PlayerMovement>();

            if (playerHP != null)
            {
                playerMov.KnockBack((_player.position - transform.position).normalized, knockbackPower);

                playerHP.TakeDamage(damage);
                hasHit = true;
            }
        }
    }

    public IEnumerator Animate()
    {
        _anim.Play("PlayerMeleeAnimation");

        yield return new WaitForSeconds(attackTimer);

        _anim.Play("IdleAnimation");
    }
}