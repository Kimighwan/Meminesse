using UnityEngine;

public class VoidbornGoddess_ChargeState : ChargeState
{
    private VoidbornGoddess enemy;
    public VoidbornGoddess_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData, VoidbornGoddess enemy) : base(entity, stateMachine, animBoolName, stateData)
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

    public override void FinishCharge()
    {
        base.FinishCharge();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        if(isChargeAnimationFinished)
            stateMachine.ChangeState(enemy.idleState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
