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

    [HideInInspector] public bool isDead = false;
    [HideInInspector] public float x;

    private State _currentState;
    private Animator _anim;
    private SpriteRenderer _spriteRenderer;
    private AnimatorStateInfo _stateInfo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentState = State.Idle;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Makes the entity look in the direction they are walking
        if (x < 0)
            _spriteRenderer.flipX = true;
        else if (x > 0)
            _spriteRenderer.flipX = false;
    }

    private void SetAnimation()
    {
        if (isDead)
        {
            return;
        }

        _stateInfo = _anim.GetCurrentAnimatorStateInfo(0);

        //Checks if entity is hurting or rolling
        if ((_stateInfo.IsName("HurtAnimation") || _stateInfo.IsName("RollingAnimation")) && _stateInfo.normalizedTime < 1)
        {
            return;
        }

        //Plays animation compared to state
        switch (_currentState)
        {
            case State.Idle:
                _anim.Play("IdleAnimation");
                break;
            case State.Walking:
                _anim.Play("WalkingAnimation");
                break;
            case State.Rolling:
                _anim.Play("RollingAnimation");
                break;
            case State.Hurt:
                _anim.Play("HurtAnimation");
                break;
            case State.Attacking:
                _anim.Play("AttackAnimation");
                break;
            case State.Dying:
                isDead = true;
                _anim.Play("DeathAnimation");
                break;
        }
    }

    public void SetState(State newState)
    {
        _currentState = newState;

        SetAnimation();
    }
}
