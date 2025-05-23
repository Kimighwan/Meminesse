using UnityEngine;

public class Archer_MoveState : MoveState
{
    private Archer enemy;

    public Archer_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, Archer enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        // TODO : transition to attackState and dodgeState
        if(isPlayerInMeleeAttackRange)
        {
            enemy.stateMachine.ChangeState(enemy.detectState);
        }
        else if (isDetectWall || !isDetectLedge)
        {
            enemy.idleState.SetFlipAfterIdle(true);
            enemy.stateMachine.ChangeState(enemy.idleState);
        }
        else if (isPlayerInRangeAttackRange)
        {
            stateMachine.ChangeState(enemy.rangeAttackState);
        }
    }      

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
