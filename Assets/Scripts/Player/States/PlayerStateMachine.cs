using UnityEngine;

public enum PlayerState
{
    Idle,
    Running,
    Jumping,
    Falling,
    Turning
}

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState currentState { get; private set; }
    private Animator animator;
    private Rigidbody2D rigid;

    private void Awake()
    {
        currentState = PlayerState.Idle;
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
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
        animator.SetBool("isTurning", currentState == PlayerState.Turning);
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
                rigid.gravityScale = 4.5f; 
                break;
            
            default:
                rigid.gravityScale = 2.5f;
                break;
        }
    }
}