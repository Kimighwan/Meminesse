using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using NUnit.Framework.Constraints;
using UnityEditor.Tilemaps;

public class PlayerController : MonoBehaviour
{
    #region Variables/References

    OneWayPlatform oneWayPlatform;

    [SerializeField] float moveSpeed = 5f;

    // Jump parameters - Not to be messsed
    [SerializeField] float maxJumpHoldTime = 1f;
    [SerializeField] float baseJumpForce = 7f;
    [SerializeField] float holdJumpForce = 12f;
    [SerializeField] float fallGravityScale = 3.5f;
    [SerializeField] float jumpGravityScale = 2.2f;
    [SerializeField] float normalGravityScale = 2.5f;
    [SerializeField] float maxFallSpeed = -20f;

    // Dash parameters
    [SerializeField] float dashSpeed = 12f;
    [SerializeField] float backDashSpeed = 9f;
    [SerializeField] float dashDuration = 0.3f;

    // Current attack type (Used in interactions)
    public AttackType currentAttackType;

    // Attack speed
    [SerializeField] float attackCoolDown = 0f;
    [SerializeField] float maxAttackInterval = 0.3f;

    // Attack damages
    [SerializeField] float[] damage_GroundedComboAttack = new float[3];
    [SerializeField] float damage_CrouchAttack;
    [SerializeField] float damage_AirAttack;
    [SerializeField] float damage_AirHeavyAttack;
    [SerializeField] float damage_HolySlash;
    [SerializeField] float damage_LightCut;

    // Skill cooldowns
    [SerializeField] float holySlashCoolDown = 30f;
    [SerializeField] float lightCutCoolDown = 15f;
    bool isHolySlashOnCoolDown = false;
    bool isLightCutOnCoolDown = false;


    // Hitboxes for combat
    private GameObject currentHitbox = null;
    [SerializeField] GameObject hitboxPivot;
    [SerializeField] GameObject[] groundAttackHitbox = new GameObject[3];
    [SerializeField] GameObject crouchAttackHitbox;
    [SerializeField] GameObject airAttackHitbox;
    [SerializeField] GameObject[] airHeavyAttackHitbox = new GameObject[3];
    [SerializeField] GameObject holySlashAttackHitbox;
    [SerializeField] GameObject lightCutAttackHitbox;

    // TBA

    // Player damage related
    private Vector2 attackerPosition;
    private bool isStunned;
    [SerializeField] float invincibleTimeAfterHit = 1.5f;
    private bool isInvincible;
    private bool isHurt;
    private bool isDead;
    private bool shieldRegenCountdown = false;
    // Related to top passive skill (balanced)
    private bool isRageMode = false;


    // Minimum height required above ground for AirHeavyAttack
    [SerializeField] float minHeightForAirHeavyAttack = 2f;

    // Distance to check for ground
    [SerializeField] float groundCheckDistance = 0.05f;
    // Width between ground check points
    [SerializeField] float groundCheckWidth = 0.8f;

    [SerializeField] MapController mapController;

    public enum PlayerState
    {
        // Movements
        Idle,
        Running,
        Jumping,
        Falling,
        Dashing,
        BackDashing,
        enterCrouching, Crouching, exitCrouching,

        // Combats
        Hurt,
        Dead,
        Healing,

        // Basic Attacks
        Attacking,
        GroundedComboAttack1, GroundedComboAttack2, GroundedComboAttack3,
        CrouchAttack,
        AirAttack,
        AirHeavyAttack_Swing, AirHeavyAttack_Fall, AirHeavyAttack_Hit,

        // Skills
        HolySlash,
        LightCut,
    }

    private PlayerState currentState;

    // Movement locks
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
    private Coroutine activeCoroutine;
    private Coroutine activeComboCoroutine;

    // For GroundedComboAttack
    private int comboCounter = 0;

    // Used to flip sprites
    private float previousDirection = 1f;

    // Combo count
    private float maxGroundedComboAttack = 3;

