using UnityEngine;

public class VoidbornGoddess_BigSpawnState : IdleState
{
    private VoidbornGoddess enemy;
    public VoidbornGoddess_BigSpawnState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, VoidbornGoddess enemy) : base(entity, stateMachine, animBoolName, stateData)
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

    public override void FinishTransition()
    {
        base.FinishTransition();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        if(isIdleTimeOver)
            stateMachine.ChangeState(enemy.cast2State);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
