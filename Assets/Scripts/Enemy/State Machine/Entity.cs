using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;

    public D_Entity entityData;


    public int facingDirection { get; protected set; }
    public Animator anim { get; private set; }
    public Rigidbody2D rigid { get; private set; }
    public AnimationToStatemachine animationToStatemachine { get; private set; }
    public int LastDamagedDirection { get; private set; }
    public bool IsStun { get; private set; }        // 스턴 상태에 돌입이 가능한가?
    protected void SetIsStun(bool value) { IsStun = value; }

    //public GameObject aliveGO { get; private set; }

    [SerializeField]
    private Transform wallCheck;    // 벽체크
    [SerializeField]
    private Transform ledgeCheck;   // 낭떨어지 체크
    [SerializeField]
    protected Transform playerCheck;  // 플레이어 체크
    [SerializeField]
    private Transform groundCheck;
    private Vector2 entityVelocity;

    private float currentHp;

    private RaycastHit2D hit;
    private Transform playerTF;

    protected int defaultDirection;

    protected float lastStunTime;

    protected bool isDead;

    public virtual void Start()
    {
        facingDirection = 1; // 기본 Entity의 방향이 오른쪽임
        defaultDirection = 1; // 스프라이트 기본 방향 // 오른쪽
        currentHp = entityData.maxHp;
        IsStun = true;
        isDead = false;

        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        animationToStatemachine = GetComponent<AnimationToStatemachine>();

        stateMachine = new FiniteStateMachine();
    }

    public virtual void Update()
    {
        stateMachine.currentState.LogicalUpdate();

        if (!IsStun)    // 스턴 불가능시 다시 회복하기 위함
        {
            if(Time.time >= lastStunTime + entityData.stunRecoveryTIme)
            {
                IsStun = true;
            }
        }
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual void SetVelocityX(float velocity)     // 속도 설정
    {
        entityVelocity = new Vector2(facingDirection * velocity, rigid.linearVelocityY);
        rigid.linearVelocity = entityVelocity;
    }

    public virtual void SetVelocityY(float velocity)     // 속도 설정
    {
        entityVelocity = new Vector2(rigid.linearVelocityX, facingDirection * velocity);
        rigid.linearVelocity = entityVelocity;
    }

    public virtual void SetVelocity(float velocity, Vector2 angle, int direction)     // 속도 설정 및 방향 설정
    {
        angle.Normalize();
        entityVelocity = new Vector2(angle.x * direction * velocity, angle.y * velocity);
        rigid.linearVelocity = entityVelocity;
    }

    public Vector2 GetEnemyPosition()
    {
        return transform.position;
    }

    public Vector2 GetDirectionToPlayer()
    {
        playerTF = PlayerTransformForRangeAttack();
        return (new Vector2(playerTF.position.x, playerTF.position.y) - GetEnemyPosition());
    }

    public virtual void Flip()                          // 방향 뒤집기
    {
        facingDirection *= -1;
        transform.Rotate(0f, 180f, 0f);
    }

    public virtual void CheckXPositionForFlip()         // Checking X position between Player and Enemy and Flip
    {
        float playerX = PlayerTransformForRangeAttack().position.x;

        if (playerX > transform.position.x && facingDirection == -1)
            Flip();
        else if(playerX < transform.position.x && facingDirection == 1)
            Flip();
    }

    public virtual void Knockback(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        rigid.linearVelocity = new Vector2(velocity * angle.x * direction, angle.y * direction);
    }

    /// <summary>
    /// position는 공격 위치 즉 player의 transform.position을 전달
    /// isStun은 스턴 공격인지 확인하는 변수, 스턴 공격이라면 true를 전달
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="position"></param>
    /// <param name="isStun"></param>
    public virtual void Damaged(float damage, Vector2 position, bool isStun = false)
    {
        currentHp -= damage;

        Knockback(entityData.knockbackSpeed, entityData.knockbackAngle, LastDamagedDirection);

        // 데미지 입을 때 생성될 파티클 인스턴스화

        if (position.x > transform.position.x)   // 플레이어가 오른쪽에 있음
        {
            LastDamagedDirection = -1;
        }
        else
        {
            LastDamagedDirection = 1;
        }

        if (currentHp <= 0) isDead = true;
    }

    #region Player Dectected
    // 현재는 모든 몬스터가 어느 한 위치에서 일직선으로 탐지 한다(2025-05-01)
    // 몬스터 종류가 많아지면서 탐지 방법이 달라진다면 변경하기

    public virtual bool CheckPlayerInMeleeAttackRange() // 플레이어가 몬스터의 근접 공격 범위에서 탐지되는지
    {
        return Physics2D.Raycast(playerCheck.position, transform.right * defaultDirection, entityData.playerInMeleeAttackRange, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInRangeAttackRange() // 플레이어가 몬스터의 원거리 공격 범위에서 탐지되는지
    {
        if(CanRangeAttackPlayer())
            return Physics2D.OverlapCircle(playerCheck.position, entityData.playerInRangeAttackRadius, entityData.whatIsPlayer);
        else return false;
        //return Physics2D.Raycast(playerCheck.position, transform.right, entityData.playerInRangeAttackRange, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInChargeRange()     // 플레이어가 몬스터의 돌진 패턴 범위에서 탐지되는지
    {
        return Physics2D.OverlapCircle(playerCheck.position, entityData.playerInChargeRadius, entityData.whatIsPlayer);
        //return Physics2D.Raycast(playerCheck.position, transform.right, entityData.playerInChargeRange, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInDetectRangeTpyeLine()      // Checking playeris detected at the detectRange
    {
        hit = Physics2D.Raycast(playerCheck.position, transform.right * defaultDirection, entityData.playerDetectRange, ~(1<<8));
        
        if (hit &&  hit.collider.name == "TempPlayer")
            return true;
        else 
            return false;
    }

    public virtual bool CheckPlayerInDetectRangeTpyeCircle()
    {
        return Physics2D.OverlapCircle(playerCheck.position, entityData.playerDetectRange, entityData.whatIsPlayer);
    }

    public Transform PlayerTransformForRangeAttack()    // Getting player 'transform' when in rangeAttackRadius
    {
        Collider2D collider2D = Physics2D.OverlapCircle(playerCheck.position, entityData.playerInRangeAttackRadius, entityData.whatIsPlayer);
        return collider2D ? collider2D.transform : transform;
    }

    public bool CanDetectPlayer()                 // Checking obstacles for Player detect           
    {
        Vector3 dirV = PlayerTransformForRangeAttack().position - transform.position;
        RaycastHit2D hitCheck = Physics2D.Raycast(playerCheck.position, dirV, entityData.playerDetectRange, ~(1 << 8));
        Debug.DrawRay(playerCheck.position, dirV * entityData.playerDetectRange, Color.red);

        if (hitCheck && hitCheck.collider.name == "TempPlayer")
            return true;
        else
            return false;
    }

    private bool CanRangeAttackPlayer()                 // Checking obstacles for rangeAttack           
    {
        Vector3 dirV = PlayerTransformForRangeAttack().position - transform.position;
        RaycastHit2D hitCheck = Physics2D.Raycast(playerCheck.position, dirV, entityData.playerInRangeAttackRadius, ~(1 << 8));
        Debug.DrawRay(playerCheck.position, dirV * entityData.playerDetectRange, Color.green);

        if (hitCheck && hitCheck.collider.name == "TempPlayer")
            return true;
        else
            return false;
    }

    #endregion

    #region Check

    public virtual bool CheckWall()
    {
        //return Physics2D.OverlapCircle(wallCheck.position, 0.14f, entityData.whatIsPlatform);
        return Physics2D.Raycast(wallCheck.position, transform.right, entityData.wallCheckDistance, entityData.whatIsPlatform);
    }

    public virtual bool CheckLedge()
    {
        return Physics2D.OverlapCircle(ledgeCheck.position, 0.14f, entityData.whatIsPlatform);
    }

    public virtual bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, entityData.groundCheckRadius, entityData.whatIsPlatform);
    }

    #endregion

    public virtual void OnDrawGizmos()
    {
        // 벽 체크 표시
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance));
        //Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDistance));
        //Gizmos.DrawWireSphere(wallCheck.position, 0.14f);

        // 땅 체크 표시
        //Gizmos.DrawWireSphere(ledgeCheck.position, 0.14f);

        // 플레이어 탐지 거리 표시
        Gizmos.DrawWireSphere(transform.position, entityData.playerDetectRange);

        // 근접 공격 발동될 조건 거리 표시
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.playerInMeleeAttackRange), 0.14f);

        // 돌진 거리 표시
        Gizmos.DrawWireSphere(playerCheck.position, entityData.playerInChargeRadius);

        // 원거리 공격 범위 표시
        Gizmos.DrawWireSphere(playerCheck.position, entityData.playerInRangeAttackRadius);        
    }
}