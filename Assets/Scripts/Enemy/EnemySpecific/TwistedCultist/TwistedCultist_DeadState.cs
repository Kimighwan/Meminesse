using UnityEngine;

public class TwistedCultist_DeadState : DeadState
{
    private TwistedCultist enemy;
    public TwistedCultist_DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData, TwistedCultist enemy) : base(entity, stateMachine, animBoolName, stateData)
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
