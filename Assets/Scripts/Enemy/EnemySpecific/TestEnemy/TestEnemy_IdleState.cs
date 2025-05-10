using UnityEngine;

public class TestEnemy_IdleState : IdleState
{
    private TestEnemy enemy;

    public TestEnemy_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, TestEnemy enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (isIdleTimeOver)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
        else if (isPlayerInMeleeAttackRange)
        {
            stateMachine.ChangeState(enemy.meleeAttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