    private Rigidbody2D rigid;
    private SpriteRenderer playerSpriteRenderer;
    private Animator animator;

    // Cache components
    private static readonly Vector2 VectorUp = Vector2.up;
    private static readonly Vector2 VectorZero = Vector2.zero;
    private Transform cachedTransform;
    private Vector2 currentVelocity;

    // Layers
    private int groundLayerMask;

    #endregion

    #region Awake/Update
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        oneWayPlatform = GetComponent<OneWayPlatform>();
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

        currentState = (isGrounded ? PlayerState.Idle : PlayerState.Falling);
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
        // Uncontrollable state
        if (lockInput || Time.timeScale == 0)
        {
            return;
        }

        // Changing orientation -> Always happens (except while attacking)
        float moveInput = Input.GetAxisRaw("Horizontal");
        if ((moveInput != 0) && !isAttacking)
            if (Mathf.Sign(moveInput) != previousDirection)
            {
                playerSpriteRenderer.flipX = !playerSpriteRenderer.flipX;
                previousDirection = Mathf.Sign(moveInput);
            }

        // Priorities
        // Skills > Dashing/Backdashing > CrouchAttack > AirHeavyAttack > AirAttack > GroundAttack > Jumping > Falling > Crouching > Idle
        if(Input.GetKeyDown(KeyCode.P))
            oneWayPlatform.OneWayPlatfrom();

        // HolySlash
        if (Input.GetKeyDown(SettingDataManager.Instance.GetKeyCode("Skill1"))
                && isGrounded && canMove && canAttack && !isHolySlashOnCoolDown)
        {
            lockInput = true;
            ChangeState(PlayerState.HolySlash);
            StartCoroutine(HolySlash());
        }
        // LightCut
        else if (Input.GetKeyDown(SettingDataManager.Instance.GetKeyCode("Skill2"))
                && isGrounded && canMove && canAttack && !isLightCutOnCoolDown)
        {
            lockInput = true;
            ChangeState(PlayerState.LightCut);
            StartCoroutine(LightCut());
        }
        // Dash
        else if (Input.GetKeyDown(SettingDataManager.Instance.GetKeyCode("Dash"))
                && canDash)
        {
            lockInput = true;
            ChangeState(PlayerState.Dashing);
            StartCoroutine(DashCharacter());
        }
        // Backdash
        else if (Input.GetKeyDown(SettingDataManager.Instance.GetKeyCode("BackDash"))
                && canDash && isGrounded)
        {
            lockInput = true;
            ChangeState(PlayerState.BackDashing);
            StartCoroutine(BackDashCharacter());
        }
        // Air Heavy Attack -> Requires certain height to perform
        else if (Input.GetKeyDown(SettingDataManager.Instance.GetKeyCode("BasicAttack"))
                && Input.GetKey(SettingDataManager.Instance.GetKeyCode("Down"))
                && canAttack && !isGrounded && IsHighEnoughForAirHeavyAttack())
        {
            canMove = false;
            ChangeState(PlayerState.AirHeavyAttack_Swing);
            StartCoroutine(AirHeavyAttack());
        }
        // Crouch Attack -> Check isCrouching and current state
        else if (Input.GetKeyDown(SettingDataManager.Instance.GetKeyCode("BasicAttack"))
                && canAttack && (isCrouching || currentState == PlayerState.Crouching))
        {
            canMove = false;
            ChangeState(PlayerState.CrouchAttack);
            StartCoroutine(CrouchAttack());
        }
        // Air Attack
        else if (Input.GetKeyDown(SettingDataManager.Instance.GetKeyCode("BasicAttack"))
                && canAttack && !isGrounded)
        {
            canMove = false;
            ChangeState(PlayerState.AirAttack);
            StartCoroutine(AirAttack());
        }
        // Grounded Combo Attack -> Can be cancelled by dashing
        else if (Input.GetKeyDown(SettingDataManager.Instance.GetKeyCode("BasicAttack"))
                && canAttack && isGrounded && !isCrouching && currentState != PlayerState.Crouching)
        {
            canMove = false;
            ChangeState(PlayerState.Attacking);
            StartCoroutine(GroundedComboAttack());
        }
        // Jump
        else if (Input.GetKeyDown(SettingDataManager.Instance.GetKeyCode("Jump"))
                && canMove && isGrounded)
        {
            ChangeState(PlayerState.Jumping);
            StartCoroutine(JumpCharacter());
        }
        // Fall
        else if (!isGrounded && rigid.linearVelocity.y < -0.01 && !isAttacking && !isDashing && !isBackDashing)
        {
            ChangeState(PlayerState.Falling);
            LimitVelocity();
        }
        // Crouch
        else if (Input.GetKeyDown(SettingDataManager.Instance.GetKeyCode("Down"))
                && isGrounded && canCrouch)
        {
            // Enter Crouch
            isCrouching = true;
            canMove = false;
            canDash = false;
            ChangeState(PlayerState.enterCrouching);
        }
        else if (Input.GetKey(SettingDataManager.Instance.GetKeyCode("Down"))
                && isGrounded && canCrouch)
        {
            // Crouch
            canMove = false;
            ChangeState(PlayerState.Crouching);
        }
        else if (Input.GetKeyUp(SettingDataManager.Instance.GetKeyCode("Down"))
                && isGrounded)
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
        {
            if (!(isDashing || isBackDashing || isCrouching || isJumping) && canMove && isGrounded)
                ChangeState(PlayerState.Running);
        }

