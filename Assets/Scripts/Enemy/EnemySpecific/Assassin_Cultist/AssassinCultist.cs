using NUnit.Framework.Interfaces;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class AssassinCultist : Entity
{
    public AssassinCultist_IdleState idleState { get; private set; }
    public AssassinCultist_HidingState hidingState { get; private set; }
    public AssassinCultist_CommingState commingState { get; private set; }
    public AssassinCultist_MoveState moveState { get; private set; }
    public AssassinCultist_DetectState detectState { get; private set; }
    public AssassinCultist_MeleeAttackState meleeAttackState { get; private set; }
    public AssassinCultist_HidingAttackState hidingAttackState { get; private set; }
    public AssassinCultist_DamagedState damagedState { get; private set; }
    public AssassinCultist_StunState stunState { get; private set; }
    public AssassinCultist_DeadState deadState { get; private set; }

    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_IdleState hidingStateData;
    [SerializeField] private D_IdleState commingStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_DetectState detectStateData;
    [SerializeField] private D_MeleeAttackState meleeAttackStateData;
    [SerializeField] private D_MeleeAttackState hidingAttackStateData;
    [SerializeField] private D_DamagedState damagedStateData;
    [SerializeField] private D_StunState stunStateData;
    [SerializeField] private D_DeadState deadStateData;

    [SerializeField] private Transform meleeAttackPos;
    [SerializeField] private Transform hidingAttackPos;

    public float AttackCoolTime { get; private set; } = 4f;
    public float LastAttackTime { get; set; }

    public bool isAttackFinish { get; set; }

    public override void Start()
    {
        base.Start();

        facingDirection = -1;
        defaultDirection = -1;
        isAttackFinish = false;

        idleState = new AssassinCultist_IdleState(this, stateMachine, "idle", idleStateData, this);
        hidingState = new AssassinCultist_HidingState(this, stateMachine, "hiding", hidingStateData, this);
        commingState = new AssassinCultist_CommingState(this, stateMachine, "comming", commingStateData, this);
        moveState = new AssassinCultist_MoveState(this, stateMachine, "move", moveStateData, this);
        detectState = new AssassinCultist_DetectState(this, stateMachine, "detect", detectStateData, this);
        meleeAttackState = new AssassinCultist_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPos, meleeAttackStateData, this);
        hidingAttackState = new AssassinCultist_HidingAttackState(this, stateMachine, "hidingAttack", hidingAttackPos, hidingAttackStateData, this);
        damagedState = new AssassinCultist_DamagedState(this, stateMachine, "damaged", damagedStateData, this);
        stunState = new AssassinCultist_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new AssassinCultist_DeadState(this, stateMachine, "dead", deadStateData, this);

        stateMachine.Init(idleState);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // 근접 공격 범위 표시
        Gizmos.DrawWireSphere(meleeAttackPos.position, meleeAttackStateData.attackRadius);
        Gizmos.DrawWireSphere(hidingAttackPos.position, hidingAttackStateData.attackRadius);
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
