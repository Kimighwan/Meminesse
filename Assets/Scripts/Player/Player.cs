using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    #region Variables/References

    [SerializeField] float moveSpeed;
    [SerializeField] float maxJumpHoldTime;
    [SerializeField] float baseJumpForce;
    [SerializeField] float holdJumpForce;

    [SerializeField] float dashSpeed;
    [SerializeField] float backDashSpeed;
    [SerializeField] float dashDuration;
    [SerializeField] float dashCooldown;

    [SerializeField] float attackCoolDown;
    [SerializeField] float maxAttackInterval;

    [SerializeField] float[] damage_GroundedComboAttack = new float[3];

    private enum PlayerState
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

    private PlayerState currentState;
    private bool canDash;
    private bool canAttack;
    private bool lockStateChange;

    private bool isDashing;
    private bool isBackDashing;
    private bool isJumping;
    private bool isGrounded;
    private bool isAttacking;

    private int comboCounter;

    private float previousDirection = 1f;
    private float maxGroundedComboAttack = 3;

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    #endregion

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        rigid.freezeRotation = true;
        rigid.interpolation = RigidbodyInterpolation2D.Interpolate;
        rigid.linearDamping = 0;

        isGrounded = true;
        canDash = true;
        canAttack = true;

        lockStateChange = false;
        comboCounter = 0;
        currentState = PlayerState.Idle;
    }

    private void Update()
    {
        HandleState();
        MoveCharacter();
    }

    #region State Management

    private void ChangeState(PlayerState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
            UpdateAnimation();
            ApplyStateEffects();
            Debug.Log("Entering State: " + currentState);
        }
    }

    private void HandleState()
    {
        if (!lockStateChange)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                canAttack = false;
                ChangeState(PlayerState.Dashing);
                StartCoroutine(DashCharacter());
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl) && canDash && isGrounded)
            {
                canAttack = false;
                ChangeState(PlayerState.BackDashing);
                StartCoroutine(BackDashCharacter());
            }
            else if (Input.GetKeyDown(KeyCode.Z) && isGrounded && canAttack)
            {
                lockStateChange = true;
                ChangeState(PlayerState.Attacking);
                StartCoroutine(GroundedComboAttack());
            }
            else if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                ChangeState(PlayerState.Jumping);
                StartCoroutine(JumpCharacter());
            }
            else if (!isGrounded && rigid.linearVelocity.y <= 0)
            {
                ChangeState(PlayerState.Falling);
            }
            else if ((moveInput != 0) && !(isDashing || isBackDashing))
            {
                if (Mathf.Sign(moveInput) != previousDirection)
                {
                    spriteRenderer.flipX = !spriteRenderer.flipX;
                    previousDirection = Mathf.Sign(moveInput);
                }
                else
                {
                    ChangeState(PlayerState.Running);
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow) && isGrounded)
            {
                ChangeState(PlayerState.Crouching);
            }
            else if (rigid.linearVelocity.x == 0 && rigid.linearVelocity.y == 0)
            {
                ChangeState(PlayerState.Idle);
            }

            animator.SetBool("isGrounded", isGrounded);
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
        AdjustGravity();
    }

    private void AdjustGravity()
    {
        if (currentState == PlayerState.Falling)
        {
            rigid.gravityScale = 5.5f;
        }
        else
        {
            rigid.gravityScale = 2.5f;
        }
    }

    #endregion

    #region Basic Character Movements

    private IEnumerator DashCharacter()
    {
        isDashing = true;
        canDash = false;
        float dashTimer = 0f;
        float dashDirection = spriteRenderer.flipX ? -1f : 1f;

        while (dashTimer < dashDuration)
        {
            rigid.linearVelocity = new Vector2(dashDirection * dashSpeed, rigid.linearVelocity.y);
            dashTimer += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
        canAttack = true;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private IEnumerator BackDashCharacter()
    {
        isBackDashing = true;
        canDash = false;
        float dashTimer = 0f;
        float dashDirection = spriteRenderer.flipX ? 1f : -1f;

        while (dashTimer < dashDuration)
        {
            rigid.linearVelocity = new Vector2(dashDirection * backDashSpeed, rigid.linearVelocity.y);
            dashTimer += Time.deltaTime;
            yield return null;
        }

        isBackDashing = false;
        canAttack = true;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void MoveCharacter()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rigid.linearVelocity = new Vector2(moveInput * moveSpeed, rigid.linearVelocity.y);
    }

    private IEnumerator JumpCharacter()
    {
        isGrounded = false;
        isJumping = true;
        float jumpTimer = 0f;
        float currentJumpForce = baseJumpForce;
        float jumpAcceleration = 1.5f;

        rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, currentJumpForce);

        while (Input.GetKey(KeyCode.Space) && jumpTimer < maxJumpHoldTime)
        {
            rigid.linearVelocity += holdJumpForce * Time.deltaTime * Vector2.up;
            jumpTimer += Time.deltaTime;
            currentJumpForce = Mathf.Max(currentJumpForce - (jumpAcceleration * Time.deltaTime), 0);
            yield return null;
        }

        isJumping = false;
    }

    #endregion

    #region Attack

    private IEnumerator GroundedComboAttack()
    {
        canAttack = false;
        comboCounter++;

        float startingTime = Time.time;
        float currentTime = startingTime;

        bool reset = true;

        switch (comboCounter)
        {
            case 1:
                ChangeState(PlayerState.GroundedComboAttack1);
                break;
            case 2:
                ChangeState(PlayerState.GroundedComboAttack2);
                break;
            case 3:
                ChangeState(PlayerState.GroundedComboAttack3);
                break;
        }

        yield return new WaitForSeconds(attackCoolDown);
        while (currentTime < startingTime + maxAttackInterval)
        {
            currentTime = Time.time;
            if (Input.GetKeyDown(KeyCode.Z) && comboCounter < maxGroundedComboAttack)
            {
                StartCoroutine(GroundedComboAttack());
                reset = false;
            }
            yield return null;
        }

        if (reset) { comboCounter = 0; lockStateChange = false; }
        canAttack = true;
    }

    #endregion
}