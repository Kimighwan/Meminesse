using UnityEngine;

public class AttackState : State
{
    protected Transform attackPosition;

    protected bool isAttackAnimationFinished;
    protected bool isPlayerInMeleeAttackRange;

    public AttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition) : base(entity, stateMachine, animBoolName)
    {
        this.attackPosition = attackPosition;
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isPlayerInMeleeAttackRange = entity.CheckPlayerInMeleeAttackRange();
    }

    public override void Enter()
    {
        base.Enter();

        entity.animationToStatemachine.attackState = this;
        isAttackAnimationFinished = false;
        entity.SetVelocity(0f);
    } 

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public virtual void TriggerAttack()
    {

    }

    public virtual void FinishAttack()
    {
        isAttackAnimationFinished = true;
    }
}
