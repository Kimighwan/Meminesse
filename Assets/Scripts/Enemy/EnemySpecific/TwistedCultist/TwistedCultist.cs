using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class TwistedCultist : Entity
{
    public TwistedCultist_BeforeIdleState beforeIdleState { get; private set; }
    public TwistedCultist_BeforeMoveState beforeMoveState { get; private set; }
    public TwistedCultist_TransitionState transitionState { get; private set; }
    public TwistedCultist_IdleState idleState { get; private set; }
    public TwistedCultist_MoveState moveState { get; private set; }
    public TwistedCultist_DetectState detectState { get; private set; }
    public TwistedCultist_MeleeAttackState meleeAttackState { get; private set; }
    public TwistedCultist_DamagedState damagedState { get; private set; }
    public TwistedCultist_StunState stunState { get; private set; }
    public TwistedCultist_DeadState deadState { get; private set; }

    [SerializeField] private D_IdleState beforeIdleStateData;
    [SerializeField] private D_MoveState beforeMoveStateData;
    [SerializeField] private D_IdleState transitionStateData;
    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_DetectState detectStateData;
    [SerializeField] private D_MeleeAttackState meleeAttackStateData;
    [SerializeField] private D_DamagedState damagedStateData;
    [SerializeField] private D_StunState stunStateData;
    [SerializeField] private D_DeadState deadStateData;

    [SerializeField] private Transform meleeAttackPosition;

    public float AttackCoolTime { get; private set; } = 2f;
    public float LastAttackTime { get; set; }

    public override void Start()
    {
        base.Start();

        facingDirection = -1;
        defaultDirection = -1;

        beforeIdleState = new TwistedCultist_BeforeIdleState(this, stateMachine, "beforeIdle", beforeIdleStateData, this);
        beforeMoveState = new TwistedCultist_BeforeMoveState(this, stateMachine, "beforeMove", beforeMoveStateData, this);
        transitionState = new TwistedCultist_TransitionState(this, stateMachine, "transition", transitionStateData, this);
        idleState = new TwistedCultist_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new TwistedCultist_MoveState(this, stateMachine, "move", moveStateData, this);
        detectState = new TwistedCultist_DetectState(this, stateMachine, "move", detectStateData, this);
        meleeAttackState = new TwistedCultist_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        damagedState = new TwistedCultist_DamagedState(this, stateMachine, "damaged", damagedStateData, this);
        stunState = new TwistedCultist_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new TwistedCultist_DeadState(this, stateMachine, "dead", deadStateData, this);

        stateMachine.Init(beforeIdleState);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // 근접 공격 범위 표시
        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }

    public override void Damaged(float damage, Vector2 position, bool isStun)
    {
        base.Damaged(damage, position, isStun);

        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }
        else if (isStun && IsStun && stateMachine.currentState != stunState)
        {
            // 스턴 공격이 들어오면 // IsStun는 스턴 상태 전환 가능한지 체크

            stateMachine.ChangeState(stunState);
            SetIsStun(false);   // 스턴 불가 상태로 설정
            lastStunTime = Time.time;
        }
        else
        {
            stateMachine.ChangeState(damagedState);
        }
    }
}
