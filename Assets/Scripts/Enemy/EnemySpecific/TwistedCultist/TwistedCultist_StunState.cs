using UnityEngine;

public class TwistedCultist_StunState : StunState
{
    private TwistedCultist enemy;
    public TwistedCultist_StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData, TwistedCultist enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (isStunTimeOver)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
