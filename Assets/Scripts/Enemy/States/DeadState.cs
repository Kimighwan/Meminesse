using UnityEngine;

public class DeadState : State
{
    protected D_DeadState stateData;

    public DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void Enter()
    {
        base.Enter();

        entity.SetVelocity(0f);
        // 죽을 때 생성될 파티클이 있다면 인스턴스화 하기
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
