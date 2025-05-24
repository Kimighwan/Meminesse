using UnityEngine;

public class Archer_DodgeState : DodgeState
{
    private Archer enemy;

    public Archer_DodgeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DodgeState stateData, Archer enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (isDodgeTimeOver)
        {
            if (isPlayerInRangeAttackRange)
            {
                enemy.stateMachine.ChangeState(enemy.rangeAttackState);
            }
            else
            {
                enemy.stateMachine.ChangeState(enemy.moveState);
            }
        }
        else if (isDetectWall || !isDetectLedge)
        {
            enemy.stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
