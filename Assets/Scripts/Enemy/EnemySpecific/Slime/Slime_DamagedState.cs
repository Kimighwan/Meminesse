using UnityEngine;

public class Slime_DamagedState : DamagedState
{
    private Slime enemy;

    public Slime_DamagedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DamagedState stateData, Slime enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        if(isAnimationDone)
            stateMachine.ChangeState(enemy.idleState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
