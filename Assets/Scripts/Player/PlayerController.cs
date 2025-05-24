using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class PlayerController : MonoBehaviour
{
    #region Variables/References

    [SerializeField] float moveSpeed;


    // To implement variable jump height
    [SerializeField] float maxJumpHoldTime;
    [SerializeField] float baseJumpForce;
    [SerializeField] float holdJumpForce;


    [SerializeField] float dashSpeed;
    [SerializeField] float backDashSpeed;
    [SerializeField] float dashDuration;
    [SerializeField] float dashCooldown;


    // Attack speed
    [SerializeField] float attackCoolDown;
    [SerializeField] float maxAttackInterval;

    // Attack damage
    [SerializeField] float[] damage_GroundedComboAttack = new float[3];
    [SerializeField] float damage_CrouchAttack;

    private enum PlayerState
    {
        Idle,
        Running,
        Jumping,
        Falling,
        Dashing,
        BackDashing,
        enterCrouching, Crouching, exitCrouching,
        Attacking,
        GroundedComboAttack1, GroundedComboAttack2, GroundedComboAttack3,
        CrouchAttack
    }

    private PlayerState currentState;


    private bool canDash;
    private bool canAttack;
    private bool canMove;
    private bool lockStateChange;


    private bool isDashing;
    private bool isBackDashing;
    private bool isJumping;
    private bool isGrounded;
    private bool isCrouching;
    private bool isAttacking;


    private int comboCounter;


    // Used to flip sprites
    private float previousDirection = 1f;
    // Combo count
    private float maxGroundedComboAttack = 3;


    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    #endregion

    #region Awake/Update
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
        canMove = true;

        lockStateChange = false;
        comboCounter = 0;
        currentState = PlayerState.Idle;
    }

    private void Update()
    {
        HandleState();
        MoveCharacter();
        animator.SetFloat("VerticalSpeed", rigid.linearVelocity.y);
    }

    #endregion

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

            // Priorities
            // Dashing/Backdashing > CrouchAttack > GroundAttack > Jumping > Falling > Running > Crouching > Idle
            // Dash
            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                canAttack = false;
                ChangeState(PlayerState.Dashing);
                StartCoroutine(DashCharacter());
            }
            // Backdash
            else if (Input.GetKeyDown(KeyCode.LeftControl) && canDash && isGrounded)
            {
                canAttack = false;
                ChangeState(PlayerState.BackDashing);
                StartCoroutine(BackDashCharacter());
            }
            // Crouch Attack
            else if (Input.GetKeyDown(KeyCode.Z) && canAttack && isCrouching)
            {
                ChangeState(PlayerState.CrouchAttack);
                StartCoroutine(CrouchAttack());
            }
            // Ground Attack -> Can be cancelled by dashing
            else if (Input.GetKeyDown(KeyCode.Z) && canAttack && (!isCrouching) && isGrounded)
            {
                lockStateChange = true;
                ChangeState(PlayerState.Attacking);
                StartCoroutine(GroundedComboAttack());
            }
            // Jump
            else if (Input.GetKeyDown(KeyCode.Space) && canMove && isGrounded)
            {
                ChangeState(PlayerState.Jumping);
                StartCoroutine(JumpCharacter());
            }
            // Fall
            else if (!isGrounded && rigid.linearVelocity.y < 0)
            {
                ChangeState(PlayerState.Falling);
            }
            // Run
            else if ((moveInput != 0) && !(isDashing || isBackDashing))
            {
                if (Mathf.Sign(moveInput) != previousDirection)
                {
                    spriteRenderer.flipX = !spriteRenderer.flipX;
                    previousDirection = Mathf.Sign(moveInput);
                }
                else if (canMove)
                {
                    ChangeState(PlayerState.Running);
                }
            }
            // Crouch
            else if (Input.GetKeyDown(KeyCode.DownArrow) && isGrounded)
            {
                isCrouching = true;
                canMove = false;
                canDash = false;
                ChangeState(PlayerState.enterCrouching);
            }
            else if (Input.GetKey(KeyCode.DownArrow) && isGrounded)
            {
                ChangeState(PlayerState.Crouching);
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow) && isGrounded)
            {
                isCrouching = false;
                canMove = true;
                canDash = true;
                ChangeState(PlayerState.exitCrouching);
            }
            // Idle
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

        animator.SetBool("isEnterCrouching", currentState == PlayerState.enterCrouching);
        animator.SetBool("isCrouching", currentState == PlayerState.Crouching);
        animator.SetBool("isExitCrouching", currentState == PlayerState.exitCrouching);

        animator.SetBool("isCrouchAttacking", currentState == PlayerState.CrouchAttack);

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

    #endregion

    #region Basic Character Movements

    private IEnumerator DashCharacter()
    {
        isDashing = true;
        canDash = false;
        float dashTimer = 0f;
        // Check facing direction
        float dashDirection = spriteRenderer.flipX ? -1f : 1f;

        while (dashTimer < dashDuration)
        {
            rigid.linearVelocity = new Vector2(dashDirection * dashSpeed, rigid.linearVelocity.y);
            dashTimer += Time.deltaTime;

            yield return null;
        }

        isDashing = false;
        canAttack = true;

        // Dash cooldown
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private IEnumerator BackDashCharacter()
    {
        isBackDashing = true;
        canDash = false;
        float dashTimer = 0f;
        // Check facing direction
        float dashDirection = spriteRenderer.flipX ? 1f : -1f;

        while (dashTimer < dashDuration)
        {
            rigid.linearVelocity = new Vector2(dashDirection * backDashSpeed, rigid.linearVelocity.y);
            dashTimer += Time.deltaTime;

            yield return null;
        }

        isBackDashing = false;
        canAttack = true;

        // Dash cooldown
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void MoveCharacter()
    {
        if (canMove)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            rigid.linearVelocity = new Vector2(moveInput * moveSpeed, rigid.linearVelocity.y);
        }
    }

    private IEnumerator JumpCharacter()
    {
        isGrounded = false;
        isJumping = true;

        float jumpTimer = 0f;
        float currentJumpForce = baseJumpForce;
        float jumpAcceleration = 1.5f;

        // Apply initial jump force
        rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, currentJumpForce);

        // Continue applying force while the button is held
        while (Input.GetKey(KeyCode.Space) && jumpTimer < maxJumpHoldTime)
        {
            rigid.linearVelocity += holdJumpForce * Time.deltaTime * Vector2.up;
            jumpTimer += Time.deltaTime;

            // Reduce jump force over time to create a smooth curve
            currentJumpForce = Mathf.Max(currentJumpForce - (jumpAcceleration * Time.deltaTime), 0);

            yield return null;
        }

        isJumping = false;
    }

    #endregion

    #region Attack

    private enum AttackType
    {
        GroundedComboAttack1,
        GroundedComboAttack2,
        GroundedComboAttack3,
        CrouchAttack
    }

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
                StartCoroutine(Attack(AttackType.GroundedComboAttack1));
                break;
            case 2:
                ChangeState(PlayerState.GroundedComboAttack2);
                StartCoroutine(Attack(AttackType.GroundedComboAttack2));
                break;
            case 3:
                ChangeState(PlayerState.GroundedComboAttack3);
                StartCoroutine(Attack(AttackType.GroundedComboAttack3));
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

    private IEnumerator CrouchAttack()
    {
        canAttack = false;

        StartCoroutine(Attack(AttackType.CrouchAttack));

        yield return new WaitForSeconds(0.22f);

        canAttack = true;
    }

    #endregion

    #region Interactions

    private IEnumerator Attack(AttackType type)
    {
        float attackTimer = 0;
        float maxScanTime = 0;
        float attackDamage = 0;

        switch (type)
        {
            case AttackType.GroundedComboAttack1:
                maxScanTime = 0.14f;
                attackDamage = damage_GroundedComboAttack[0];
                break;
            case AttackType.GroundedComboAttack2:
                maxScanTime = 0.12f;
                attackDamage = damage_GroundedComboAttack[1];
                break;
            case AttackType.GroundedComboAttack3:
                maxScanTime = 0.08f;
                attackDamage = damage_GroundedComboAttack[2];
                break;
            case AttackType.CrouchAttack:
                maxScanTime = 0.12f;
                attackDamage = damage_CrouchAttack;
                break;
            default:
                Debug.Log("Attack type?: " + type);
                break;
        }

        // hit scan

        yield return null;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && rigid.linearVelocity.y <= 0)
        {
            isGrounded = true;
            animator.SetBool("isGrounded", isGrounded);
            ChangeState(PlayerState.Idle);
        }
    }

    #endregion
}