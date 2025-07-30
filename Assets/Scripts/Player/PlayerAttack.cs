using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackSpeed;

    public GameObject attackPrefab;

    private bool _canAttack;
    private float _originalSpeed;
    private float _angle;

    private Collider2D _attackCollider;
    private GameObject _attackObject;
    private SpriteRenderer _spriteRenderer;
    private PlayerMovement _playerMovement;
    private Vector2 _direction;
    private Vector3 _mouseWorldPos;

    void Start()
    {
        _canAttack = true;
        _attackObject = Instantiate(attackPrefab, transform.position, Quaternion.identity); //Adds _attackObject to the world
        _attackCollider = _attackObject.GetComponent<Collider2D>();
        _attackCollider.enabled = false;
        _playerMovement = GetComponent<PlayerMovement>();
        _originalSpeed = _playerMovement.speed;
        _spriteRenderer = _attackObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        RotateAttackObject();

        if (Input.GetMouseButtonDown(0) && _canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    private void RotateAttackObject() 
    {
        _mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _direction = _mouseWorldPos - transform.position;
        _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

        if (_direction.x < 0)
        {
            _spriteRenderer.flipX = true;
            _angle -= 180;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }

        _attackObject.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0f, 0f, _angle));
    }

    private IEnumerator Attack()
    {
        PlayerAttackCollision PAC = _attackObject.GetComponent<PlayerAttackCollision>();
        PAC.damagedEnemies.Clear();

        PAC.StartCoroutine(PAC.Animate());

        _attackCollider.enabled = true;

        _playerMovement.speed = _originalSpeed / 2f;

        _canAttack = false;
        
        yield return new WaitForSeconds(attackSpeed / 3);

        //Pushes player if standing still and attacking
        _playerMovement.MeleeKnockback(_direction, 1.7f);

        yield return new WaitForSeconds(attackSpeed / 1.5f);
        
        _playerMovement.speed = _originalSpeed;

        yield return new WaitForSeconds(attackCooldown);
        _canAttack = true;
        _attackCollider.enabled = false;
    }
}
