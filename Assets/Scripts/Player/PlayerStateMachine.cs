using UnityEngine;

public enum PlayerState
{
    Idle,
    Running,
    Jumping,
    Falling,
    Dashing,
    BackDashing,
    Crouching,
    Attacking,
    GroundedComboAttack1,
    GroundedComboAttack2,
    GroundedComboAttack3
}

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState currentState { get; private set; }
    private Animator animator;
    private Rigidbody2D rigid;

    private int comboCounter;

    private void Awake()
    {
        currentState = PlayerState.Idle;
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();

        comboCounter = 0;
    }

    private void Update()
    {
        animator.SetFloat("VerticalSpeed", rigid.linearVelocity.y);
    }

    public void ChangeState(PlayerState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
            UpdateAnimation();
            ApplyStateEffects();
            Debug.Log("Entering State: " + currentState);
        }
    }

    private void UpdateAnimation()
    {
        animator.SetBool("isIdle", currentState == PlayerState.Idle);
        animator.SetBool("isRunning", currentState == PlayerState.Running);
        animator.SetBool("isJumping", currentState == PlayerState.Jumping);
        animator.SetBool("isFalling", currentState == PlayerState.Falling);
        animator.SetBool("isDashing", currentState == PlayerState.Dashing);
        animator.SetBool("isBackDashing", currentState == PlayerState.BackDashing);

        if (currentState == PlayerState.GroundedComboAttack1)
            animator.SetInteger("GroundedComboAttack", 1);
        else if (currentState == PlayerState.GroundedComboAttack2)
            animator.SetInteger("GroundedComboAttack", 2);
        else if (currentState == PlayerState.GroundedComboAttack3)
            animator.SetInteger("GroundedComboAttack", 3);
        else if (currentState != PlayerState.Attacking)
            animator.SetInteger("GroundedComboAttack", 0);
    }

    private void ApplyStateEffects()
    {
        // Adjust gravity depending on state
        AdjustGravity();
    }

    private void AdjustGravity()
    {
        switch (currentState)
        {
            case PlayerState.Falling:
                // Increased gravity for faster descent
                rigid.gravityScale = 5.5f; 
                break;

            default:
                rigid.gravityScale = 2.5f;
                break;
        }
    }

    private void ResetComboCounts()
    {
        comboCounter = 0;
    }
}