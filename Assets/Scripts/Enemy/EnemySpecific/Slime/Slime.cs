using UnityEngine;

public class Slime : Entity
{
    public Slime_IdleState idleState { get; private set; }
    public Slime_MoveState moveState { get; private set; }
    public Slime_AttackState attackState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_MeleeAttackState meleeAttackStateData;

    [SerializeField]
    private Transform meleeAttackPos;

    public override void Start()
    {
        base.Start();

        facingDirection = -1;
        idleState = new Slime_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new Slime_MoveState(this, stateMachine, "move", moveStateData, this);
        attackState = new Slime_AttackState(this, stateMachine, "attack", meleeAttackPos, meleeAttackStateData, this); 

        stateMachine.Init(moveState);
    }

    public override bool CheckPlayerInMeleeAttackRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right * -1, entityData.playerInMeleeAttackRange, entityData.whatIsPlayer);

    }

    public override void Damaged(float damage, Vector2 position, bool isStun = false)
    {
        base.Damaged(damage, position, isStun);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // 근접 공격 범위 표시
        Gizmos.DrawWireSphere(meleeAttackPos.position, meleeAttackStateData.attackRadius);
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
