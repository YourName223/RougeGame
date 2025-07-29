using UnityEngine;

public enum State
{
    Idle,
    Walking,
    Rolling,
    Hurt,
    Attacking,
    Dying
}

public class HandleAnimation : MonoBehaviour
{
    private Animator anim;
    public float x;
    public bool isDead = false;
    private SpriteRenderer spriteRenderer;
    public State currentState = State.Idle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (x < 0)
            spriteRenderer.flipX = true;
        else if (x > 0)
            spriteRenderer.flipX = false;

        SetAnimation();
    }

    private void SetAnimation()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (isDead)
        {
            return;
        }
        
        if ((stateInfo.IsName("HurtAnimation") || stateInfo.IsName("RollingAnimation")) && stateInfo.normalizedTime < 1f)
        {
            return;
        }

        switch (currentState)
        {
            case State.Idle:
                anim.Play("IdleAnimation");
                break;
            case State.Walking:
                anim.Play("WalkingAnimation");
                break;
            case State.Rolling:
                anim.Play("RollingAnimation");
                break;
            case State.Hurt:
                anim.Play("HurtAnimation");
                break;
            case State.Attacking:
                anim.Play("AttackAnimation");
                break;
            case State.Dying:
                isDead = true;
                anim.Play("DeathAnimation");
                break;
        }
    }

    public void SetState(State newState)
    {
        currentState = newState;
    }
}
