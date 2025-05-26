using UnityEngine;

public class Archer_IdleState : IdleState
{
    private Archer enemy;
    public Archer_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Archer enemy) : base(entity, stateMachine, animBoolName, stateData)
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

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        // TODO : transition to dodgeState and attackState
        if (isPlayerInMeleeAttackRange)
        {
            stateMachine.ChangeState(enemy.detectState);
        }
        else if (isPlayerInRangeAttackRange && entity.CanRangeAttackPlayer())
        {
            enemy.idleState.SetFlipAfterIdle(false);
            stateMachine.ChangeState(enemy.rangeAttackState);
        }
        else if (isIdleTimeOver)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
