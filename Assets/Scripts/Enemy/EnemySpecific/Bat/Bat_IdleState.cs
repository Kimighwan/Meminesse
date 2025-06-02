using UnityEngine;

public class Bat_IdleState : IdleState
{
    private Bat enemy;

    public Bat_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Bat enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        if(!isPlayerInRangeAttackRange)
            stateMachine.ChangeState(enemy.sleepState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
