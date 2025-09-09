using NUnit.Framework.Constraints;
using System.Xml;
using UnityEngine;

public class Mushroom : Entity
{
    public Mushroom_IdleState idleState { get; private set; }
    public Mushroom_MoveState moveState { get; private set; }
    public Mushroom_MeleeAttackState meleeAttackState { get; private set; }
    public Mushroom_DamagedState damagedState { get; private set; }
    public Mushroom_StunState stunState { get; private set; }
    public Mushroom_DeadState deadState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_MeleeAttackState meleeAttackStateData;
    [SerializeField]
    private D_DamagedState dmagedStateData;
    [SerializeField]
    private D_StunState stunStateData;
    [SerializeField]
    private D_DeadState deadStateData;

    [SerializeField]
    private Transform meleeAttackPosition;

    public override void Start()
    {
        base.Start();

        facingDirection = -1;
        defaultDirection = -1;

        idleState = new Mushroom_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new Mushroom_MoveState(this, stateMachine, "move", moveStateData, this);
        meleeAttackState = new Mushroom_MeleeAttackState(this, stateMachine, "attack", meleeAttackPosition, meleeAttackStateData, this);
        damagedState = new Mushroom_DamagedState(this, stateMachine, "damaged", dmagedStateData, this);
        stunState = new Mushroom_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new Mushroom_DeadState(this, stateMachine, "dead", deadStateData, this);


        stateMachine.Init(idleState);
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
