using UnityEngine;

public class FireWizard_DeadState : DeadState
{
    private FireWizard enemy;

    public FireWizard_DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData, FireWizard enemy) : base(entity, stateMachine, animBoolName, stateData)
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
