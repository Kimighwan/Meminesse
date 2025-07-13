using UnityEngine;

public class EvilWizard_DeadState : DeadState
{
    private EvilWizard enemy;

    public EvilWizard_DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData, EvilWizard enemy) : base(entity, stateMachine, animBoolName, stateData)
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
