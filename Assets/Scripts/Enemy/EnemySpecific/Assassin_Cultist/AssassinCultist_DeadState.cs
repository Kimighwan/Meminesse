using UnityEngine;

public class AssassinCultist_DeadState : DeadState
{
    private AssassinCultist enemy;
    public AssassinCultist_DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData, AssassinCultist enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishDeadAnimation()
    {
        base.FinishDeadAnimation();
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
