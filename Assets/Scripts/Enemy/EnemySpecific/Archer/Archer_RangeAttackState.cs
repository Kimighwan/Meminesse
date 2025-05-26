using UnityEngine;

public class Archer_RangeAttackState : RangeAttackState
{
    private Archer enemy;

    public Archer_RangeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_RangeAttackState stateData, Archer enemy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        if(isAttackAnimationFinished)
        {
            if(isPlayerInMeleeAttackRange)
            {
                stateMachine.ChangeState(enemy.detectState);
            }
            else
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
    }
}
