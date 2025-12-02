using NUnit.Framework.Interfaces;
using System.Xml;
using UnityEngine;

public class Slime : Entity
{
    public Slime_IdleState idleState { get; private set; }
    public Slime_MoveState moveState { get; private set; }
    public Slime_AttackState attackState { get; private set; }
    public Slime_DamagedState damagedState { get; private set; }
    public Slime_DeadState deadState { get; private set; }

    [SerializeField] D_IdleState idleStateData;
    [SerializeField] D_MoveState moveStateData;
    [SerializeField] D_MeleeAttackState meleeAttackStateData;
    [SerializeField] D_DamagedState damagedStateData;
    [SerializeField] D_DeadState deadStateData;

    [SerializeField]
    private Transform meleeAttackPos;

    public float LastAttackTime { get; set; }

    public override void Start()
    {
        base.Start();

        facingDirection = -1;
        idleState = new Slime_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new Slime_MoveState(this, stateMachine, "move", moveStateData, this);
        attackState = new Slime_AttackState(this, stateMachine, "attack", meleeAttackPos, meleeAttackStateData, this);
        damagedState = new Slime_DamagedState(this, stateMachine, "damaged", damagedStateData, this);
        deadState = new Slime_DeadState(this, stateMachine, "die", deadStateData, this);


        stateMachine.Init(moveState);
    }

    public override bool CheckPlayerInMeleeAttackRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right * -1, entityData.playerInMeleeAttackRange, entityData.whatIsPlayer);

    }

    public override void Damaged(float damage, Vector2 position, bool isStun = false, float defIgnore = 0f)
    {
        base.Damaged(damage, position, isStun);

        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }
        else
        {
            stateMachine.ChangeState(damagedState);
        }
    }

#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // 근접 공격 범위 표시
        Gizmos.DrawWireSphere(meleeAttackPos.position, meleeAttackStateData.attackRadius);
    }
#endif

    public void OnClickDamaged()
    {
        Damaged(10, new Vector2(0, 0), false);
    }

    public void OnClickStun()
    {
        Damaged(5, new Vector2(0, 0), true);
    }
}
