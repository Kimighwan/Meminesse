using NUnit.Framework.Interfaces;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class NomalCultist : Entity
{
    public NomalCultist_IdleState idleState { get; private set; }
    public NomalCultist_MoveState moveState { get; private set; }
    public NomalCultist_DetectState detectState { get; private set; }
    public NomalCultist_RangeAttackState rangeAttackState { get; private set; }
    public NomalCultist_DamagedState damagedState { get; private set; }
    public NomalCultist_StunState stunState { get; private set; }
    public NomalCultist_DeadState deadState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_DetectState detectStateData;
    [SerializeField] 
    private D_RangeAttackState rangeAttackStateData;
    [SerializeField]
    private D_DamagedState damagedStateData;
    [SerializeField]
    private D_StunState sunStateData;
    [SerializeField]
    private D_DeadState deadStateData;

    [SerializeField]
    private Transform rangeAttackPos;

    public float AttackCoolTime { get; set; } = 2f;
    public float LastAttackTime { get; set; }

    public override void Start()
    {
        base.Start();

        facingDirection = -1;
        defaultDirection = -1;

        idleState = new NomalCultist_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new NomalCultist_MoveState(this, stateMachine, "move", moveStateData, this);
        detectState = new NomalCultist_DetectState(this, stateMachine, "move", detectStateData, this);
        rangeAttackState = new NomalCultist_RangeAttackState(this, stateMachine, "rangeAttack", rangeAttackPos, rangeAttackStateData, this);
        damagedState = new NomalCultist_DamagedState(this, stateMachine, "damaged", damagedStateData, this);
        stunState = new NomalCultist_StunState(this, stateMachine, "stun", sunStateData, this);
        deadState = new NomalCultist_DeadState(this, stateMachine, "dead", deadStateData, this);

        stateMachine.Init(idleState);
    }

    public override void Damaged(float damage, Vector2 position, bool isStun = false, float defIgnore = 0f)
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
