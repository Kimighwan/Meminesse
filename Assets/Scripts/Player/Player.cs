using UnityEngine;
using System.Collections;
using UnityEditor.Experimental.GraphView;

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


    private bool canDash;
    private bool canAttack;
    private bool lockStateChange;


    private bool isDashing;
    private bool isBackDashing;
    private bool isJumping;
    private bool isGrounded;
    private bool isAttacking;


    private int comboCounter;


    // Used to flip sprites
    private float previousDirection = 1f;
    // Combo count for grounded attacks
    private float maxGroundedComboAttack = 3;


    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator animator;


    private PlayerStateMachine stateMachine;

    #endregion

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        stateMachine = GetComponent<PlayerStateMachine>();

        rigid.freezeRotation = true;
        rigid.interpolation = RigidbodyInterpolation2D.Interpolate;
        rigid.linearDamping = 0;

        isGrounded = true;
        canDash = true;
        canAttack = true;

        lockStateChange = false;

        comboCounter = 0;
    }

    private void Update()
    {
        HandleState();
        MoveCharacter();
    }

    private void HandleState()
    {
        if (!lockStateChange)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");

            // Priorities
            // Dashing/Backdashing > GroundAttack > Jumping > Falling > Running > Idle
            // Dashing
            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                canAttack = false;
                stateMachine.ChangeState(PlayerState.Dashing);
                StartCoroutine(DashCharacter());
            }
            // Backdashing
            else if (Input.GetKeyDown(KeyCode.LeftControl) && canDash && isGrounded)
            {
                canAttack = false;
                stateMachine.ChangeState(PlayerState.BackDashing);
                StartCoroutine(BackDashCharacter());
            }
            // Ground Attack -> Can be cancelled by dashing but not vice versa
            else if (Input.GetKeyDown(KeyCode.Z) && isGrounded && canAttack)
            {
                lockStateChange = true;
                stateMachine.ChangeState(PlayerState.Attacking);
                StartCoroutine(GroundedComboAttack());
            }
            // Jumping
            else if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                stateMachine.ChangeState(PlayerState.Jumping);
                StartCoroutine(JumpCharacter());
            }
            // Falling
            else if (!isGrounded && rigid.linearVelocity.y <= 0)
            {
                stateMachine.ChangeState(PlayerState.Falling);
            }
            // Running
            else if ((moveInput != 0) && !(isDashing || isBackDashing))
            {
                if (Mathf.Sign(moveInput) != previousDirection)
                {
                    // Turn around
                    spriteRenderer.flipX = !spriteRenderer.flipX; 
                    previousDirection = Mathf.Sign(moveInput);
                }
                else
                {
                    stateMachine.ChangeState(PlayerState.Running);
                }
            }
            // Idle
            else if (rigid.linearVelocityX == 0 && rigid.linearVelocity.y == 0)
            {
                stateMachine.ChangeState(PlayerState.Idle);
            }

            animator.SetBool("isGrounded", isGrounded);
        }
    }

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
            rigid.linearVelocity = new Vector2(dashDirection * dashSpeed, rigid.linearVelocityY);
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
            rigid.linearVelocity = new Vector2(dashDirection * backDashSpeed, rigid.linearVelocityY);
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
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput != 0)
        {
            rigid.linearVelocity = new Vector2(moveInput * moveSpeed, rigid.linearVelocity.y);
        }
        else
        {
            rigid.linearVelocity = new Vector2(0, rigid.linearVelocity.y);
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
        rigid.linearVelocity = new Vector2(rigid.linearVelocityX, currentJumpForce);

        // Continue applying force when jump button is held
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
        GroundedComboAttack3
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
                stateMachine.ChangeState(PlayerState.GroundedComboAttack1);
                StartCoroutine(Attack(AttackType.GroundedComboAttack1));
                break;
            case 2:
                stateMachine.ChangeState(PlayerState.GroundedComboAttack2);
                StartCoroutine(Attack(AttackType.GroundedComboAttack2));
                break;
            case 3:
                stateMachine.ChangeState(PlayerState.GroundedComboAttack3);
                StartCoroutine(Attack(AttackType.GroundedComboAttack3));
                break;
            default:
                break;
        }

        // Wait before allowing next input
        yield return new WaitForSeconds(attackCoolDown);

        while (currentTime < startingTime + maxAttackInterval)
        {
            currentTime = Time.time;

            // Next input
            if (Input.GetKeyDown(KeyCode.Z) && comboCounter < maxGroundedComboAttack)
            {
                // Continue combo
                StartCoroutine(GroundedComboAttack());
                reset = false;
            }

            yield return null;
        }

        if (reset)
        {
            Debug.Log("Combo Reset");
            comboCounter = 0;
            lockStateChange = false;
        }

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
            default:
                Debug.Log("Attack type?: " + type);
                break;
        }

        yield return null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && rigid.linearVelocity.y <= 0)
        {
            isGrounded = true;
            animator.SetBool("isGrounded", isGrounded);
            stateMachine.ChangeState(PlayerState.Idle);
        }
    }

    #endregion
}