        animator.SetBool("isGrounded", isGrounded);
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
        animator.SetBool("isHurt", currentState == PlayerState.Hurt);
        animator.SetBool("isDead", currentState == PlayerState.Dead);

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

        // Update skill states
        animator.SetBool("isHolySlash", currentState == PlayerState.HolySlash);
        animator.SetBool("isLightCut", currentState == PlayerState.LightCut);
    }

    private void ApplyStateEffects()
    {
        // Adjust gravity based on state
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

    private void LimitVelocity()
    {
        // Falling speed = negative value
        if (rigid.linearVelocity.y < maxFallSpeed)
        {
            rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, maxFallSpeed);
        }
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

    #region Basic Character Behaviour
    private IEnumerator DashCharacter()
    {
        float dashCooldown = PlayerDataManager.Instance.GetDashCoolDown();

        isDashing = true;
        canDash = false;
        float dashTimer = 0f;

        // Get current input direction for dash, don't rely on previous direction
        float moveInput = Input.GetAxisRaw("Horizontal");
        float dashDirection = moveInput != 0 ? Mathf.Sign(moveInput) : (playerSpriteRenderer.flipX ? -1f : 1f);

        // Update orientation based on dash direction
        if (Mathf.Sign(dashDirection) != previousDirection)
        {
            playerSpriteRenderer.flipX = !playerSpriteRenderer.flipX;
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
        float dashCooldown = PlayerDataManager.Instance.GetDashCoolDown();

        isBackDashing = true;
        canDash = false;
        float dashTimer = 0f;
        // Check facing direction
        float dashDirection = playerSpriteRenderer.flipX ? 1f : -1f;

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
        if (lockInput || Time.timeScale == 0)
            return;

        if (canMove)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");

            if (moveInput != 0 && !IsTouchingWall())
            {
                Vector2 velocity = rigid.linearVelocity;
                velocity.x = moveInput * moveSpeed;
                rigid.linearVelocity = velocity;
            }
            else if (rigid.linearVelocity.x != 0)
            {
                Vector2 velocity = rigid.linearVelocity;
                velocity.x = 0;
                rigid.linearVelocity = velocity;
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
        while (Input.GetKey(SettingDataManager.Instance.GetKeyCode("Jump")) && jumpTimer < maxJumpHoldTime)
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

    private IEnumerator Hurt()
    {
        int topPassiveLevel_beginner = PlayerDataManager.Instance.GetTopPassive(3);

        ChangeState(PlayerState.Hurt);
        isHurt = true;
        isInvincible = true;
        StartCoroutine(FlickerInvincibility());
        lockInput = true;
        float animationTime = 0.153f;

        yield return new WaitForSeconds(animationTime);
        lockInput = false;
        Debug.Log("lockinput: " + lockInput);

        if (topPassiveLevel_beginner >= 3)
        {
            Debug.Log("Top Passive Beginner Level 3: bonus invincibility time");
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(invincibleTimeAfterHit - animationTime);
        isInvincible = false;
        Debug.Log("isinvincible: " + isInvincible);
    }

    private IEnumerator Dead()
    {
        ChangeState(PlayerState.Dead);
        isInvincible = true;
        isDead = true;
        lockInput = true;
        yield return new WaitForSeconds(3f);

        mapController.RespawnPlayer(this);
        isDead = false;
        isInvincible = false;
        lockInput = false;
        ChangeState(PlayerState.Idle);
    }

    private IEnumerator FlickerInvincibility()
    {
        float flickerInterval = 0.1f;
        while (isInvincible)
        {
            playerSpriteRenderer.enabled = !playerSpriteRenderer.enabled;
            yield return new WaitForSeconds(flickerInterval);
        }
        playerSpriteRenderer.enabled = true;
    }

    private IEnumerator ShieldRegeneration(int skillLevel)
    {
        shieldRegenCountdown = true;

        // Timer for shield regeneration after taking damage
        switch (skillLevel)
        {
            case 1:
                yield return new WaitForSeconds(60f);
                break;
            case 2:
                yield return new WaitForSeconds(45f);
                break;
            case 3:
                yield return new WaitForSeconds(30f);
                break;
            default:
                if (skillLevel != 0)
                    Debug.LogError("Invalid skill level for Shield Regeneration: " + skillLevel);
                break;
        }

        Debug.Log("Shield Regenerated");

        shieldRegenCountdown = false;

        yield return null;
    }

    private IEnumerator EnterRageMode(int skillLevel)
    {
        isRageMode = true;

        float rageDuration = 0f;
        switch (skillLevel)
        {
            case 1:
                rageDuration = 4f;
                break;
            case 2:
                rageDuration = 6f;
                break;
            case 3:
                rageDuration = 6f;
                break;
            default:
                if (skillLevel != 0)
                {
                    Debug.LogError("Invalid skill level for Rage Mode: " + skillLevel);
                }
                break;
        }

        yield return new WaitForSeconds(rageDuration);

        isRageMode = false;
    }

    #endregion

    #region Attack

    public enum AttackType
    {
        GroundedComboAttack1,
        GroundedComboAttack2,
        GroundedComboAttack3,
        CrouchAttack,
        AirAttack,
        AirHeavyAttack,
        HolySlash,
        LightCut,
    }

    private IEnumerator GroundedComboAttack()
    {
        isAttacking = true;

        // Stop all momentum when starting attack
        rigid.linearVelocity = VectorZero;

        // Stop previous attack coroutine before starting a new one as to prevent overlapping
        if (activeComboCoroutine != null)
        {
            StopCoroutine(activeComboCoroutine);
            currentHitbox.SetActive(false);
        }

        activeComboCoroutine = StartCoroutine(ComboAttackSequence());
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
                activeComboCoroutine = StartCoroutine(Attack(AttackType.GroundedComboAttack1));
                break;
            case 2:
                ChangeState(PlayerState.GroundedComboAttack2);
                activeComboCoroutine = StartCoroutine(Attack(AttackType.GroundedComboAttack2));
                break;
            case 3:
                ChangeState(PlayerState.GroundedComboAttack3);
                activeComboCoroutine = StartCoroutine(Attack(AttackType.GroundedComboAttack3));
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
        canMove = true;
    }

    private void StartNewComboAttack()
    {
        // Stop previous coroutine before restarting attack
        if (activeComboCoroutine != null)
        {
            StopCoroutine(activeComboCoroutine);
            currentHitbox.SetActive(false);
        }

        activeComboCoroutine = StartCoroutine(GroundedComboAttack());
    }

    private IEnumerator CrouchAttack()
    {
        isAttacking = true;
        canAttack = false;

        float attackDuration = 0.2f;

        StartCoroutine(Attack(AttackType.CrouchAttack));

        yield return new WaitForSeconds(attackDuration);

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
                playerSpriteRenderer.flipX = !playerSpriteRenderer.flipX;
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
        rigid.gravityScale = 14f;
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

    private IEnumerator HolySlash()
    {
        isHolySlashOnCoolDown = true;
        isAttacking = true;
        canAttack = false;
        canDash = false;
        canMove = false;

        float chargeDuration = 1.2f;
        yield return new WaitForSeconds(chargeDuration);

        StartCoroutine(Attack(AttackType.HolySlash));

        // Remaining time until the animation ends
        yield return new WaitForSeconds(0.2f);
        isAttacking = false;
        canAttack = true;
        canMove = true;
        canDash = true;
        lockInput = false;

        float cooldownReduction = PlayerDataManager.Instance.GetSkillCoolDown();
        float effectiveCoolDown = holySlashCoolDown * (1 - cooldownReduction);
        yield return new WaitForSeconds(effectiveCoolDown);
        isHolySlashOnCoolDown = false;
    }

    private IEnumerator LightCut()
    {
        isLightCutOnCoolDown = true;
        isAttacking = true;
        canAttack = false;
        canDash = false;
        canMove = false;

        float chargeDuration = 0.63f;
        yield return new WaitForSeconds(chargeDuration);

        StartCoroutine(Attack(AttackType.LightCut));

        // Dash across the screen while attacking
        float attackDuration = 0.04f;
        float attackTimer = 0f;

        // Movement direction is based upon the facing direction of the player
        float dashDirection = playerSpriteRenderer.flipX ? -1 : 1;
        isInvincible = true;

        // Actual movement (invincible while dashing)
        while (attackTimer < attackDuration)
        {
            rigid.linearVelocity = new Vector2(dashDirection * dashSpeed * 10, 0);
            attackTimer += Time.deltaTime;
            yield return null;
        }

        isInvincible = false;
        // Immediate stop
        rigid.linearVelocity = new Vector2(0, 0);
        yield return new WaitForSeconds(0.06f);
        isAttacking = false;
        canAttack = true;
        canMove = true;
        canDash = true;
        lockInput = false;

        float cooldownReduction = PlayerDataManager.Instance.GetSkillCoolDown();
        float effectiveCoolDown = lightCutCoolDown * (1 - cooldownReduction);
        yield return new WaitForSeconds(effectiveCoolDown);
        isLightCutOnCoolDown = false;
    }

    private float GetSkillBonusDamage(int balancedLevel)
    {
        float bonusDamage = 0;
        switch (balancedLevel)
        {
            case 1:
                bonusDamage = 1;
                break;
            case 2:
                bonusDamage = 2;
                break;
            case 3:
                bonusDamage = 3;
                break;
            default:
                bonusDamage = 0;
                if (balancedLevel != 0)
                {
                    Debug.LogError("Invalid balanced skill level for bonus damage: " + balancedLevel);
                }
                break;
        }
        return bonusDamage;
    }

    #endregion

    #region Interactions

    private IEnumerator Attack(AttackType type)
    {
        float maxScanTime = 0;
        float attackMultiplier = 1;

        float baseAttack = PlayerDataManager.Instance.GetDamage();
        float weaponLevel = PlayerDataManager.Instance.GetWeaponLevel();
        int topPassiveLevel_balanced = PlayerDataManager.Instance.GetTopPassive(2);

        float skillBonus = GetSkillBonusDamage(topPassiveLevel_balanced);

        baseAttack *= (1 + (weaponLevel - 1 + skillBonus) * 0.1f);
        // e.g. Level 1 weapon = 100% base damage, Level 2 weapon = 110% base damage, etc.
        // skillBonus can be added later when top passive skills are leveled up

        // Select hitboxes
        switch (type)
        {
            case AttackType.GroundedComboAttack1:
                currentHitbox = groundAttackHitbox[0];
                maxScanTime = 0.14f;
                attackMultiplier = damage_GroundedComboAttack[0];
                break;
            case AttackType.GroundedComboAttack2:
                currentHitbox = groundAttackHitbox[1];
                maxScanTime = 0.12f;
                attackMultiplier = damage_GroundedComboAttack[1];
                break;
            case AttackType.GroundedComboAttack3:
                currentHitbox = groundAttackHitbox[2];
                maxScanTime = 0.22f;
                attackMultiplier = damage_GroundedComboAttack[2];
                break;
            case AttackType.CrouchAttack:
                currentHitbox = crouchAttackHitbox;
                maxScanTime = 0.12f;
                attackMultiplier = damage_CrouchAttack;
                break;
            case AttackType.AirAttack:
                currentHitbox = airAttackHitbox;
                maxScanTime = 0.13f;
                attackMultiplier = damage_AirAttack;
                break;
            case AttackType.AirHeavyAttack:
                currentHitbox = airHeavyAttackHitbox[0];
                maxScanTime = 0.15f;
                attackMultiplier = damage_AirHeavyAttack;
                break;
            case AttackType.HolySlash:
                currentHitbox = holySlashAttackHitbox;
                maxScanTime = 0.06f;
                attackMultiplier = damage_HolySlash;
                break;
            case AttackType.LightCut:
                currentHitbox = lightCutAttackHitbox;
                maxScanTime = 0.06f;
                attackMultiplier = damage_LightCut;
                break;
            default:
                Debug.Log("What's this?\nHow did we get here?: " + type);
                break;
        }

        // Get defense ignore from playerdatamanager
        float defIgnore = PlayerDataManager.Instance.GetDefenseIgnore();

        PlayerAttackHitbox hitboxManager = currentHitbox.GetComponent<PlayerAttackHitbox>();
        hitboxManager.setAttackType(type);
        hitboxManager.setAttackDamage(baseAttack * attackMultiplier);
        hitboxManager.setPlayerPosition(transform.position);
        hitboxManager.setDefenceIngore(defIgnore);

        // Prepare the hitbox for this attack activation (clears per-activation cache)
        hitboxManager.BeginAttack();
        currentHitbox.SetActive(true);

        // Flip the hitbox according to the player's facing direction
        if (playerSpriteRenderer.flipX)
        {
            hitboxPivot.transform.localScale = new Vector3(-1, 1, 0);
        }
        else
        {
            hitboxPivot.transform.localScale = new Vector3(1, 1, 0);
        }

        // Runs different types of hit scans according to the attack type
        switch (type)
        {
            // Attacks twice
            case AttackType.AirAttack:
                // First slash
                yield return new WaitForSeconds(maxScanTime);
                // Simulates double slash by inserting a break
                currentHitbox.SetActive(false);
                yield return new WaitForSeconds(0.1f);
                // Second slash
                currentHitbox.SetActive(true);
                yield return new WaitForSeconds(maxScanTime);
                break;
            // Has three different type of attacks
            case AttackType.AirHeavyAttack:
                // Swing
                yield return new WaitForSeconds(maxScanTime);
                currentHitbox.SetActive(false);
                // Change hitbox to airheavy_fall
                currentHitbox = airHeavyAttackHitbox[1];
                currentHitbox.SetActive(true);
                while (!isGrounded)
                {
                    yield return null;
                }
                currentHitbox.SetActive(false);
                // Change to airheavy_hit when hitting the ground
                maxScanTime = 0.3f;
                currentHitbox = airHeavyAttackHitbox[2];
                currentHitbox.SetActive(true);
                yield return new WaitForSeconds(maxScanTime);
                break;
            // Ground Combo, Crouch, Skill -> Basic hit scan
            default:
                // Perform hit scan for a given time
                yield return new WaitForSeconds(maxScanTime);
                break;
        }

        currentHitbox.SetActive(false);
        isAttacking = false;

        yield return null;
    }

    public void Damaged(float damage, Entity entity, bool isStun = false)
    {
        if (isInvincible)
        {
            Debug.Log("Damage Nullified: invincible");
            return;
        }

        int topPassiveLevel_beginner = PlayerDataManager.Instance.GetTopPassive(3);

        // Top Passive Beginner Skill: Shield Regeneration
        if (topPassiveLevel_beginner >= 1 && !shieldRegenCountdown)
        {
            Debug.Log("Damage Nullified: shielded");
            StartCoroutine(ShieldRegeneration(topPassiveLevel_beginner));
            return;
        }

        int topPassiveLevel_balanced = PlayerDataManager.Instance.GetTopPassive(2);
        // Top Passive Balanced Skill: Danage reflection
        if (topPassiveLevel_balanced >= 1)
        {
            float reflectDamage = PlayerDataManager.Instance.GetDamage();
            switch (topPassiveLevel_balanced)
            {
                case 1:
                    reflectDamage *= 1f;
                    break;
                case 2:
                    reflectDamage *= 1.5f;
                    break;
                case 3:
                    reflectDamage *= 3f;
                    break;
                default:
                    reflectDamage = 0;
                    if (topPassiveLevel_balanced != 0)
                    {
                        Debug.LogError("Invalid balanced skill level for damage reflection: " + topPassiveLevel_balanced);
                    }
                    break;
            }


            //// Reflect damage to enemy who attacked me (given by position)
            //Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, 0.1f);
            //foreach (var hitCollider in hitColliders)
            //{
            //    Entity enemy = hitCollider.GetComponent<Entity>();
            //    if (enemy != null)
            //    {
            //        enemy.Damaged(reflectDamage, transform.position);
            //        Debug.Log("Reflected Damage to Enemy at position: " + enemy.transform.position);
            //    }
            //}
        }

        int takenDamage = 0 - (int)damage;
        PlayerDataManager.Instance.SetHp(takenDamage);
        HUD.Instance.UpdateHUD();

        int playerHealth = PlayerDataManager.Instance.GetHp();
        Debug.Log("Damage took: " + damage + ", Remaining Health: " + playerHealth);

        if (playerHealth <= 0)
        {
            StartCoroutine(Dead());
        }
        else
        {
            StartCoroutine(Hurt());
        }

        //attackerPosition = position;
        isStunned = isStun;
    }

    private bool IsHighEnoughForAirHeavyAttack()
    {
        return !Physics2D.Raycast(cachedTransform.position, Vector2.down, minHeightForAirHeavyAttack, groundLayerMask);
    }

    private bool IsTouchingWall()
    {
        // Shoot five rays in the facing direction to detect walls
        float direction = playerSpriteRenderer.flipX ? -1f : 1f;
        // Adjust as needed
        float rayLength = 0.8f;
        int wallLayerMask = LayerMask.GetMask("Ground");

        // Get collider bounds
        Bounds bounds = GetComponent<Collider2D>().bounds;
        Vector2 topmost = new Vector2(bounds.center.x, bounds.max.y - 0.05f);
        Vector2 top = new Vector2(bounds.center.x, bounds.max.y - 0.5f);
        Vector2 middle = new Vector2(bounds.center.x, bounds.center.y);
        Vector2 bottom = new Vector2(bounds.center.x, bounds.min.y + 0.5f);
        Vector2 bottommost = new Vector2(bounds.center.x, bounds.min.y + 0.05f);

        Vector2 rayDir = new Vector2(direction, 0);

        RaycastHit2D hitTopmost = Physics2D.Raycast(topmost, rayDir, rayLength, wallLayerMask);
        RaycastHit2D hitTop = Physics2D.Raycast(top, rayDir, rayLength, wallLayerMask);
        RaycastHit2D hitMiddle = Physics2D.Raycast(middle, rayDir, rayLength, wallLayerMask);
        RaycastHit2D hitBottom = Physics2D.Raycast(bottom, rayDir, rayLength, wallLayerMask);
        RaycastHit2D hitBottommost = Physics2D.Raycast(bottommost, rayDir, rayLength, wallLayerMask);

        Debug.DrawRay(topmost, rayDir * rayLength, Color.red);
        Debug.DrawRay(top, rayDir * rayLength, Color.red);
        Debug.DrawRay(middle, rayDir * rayLength, Color.red);
        Debug.DrawRay(bottom, rayDir * rayLength, Color.red);
        Debug.DrawRay(bottommost, rayDir * rayLength, Color.red);

        return (hitTopmost.collider != null || hitTop.collider != null || hitMiddle.collider != null || hitBottom.collider != null || hitBottommost.collider != null);
    }

    private void CheckGrounded()
    {
        // Don't check during special states
        if (IsAirHeavyAttackState(currentState) || isAttacking)
            return;

        // Cast two rays from the bottom edges of the player
        Vector2 centerPosition = transform.position;

        // Cast rays from bottom edges of the player
        // Slightly above the bottom to avoid ground clipping
        float verticalOffset = 0.7f;
        Vector2 rayStartLeft = centerPosition + (Vector2.left * groundCheckWidth * 0.5f);
        Vector2 rayStartRight = centerPosition + (Vector2.right * groundCheckWidth * 0.5f);
        rayStartLeft.y -= verticalOffset;
        rayStartRight.y -= verticalOffset;

        // Check both bottom corners
        RaycastHit2D hitLeft = Physics2D.Raycast(rayStartLeft, Vector2.down, groundCheckDistance, groundLayerMask);
        RaycastHit2D hitRight = Physics2D.Raycast(rayStartRight, Vector2.down, groundCheckDistance, groundLayerMask);

        // Debug rays
        Vector2 leftVector = rayStartLeft;
        Vector2 rightVector = rayStartRight;
        leftVector.y -= groundCheckDistance;
        rightVector.y -= groundCheckDistance;

        if (hitLeft.collider && hitRight.collider)
        {
            Debug.DrawLine(rayStartLeft, leftVector, Color.green);
            Debug.DrawLine(rayStartRight, rightVector, Color.green);
        }
        else if (hitRight.collider)
        {
            Debug.DrawLine(rayStartLeft, leftVector, Color.red);
            Debug.DrawLine(rayStartRight, rightVector, Color.green);
        }
        else if (hitLeft.collider)
        {
            Debug.DrawLine(rayStartLeft, leftVector, Color.green);
            Debug.DrawLine(rayStartRight, rightVector, Color.red);
        }
        else
        {
            Debug.DrawLine(rayStartLeft, leftVector, Color.red);
            Debug.DrawLine(rayStartRight, rightVector, Color.red);
        }

        // Update grounded state
        isGrounded = (hitLeft.collider || hitRight.collider) && (rigid.linearVelocity.y > -0.01f && rigid.linearVelocity.y < 0.01f);

        // Handle state transitions
        if (isGrounded && currentState == PlayerState.Falling)
        {
            Debug.Log("if (isGrounded && currentState == PlayerState.Falling)");
            ChangeState(PlayerState.Idle);
        }
        else if (!isGrounded && !IsAirborneState(currentState))
        {
            Debug.Log("else if (!isGrounded && !IsAirborneState(currentState))");
            ChangeState(PlayerState.Falling);
        }

        animator.SetBool("isGrounded", isGrounded);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
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