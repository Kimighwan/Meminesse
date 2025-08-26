using UnityEngine;

public class VoidbornGoddess_DeadState : DeadState
{
    private VoidbornGoddess deadState;
    public VoidbornGoddess_DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData, VoidbornGoddess deadState) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.deadState = deadState;
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

        if(isDoneAnimation)
            GameObject.Destroy(entity.gameObject);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
