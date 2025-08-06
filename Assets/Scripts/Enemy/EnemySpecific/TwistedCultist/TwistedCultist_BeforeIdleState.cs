using UnityEngine;

public class TwistedCultist_BeforeIdleState : IdleState
{
    private TwistedCultist enemy;
    public TwistedCultist_BeforeIdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, TwistedCultist enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        if(isPlayerInRangeAttackRange)
            stateMachine.ChangeState(enemy.transitionState);
        else if (isIdleTimeOver)
            stateMachine.ChangeState(enemy.beforeMoveState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
