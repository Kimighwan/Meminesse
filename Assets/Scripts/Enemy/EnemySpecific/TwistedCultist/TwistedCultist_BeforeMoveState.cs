using UnityEngine;

public class TwistedCultist_BeforeMoveState : MoveState
{
    private TwistedCultist enemy;
    public TwistedCultist_BeforeMoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, TwistedCultist enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (isDetectWall || !isDetectLedge)
        {
            enemy.beforeIdleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemy.beforeIdleState);
        }
        else if (isPlayerInRangeAttackRange)
            stateMachine.ChangeState(enemy.transitionState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
