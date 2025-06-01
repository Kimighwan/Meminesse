using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class PlayerController : MonoBehaviour
{
    #region Variables/References

    [SerializeField] float moveSpeed;

    // Jump parameters
    [SerializeField] float maxJumpHoldTime = 0.3f;
    [SerializeField] float baseJumpForce = 14f;
    [SerializeField] float holdJumpForce = 30f;
    [SerializeField] float fallGravityScale = 3.5f;
    [SerializeField] float jumpGravityScale = 2.2f;
    [SerializeField] float normalGravityScale = 2.5f;

    // Dash parameters
    [SerializeField] float dashSpeed;
    [SerializeField] float backDashSpeed;
    [SerializeField] float dashDuration;
    [SerializeField] float dashCooldown;

    // Attack speed
    [SerializeField] float attackCoolDown;
    [SerializeField] float maxAttackInterval;

    // Attack damages
    [SerializeField] float[] damage_GroundedComboAttack = new float[3];
    [SerializeField] float damage_CrouchAttack;
    [SerializeField] float damage_AirAttack;
    [SerializeField] float damage_AirHeavyAttack;

    // Minimum height required above ground for AirHeavyAttack
    [SerializeField] float minHeightForAirHeavyAttack = 2f; 

    // Distance to check for ground
    [SerializeField] float groundCheckDistance = 0.1f; 
    // Width between ground check points
    [SerializeField] float groundCheckWidth = 0.4f;   

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
        CrouchAttack,
        AirAttack,
        AirHeavyAttack_Swing, AirHeavyAttack_Fall, AirHeavyAttack_Hit
    }

    private PlayerState currentState;

    // Locks
    private bool canDash = true;
    private bool canAttack = true;
    private bool canMove = true;
    private bool canCrouch = true;
    private bool lockInput = false;

    // Current states
    private bool isDashing;
    private bool isBackDashing;
    private bool isGrounded;
    private bool isCrouching;
    private bool isAttacking;
    private bool isJumping;
    // For GroundedComboAttack
    private int comboCounter = 0;

    // Used to flip sprites
    private float previousDirection = 1f;
    // Combo count
    private float maxGroundedComboAttack = 3;

    private Coroutine activeAttackCoroutine;

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Cache commonly used vectors
    private static readonly Vector2 VectorUp = Vector2.up;
    private static readonly Vector2 VectorZero = Vector2.zero;
    private static readonly ContactPoint2D[] ContactPoints = new ContactPoint2D[4];

    // Cache components
    private Transform cachedTransform;
    private Vector2 currentVelocity;
    private int groundLayerMask;

    #endregion

    #region Awake/Update
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        cachedTransform = transform;

        rigid.freezeRotation = true;
        rigid.interpolation = RigidbodyInterpolation2D.Interpolate;
        rigid.linearDamping = 0;

        // Cache the ground layer mask
        groundLayerMask = LayerMask.GetMask("Ground");

        // Do initial ground check
        Vector2 position = cachedTransform.position;
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, groundCheckDistance, groundLayerMask);
        isGrounded = (hit.collider != null);
        Debug.Log("Initial ground check: " + isGrounded);
        
        currentState = isGrounded ? PlayerState.Idle : PlayerState.Falling;
        animator.SetBool("isGrounded", isGrounded);
    }

    private void Update()
    {
        CheckGrounded();
        ManageInputs();
        MoveCharacter();
        currentVelocity = rigid.linearVelocity;
        animator.SetFloat("VerticalSpeed", currentVelocity.y);
    }

    #endregion

    #region Input/State Management

    private void ChangeState(PlayerState newState)
    {
        // Prevent any other state changes during AirHeavyAttack sequence
        if (IsAirHeavyAttackState(currentState))
        {
            // Only allow transitions between AirHeavyAttack states or from AirHeavyAttack_Hit to other states
            if (!IsAirHeavyAttackState(newState) && currentState != PlayerState.AirHeavyAttack_Hit)
            {
                return;
            }
        }

        if (currentState != newState)
        {
            currentState = newState;
            UpdateAnimation();
            ApplyStateEffects();
            Debug.Log("Entering State: " + currentState);
        }
    }

    private void ManageInputs()
    {
        if (!lockInput)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");

            // Change orientation -> Always happens (except while attacking)
            if ((moveInput != 0) && !isAttacking)
                if (Mathf.Sign(moveInput) != previousDirection)
                {
                    spriteRenderer.flipX = !spriteRenderer.flipX;
                    previousDirection = Mathf.Sign(moveInput);
                }
            
            // Priorities
            // Dashing/Backdashing > CrouchAttack > AirHeavyAttack > AirAttack > GroundAttack > Jumping > Falling > Crouching > Idle
            
            // Dash
            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                lockInput = true;
                ChangeState(PlayerState.Dashing);
                StartCoroutine(DashCharacter());
            }
            // Backdash
            else if (Input.GetKeyDown(KeyCode.LeftControl) && canDash && isGrounded)
            {
                lockInput = true;
                ChangeState(PlayerState.BackDashing);
                StartCoroutine(BackDashCharacter());
            }
            // Air Heavy Attack -> Requires certain height to perform
            else if (Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.DownArrow) && canAttack && !isGrounded && IsHighEnoughForAirHeavyAttack())
            {
                canMove = false;
                ChangeState(PlayerState.AirHeavyAttack_Swing);
                StartCoroutine(AirHeavyAttack());
            }
            // Crouch Attack -> Check isCrouching and current state
            else if (Input.GetKeyDown(KeyCode.Z) && canAttack && (isCrouching || currentState == PlayerState.Crouching))
            {
                canMove = false;
                ChangeState(PlayerState.CrouchAttack);
                StartCoroutine(CrouchAttack());
            }
            // Air Attack
            else if (Input.GetKeyDown(KeyCode.Z) && canAttack && !isGrounded)
            {
                canMove = false;
                ChangeState(PlayerState.AirAttack);
                StartCoroutine(AirAttack());
            }
            // Grounded Combo Attack -> Can be cancelled by dashing
            else if (Input.GetKeyDown(KeyCode.Z) && canAttack && isGrounded && !isCrouching && currentState != PlayerState.Crouching)
            {
                canMove = false;
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
            else if (!isGrounded && rigid.linearVelocity.y < 0 && !isAttacking && !isDashing && !isBackDashing)
            {
                ChangeState(PlayerState.Falling);
            }
            // Crouch
            else if (Input.GetKeyDown(KeyCode.DownArrow) && isGrounded && canCrouch)
            {
                // Enter Crouch
                isCrouching = true;
                canMove = false;
                canDash = false;
                ChangeState(PlayerState.enterCrouching);
            }
            else if (Input.GetKey(KeyCode.DownArrow) && isGrounded && canCrouch)
            {
                // Crouch
                canMove = false;
                ChangeState(PlayerState.Crouching);
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow) && isGrounded)
            {
                // Exit Crouch
                isCrouching = false;
                canMove = true;
                canDash = true;
                ChangeState(PlayerState.exitCrouching);
            }
            // Idle
            else if (rigid.linearVelocity.x == 0 && rigid.linearVelocity.y == 0 && isGrounded)
            {
                ChangeState(PlayerState.Idle);
            }

            // Move
            if (moveInput != 0)
                if (!(isDashing || isBackDashing || isCrouching || isJumping) && canMove && isGrounded)
                    ChangeState(PlayerState.Running);

            animator.SetBool("isGrounded", isGrounded);
        }
    }

    private void UpdateAnimation()
    {
        // Reset combo if not in combo state
        if (!IsInComboState(currentState))
        {
            animator.SetInteger("GroundedComboAttack", 0);
        }

        // Update basic states
        animator.SetBool("isIdle", currentState == PlayerState.Idle);
        animator.SetBool("isRunning", currentState == PlayerState.Running);
        animator.SetBool("isJumping", currentState == PlayerState.Jumping);
        animator.SetBool("isFalling", currentState == PlayerState.Falling);
        animator.SetBool("isDashing", currentState == PlayerState.Dashing);
        animator.SetBool("isBackDashing", currentState == PlayerState.BackDashing);

        // Update crouch states - ensure these are mutually exclusive
        bool isEnteringCrouch = (currentState == PlayerState.enterCrouching);
        bool isFullyCrouched = (currentState == PlayerState.Crouching);
        bool isExitingCrouch = (currentState == PlayerState.exitCrouching);
        
        animator.SetBool("isEnterCrouching", isEnteringCrouch);
        animator.SetBool("isCrouching", isFullyCrouched);
        animator.SetBool("isExitCrouching", isExitingCrouch);

        // Update attack states
        animator.SetBool("isCrouchAttacking", currentState == PlayerState.CrouchAttack);
        animator.SetBool("isAirAttacking", currentState == PlayerState.AirAttack);
        
        // Air Heavy Attack states
        animator.SetBool("isAirHeavyAttack_Swing", currentState == PlayerState.AirHeavyAttack_Swing);
        animator.SetBool("isAirHeavyAttack_Fall", currentState == PlayerState.AirHeavyAttack_Fall);
        animator.SetBool("isAirHeavyAttack_Hit", currentState == PlayerState.AirHeavyAttack_Hit);

        // Update combo states
        if (currentState == PlayerState.GroundedComboAttack1)
            animator.SetInteger("GroundedComboAttack", 1);
        else if (currentState == PlayerState.GroundedComboAttack2)
            animator.SetInteger("GroundedComboAttack", 2);
        else if (currentState == PlayerState.GroundedComboAttack3)
            animator.SetInteger("GroundedComboAttack", 3);
    }

    private void ApplyStateEffects()
    {
        // Only gravity fixes for now, more to be added later
        AdjustGravity();
    }

    private void AdjustGravity()
    {
        // Default gravity
        float targetGravity = normalGravityScale;

        // If airborne, default to fall gravity(heavier gravity) unless in special states
        if (!isGrounded)
        {
            targetGravity = fallGravityScale;

            // Special airborne state handling
            switch (currentState)
            {
                case PlayerState.Jumping:
                    // Apply lower gravity while jumping up, regardless of horizontal movement
                    targetGravity = jumpGravityScale;
                    break;

                case PlayerState.Falling:
                    // Apply fall gravity while falling
                    targetGravity = fallGravityScale;
                    break;

                case PlayerState.AirAttack:
                    // No gravity during air attack
                    targetGravity = 0f;
                    break;

                case PlayerState.AirHeavyAttack_Fall:
                    // Increased gravity for heavy attack fall
                    targetGravity = 4f;
                    break;

                case PlayerState.AirHeavyAttack_Swing:
                case PlayerState.AirHeavyAttack_Hit:
                    // No gravity during these phases
                    targetGravity = 0f;
                    break;
            }
        }

        rigid.gravityScale = targetGravity;
    }
    
    // State checks
    private bool IsAirHeavyAttackState(PlayerState state)
    {
        return state == PlayerState.AirHeavyAttack_Swing ||
               state == PlayerState.AirHeavyAttack_Fall ||
               state == PlayerState.AirHeavyAttack_Hit;
    }

    private bool IsInComboState(PlayerState state)
    {
        return state == PlayerState.GroundedComboAttack1 ||
               state == PlayerState.GroundedComboAttack2 ||
               state == PlayerState.GroundedComboAttack3 ||
               state == PlayerState.Attacking;
    }

    private bool IsAirborneState(PlayerState state)
    {
        return state == PlayerState.Jumping || 
               state == PlayerState.Falling || 
               state == PlayerState.AirAttack || 
               IsAirHeavyAttackState(state) ||
               state == PlayerState.Dashing;
    }

    #endregion

    #region Basic Character Movements

    private IEnumerator DashCharacter()
    {
        isDashing = true;
        canDash = false;
        float dashTimer = 0f;

        // Get current input direction for dash, don't rely on previous direction
        float moveInput = Input.GetAxisRaw("Horizontal");
        float dashDirection = moveInput != 0 ? Mathf.Sign(moveInput) : (spriteRenderer.flipX ? -1f : 1f);

        // Update orientation based on dash direction
        if (Mathf.Sign(dashDirection) != previousDirection)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            previousDirection = Mathf.Sign(dashDirection);
        }

        while (dashTimer < dashDuration)
        {
            rigid.linearVelocity = new Vector2(dashDirection * dashSpeed, 0);
            dashTimer += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
        canAttack = true;
        canMove = true;
        lockInput = false;

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
            rigid.linearVelocity = new Vector2(dashDirection * backDashSpeed, 0);
            dashTimer += Time.deltaTime;

            yield return null;
        }

        isBackDashing = false;
        canAttack = true;
        canMove = true;
        lockInput = false;

        // Dash cooldown
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void MoveCharacter()
    {
        if (canMove)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            if (moveInput != 0)
            {
                currentVelocity.x = moveInput * moveSpeed;
                currentVelocity.y = rigid.linearVelocity.y;
                rigid.linearVelocity = currentVelocity;
            }
            else if (rigid.linearVelocity.x != 0)
            {
                currentVelocity.x = 0;
                currentVelocity.y = rigid.linearVelocity.y;
                rigid.linearVelocity = currentVelocity;
            }
        }
    }

    private IEnumerator JumpCharacter()
    {
        isGrounded = false;
        isJumping = true;

        float jumpTimer = 0f;
        
        // Apply initial jump force
        rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, baseJumpForce);

        // Continue applying force while the button is held
        while (Input.GetKey(KeyCode.Space) && jumpTimer < maxJumpHoldTime)
        {
            rigid.linearVelocity += VectorUp * holdJumpForce * Time.deltaTime;
            jumpTimer += Time.deltaTime;
            yield return null;
        }

        // If player released jump early, add a bit more downward force for snappier feel
        if (jumpTimer < maxJumpHoldTime && rigid.linearVelocity.y > 0)
        {
            rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, rigid.linearVelocity.y * 0.5f);
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
        CrouchAttack,
        AirAttack,
        AirHeavyAttack,
        // TODO: Add another attack type for the hit animation
    }

    private IEnumerator GroundedComboAttack()
    {
        isAttacking = true;
        
        // Stop all momentum when starting attack
        rigid.linearVelocity = VectorZero;

        // Stop previous attack coroutine before starting a new one as to prevent overlapping
        if (activeAttackCoroutine != null)
        {
            StopCoroutine(activeAttackCoroutine);
        }

        activeAttackCoroutine = StartCoroutine(ComboAttackSequence());
        yield return null;
    }

    private IEnumerator ComboAttackSequence()
    {
        canAttack = false;
        comboCounter++;

        float startingTime = Time.time;
        float currentTime = startingTime;

        switch (comboCounter)
        {
            case 1:
                ChangeState(PlayerState.GroundedComboAttack1);
                activeAttackCoroutine = StartCoroutine(Attack(AttackType.GroundedComboAttack1));
                break;
            case 2:
                ChangeState(PlayerState.GroundedComboAttack2);
                activeAttackCoroutine = StartCoroutine(Attack(AttackType.GroundedComboAttack2));
                break;
            case 3:
                ChangeState(PlayerState.GroundedComboAttack3);
                activeAttackCoroutine = StartCoroutine(Attack(AttackType.GroundedComboAttack3));
                break;
        }

        // Cooldown until next combo
        yield return new WaitForSeconds(attackCoolDown);

        // Next combo input window
        while (currentTime < startingTime + maxAttackInterval - attackCoolDown)
        {
            currentTime = Time.time;
            if (Input.GetKeyDown(KeyCode.Z) && comboCounter < maxGroundedComboAttack)
            {
                // This coroutine will be stopped by the new one
                StartNewComboAttack();
            }
            yield return null;
        }

        if (comboCounter == 3)
        {
            // Special delay after third attack
            // Only allow dashing during this time
            canMove = false;
            canAttack = false;
            canCrouch = false;
            isAttacking = true;
            
            // Delay duration after third attack
            yield return new WaitForSeconds(0.4f);
            
            // Reset states
            canMove = true;
            canAttack = true;
            canCrouch = true;
            isAttacking = false;
        }

        // Reset combo state first
        comboCounter = 0;
        animator.SetInteger("GroundedComboAttack", 0);
        
        // Ensure one frame of state update
        yield return null;
        
        // Now check for transitions
        if (Input.GetKey(KeyCode.DownArrow) && canCrouch)
        {
            isCrouching = true;
            canMove = false;
            canDash = false;
            ChangeState(PlayerState.enterCrouching);
        }
        else if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f && canMove)
        {
            ChangeState(PlayerState.Running);
        }
        else
        {
            ChangeState(PlayerState.Idle);
        }

        canAttack = true;
        isAttacking = false;
        canMove = true;
    }

    private void StartNewComboAttack()
    {
        // Stop previous coroutine before restarting attack
        if (activeAttackCoroutine != null)
        {
            StopCoroutine(activeAttackCoroutine);
        }

        activeAttackCoroutine = StartCoroutine(GroundedComboAttack());
    }

    private IEnumerator CrouchAttack()
    {
        isAttacking = true;
        canAttack = false;

        StartCoroutine(Attack(AttackType.CrouchAttack));

        yield return new WaitForSeconds(0.2f);

        isAttacking = false;
        canAttack = true;
    }

    private IEnumerator AirAttack()
    {
        isAttacking = true;
        canAttack = false;
        canDash = false;
        canMove = false;
        
        StartCoroutine(Attack(AttackType.AirAttack));

        float attackDuration = 0.32f;
        float elapsedTime = 0f;
        
        // Complete stop during attack animation
        while (elapsedTime < attackDuration)
        {
            rigid.linearVelocity = VectorZero;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        // Apply initial downward force to start falling
        rigid.linearVelocity = new Vector2(0f, -3f);
        ChangeState(PlayerState.Falling);

        canDash = true;  
        // Keep canMove false to prevent horizontal movement

        // Allow orientation changes during fall
        // Exit loop if dashing
        while (!isGrounded && !isDashing)
        {
            // Allow orientation changes based on horizontal input
            float moveInput = Input.GetAxisRaw("Horizontal");
            if (moveInput != 0 && Mathf.Sign(moveInput) != previousDirection)
            {
                spriteRenderer.flipX = !spriteRenderer.flipX;
                previousDirection = Mathf.Sign(moveInput);
            }
            yield return null;
        }

        // Reset states once grounded
        isAttacking = false;
        canAttack = true;
        canMove = true;
    }

    private IEnumerator AirHeavyAttack()
    {
        isAttacking = true;
        canAttack = false;
        canMove = false;
        canDash = false;
        isGrounded = false;
        
        // Phase 1: Swing animation
        ChangeState(PlayerState.AirHeavyAttack_Swing);
        // TODO: Add another attack type for the hit animation
        StartCoroutine(Attack(AttackType.AirHeavyAttack));
        
        // Duration of swing animation
        yield return new WaitForSeconds(0.2f);
        
        // Phase 2: Fall animation
        ChangeState(PlayerState.AirHeavyAttack_Fall);
        
        // Apply increased gravity and downward force
        rigid.gravityScale = 4f;
        currentVelocity.x = 0f;
        currentVelocity.y = -15f;
        rigid.linearVelocity = currentVelocity;
        
        // Fall until the player hits the ground
        // TODO: Add another attack type for the hit animation
        while (currentState == PlayerState.AirHeavyAttack_Fall)
        {
            // Lock horizontal movement but maintain vertical momentum
            currentVelocity.x = 0f;
            currentVelocity.y = rigid.linearVelocity.y;
            rigid.linearVelocity = currentVelocity;
            yield return null;
        }
    }

    private IEnumerator FinishAirHeavyAttack()
    {
        isGrounded = true;
        animator.SetBool("isGrounded", isGrounded);
        
        // Duration of hit animation
        yield return new WaitForSeconds(0.28f);

        // Reset states
        isAttacking = false;
        canAttack = true;
        canMove = true;
        canDash = true;
        rigid.gravityScale = normalGravityScale;

        // Check if player is holding down after landing
        if (Input.GetKey(KeyCode.DownArrow))
        {
            isCrouching = true;
            canMove = false;
            canDash = false;
            ChangeState(PlayerState.Crouching);
        }
        else
        {
            // Only force idle state if no other input is being processed
            float moveInput = Input.GetAxisRaw("Horizontal");
            ChangeState(moveInput != 0 && canMove ? PlayerState.Running : PlayerState.Idle);
        }
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
            case AttackType.AirAttack:
                // Needs to be adjusted
                maxScanTime = 0.15f;
                attackDamage = damage_AirAttack;
                break;
            case AttackType.AirHeavyAttack:
                // Needs to be adjusted
                maxScanTime = 0.15f;
                attackDamage = damage_AirHeavyAttack;
                break;
            default:
                Debug.Log("What's this attack type?: " + type);
                break;
        }

        

        // hit scan

        yield return null;
    }
    
    private bool IsHighEnoughForAirHeavyAttack()
    {
        return !Physics2D.Raycast(cachedTransform.position, Vector2.down, minHeightForAirHeavyAttack, groundLayerMask);
    }
    
    private void CheckGrounded()
    {
        // Don't check during special states
        if (currentState == PlayerState.AirAttack || IsAirHeavyAttackState(currentState))
            return;

        // Cast two rays from the bottom edges of the player
        Vector2 centerPosition = cachedTransform.position;
        bool wasGrounded = isGrounded;

        // Cast rays from bottom edges of the player
        Vector2 rayStartLeft = centerPosition + (Vector2.left * groundCheckWidth * 0.5f);
        Vector2 rayStartRight = centerPosition + (Vector2.right * groundCheckWidth * 0.5f);
        
        // Check both bottom corners
        RaycastHit2D hitLeft = Physics2D.Raycast(rayStartLeft, Vector2.down, groundCheckDistance, groundLayerMask);
        RaycastHit2D hitRight = Physics2D.Raycast(rayStartRight, Vector2.down, groundCheckDistance, groundLayerMask);

        // Update grounded state
        isGrounded = (hitLeft.collider != null || hitRight.collider != null) && rigid.linearVelocity.y <= 0;
        
        // Handle state transitions
        if (isGrounded && currentState == PlayerState.Falling)
        {
            ChangeState(PlayerState.Idle);
        }
        else if (!isGrounded && !IsAirborneState(currentState))
        {
            ChangeState(PlayerState.Falling);
        }

        animator.SetBool("isGrounded", isGrounded);
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Only handle special case for air heavy attack landing
            if (currentState == PlayerState.AirHeavyAttack_Fall)
            {
                ChangeState(PlayerState.AirHeavyAttack_Hit);
                StartCoroutine(FinishAirHeavyAttack());
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("isGrounded", false);
        }
    }


    #endregion

}