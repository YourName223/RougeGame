using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public Transform attackPoint;
    public float attackCooldown;
    public GameObject attack;
    private bool canAttack = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = mouseWorldPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        attackPoint.rotation = Quaternion.Euler(0f, 0f, angle);

        GameObject attackInstance = Instantiate(attack, attackPoint.position, attackPoint.rotation);
        Rigidbody2D rb = attackInstance.GetComponent<Rigidbody2D>();

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
