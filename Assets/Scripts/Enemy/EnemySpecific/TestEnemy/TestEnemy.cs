using NUnit.Framework.Interfaces;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class TestEnemy : Entity
{
    public TestEnemy_IdleState idleState { get; private set; }
    public TestEnemy_MoveState moveState { get; private set; }
    public TestEnemy_ChargeState chargeState { get; private set; }
    public TestEnemy_MeleeAttackState meleeAttackState { get; private set; }
    public TestEnemy_DetectState detectState { get; private set; }
    public TestEnemy_StunState stunState { get; private set; }
    public TestEnemy_DeadState deadState { get; private set; }
    public TestEnemy_DamagedState damagedState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_ChargeState chargeStateData;
    [SerializeField]
    private D_MeleeAttackState meleeAttackStateData;
    [SerializeField]
    private D_DetectState detectStateData;
    [SerializeField]
    private D_StunState stunStateData;
    [SerializeField]
    private D_DeadState deadStateData;
    [SerializeField]
    private D_DamagedState dmagedStateData;


    [SerializeField]
    private Transform meleeAttackPosition;

    public override void Start()
    {
        base.Start();

        idleState = new TestEnemy_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new TestEnemy_MoveState(this, stateMachine, "move", moveStateData, this);
        chargeState = new TestEnemy_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        meleeAttackState = new TestEnemy_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        detectState = new TestEnemy_DetectState(this, stateMachine, "detect", detectStateData, this);
        stunState = new TestEnemy_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new TestEnemy_DeadState(this, stateMachine, "dead", deadStateData, this);
        damagedState = new TestEnemy_DamagedState(this, stateMachine, "damaged", dmagedStateData, this);

        stateMachine.Init(idleState);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // 근접 공격 범위 표시
        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }

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
