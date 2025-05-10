using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class TestEnemy : Entity
{
    public TestEnemy_IdleState idleState { get; private set; }
    public TestEnemy_MoveState moveState { get; private set; }
    public TestEnemy_ChargeState chargeState { get; private set; }
    public TestEnemy_MeleeAttackState meleeAttackState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_ChargeState chargeStateData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;

    [SerializeField]
    private Transform meleeAttackPosition;

    public override void Start()
    {
        base.Start();

        idleState = new TestEnemy_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new TestEnemy_MoveState(this, stateMachine, "move", moveStateData, this);
        chargeState = new TestEnemy_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        meleeAttackState = new TestEnemy_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);

        stateMachine.Init(idleState);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
}
