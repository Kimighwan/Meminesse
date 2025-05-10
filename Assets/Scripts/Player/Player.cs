using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float turnDuration;

    private bool isGrounded;
    private float previousDirection = 1f;

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private PlayerStateMachine stateMachine;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        stateMachine = GetComponent<PlayerStateMachine>();
    }

    private void Update()
    {
        HandleState();
        MoveCharacter();
    }

    private void HandleState()
    {
        float moveInput = Input.GetAxis("Horizontal");

        // Animation Priorities
        // Jumping > Falling > Running > Idle
        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            stateMachine.ChangeState(PlayerState.Jumping);
            JumpCharacter();
        }
        // Falling
        else if (!isGrounded && rigid.linearVelocity.y < 0)
        {
            stateMachine.ChangeState(PlayerState.Falling);
        }
        // Running
        else if (moveInput != 0)
        {
            if (Mathf.Sign(moveInput) != previousDirection)
            {
                StartCoroutine(ActivateTurningState()); // Temporary turning state
                previousDirection = Mathf.Sign(moveInput);
            }
            else
            {
                stateMachine.ChangeState(PlayerState.Running);
            }
        }
        // Idle
        else
        {
            stateMachine.ChangeState(PlayerState.Idle);
        }
    }

        private void MoveCharacter()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rigid.linearVelocity = new Vector2(moveInput * moveSpeed, rigid.linearVelocity.y);
    }

    private void JumpCharacter()
    {
        rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, jumpForce);
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            stateMachine.ChangeState(PlayerState.Idle);
        }
    }

    private IEnumerator ActivateTurningState()
    {
        stateMachine.ChangeState(PlayerState.Turning);
        // Flip sprite direction
        spriteRenderer.flipX = !spriteRenderer.flipX; 
        // Short turning animation duration
        yield return new WaitForSeconds(turnDuration); 
        stateMachine.ChangeState(PlayerState.Running);
    }

}