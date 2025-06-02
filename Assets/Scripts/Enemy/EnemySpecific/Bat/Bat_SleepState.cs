using UnityEngine;

public class Bat_SleepState : SleepState
{
    private Bat enemy;

    public Bat_SleepState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Bat enemy) : base(entity, stateMachine, animBoolName)
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

        entity.rigid.gravityScale = 0f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        if (isPlayerDetect)
            stateMachine.ChangeState(enemy.idleState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
