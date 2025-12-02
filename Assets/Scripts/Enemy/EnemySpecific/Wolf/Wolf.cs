using UnityEngine;

public class Wolf : Entity
{
    public Wolf_IdleState idleState { get; private set; }
    public Wolf_MoveState moveState { get; private set; }
    public Wolf_DetectState detectState { get; private set; }
    public Wolf_ChargeState chargeState { get; private set; }
    public Wolf_MeleeAttackState meleeAttackState { get; private set; }
    public Wolf_DamagedState damagedState { get; private set; }
    public Wolf_DeadState deadState { get; private set; }
    public Wolf_StunState stunState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_DetectState detectStateData;
    [SerializeField]
    private D_ChargeState chargeStateData;
    [SerializeField]
    private D_MeleeAttackState meleeAttackStateData;
    [SerializeField]
    private D_DamagedState dmagedStateData;
    [SerializeField]
    private D_DeadState deadStateData;
    [SerializeField]
    private D_StunState stunStateData;

    [SerializeField]
    private Transform meleeAttackPosition;

    public float ChargeCoolTime { get; set; } = 3f;
    public float LastAttackTime { get; set; }
    public float LastChargeTime { get; set; }

    public override void Start()
    {
        base.Start();

        facingDirection = -1;
        defaultDirection = -1;

        idleState = new Wolf_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new Wolf_MoveState(this, stateMachine, "move", moveStateData, this);
        detectState = new Wolf_DetectState(this, stateMachine, "detect", detectStateData, this);
        chargeState = new Wolf_ChargeState(this, stateMachine, "detect", chargeStateData, this);
        meleeAttackState = new Wolf_MeleeAttackState(this, stateMachine, "attack", meleeAttackPosition, meleeAttackStateData, this);
        damagedState = new Wolf_DamagedState(this, stateMachine, "damaged", dmagedStateData, this);
        deadState = new Wolf_DeadState(this, stateMachine, "dead", deadStateData, this);
        stunState = new Wolf_StunState(this, stateMachine, "stun", stunStateData, this);

        stateMachine.Init(idleState);
    }

#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // 근접 공격 범위 표시
        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
#endif

    public override void Damaged(float damage, Vector2 position, bool isStun, float defIgnore = 0f)
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

    public void OnClickDamaged()
    {
        Damaged(10, new Vector2(0, 0), false);
    }

    public void OnClickStun()
    {
        Damaged(5, new Vector2(0, 0), true);
    }

}
