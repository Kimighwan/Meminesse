using UnityEngine;
using System.Collections;
using UnityEditor.Experimental.GraphView;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    // To implement variable jump height
    [SerializeField] float maxJumpHoldTime;
    [SerializeField] float baseJumpForce;
    [SerializeField] float holdJumpForce;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;
    [SerializeField] float dashCooldown;

    private bool isDashing;
    private bool canDash;
    private bool isJumping;
    private bool isGrounded;

    // Used to flip sprites
    private float previousDirection = 1f;

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private PlayerStateMachine stateMachine;

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
    }

    private void Update()
    {
        HandleState();
        MoveCharacter();
    }

    private void HandleState()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        // Priorities
        // Dashing > Jumping > Falling > Running > Idle
        // Jumping
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            stateMachine.ChangeState(PlayerState.Dashing);
            StartCoroutine(DashCharacter());
        }
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
        else if (moveInput != 0)
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
            rigid.linearVelocity += Vector2.up * holdJumpForce * Time.deltaTime;
            jumpTimer += Time.deltaTime;

            // Reduce jump force over time to create a smooth curve
            currentJumpForce = Mathf.Max(currentJumpForce - (jumpAcceleration * Time.deltaTime), 0);

            yield return null;
        }

        isJumping = false;
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
}