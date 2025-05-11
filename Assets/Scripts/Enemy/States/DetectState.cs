using UnityEngine;

public class DetectState : State
{
    protected D_DetectState stateData;

    protected bool isPlayerInMeleeAttackRange;
    protected bool isDetectedPlayer;


    public DetectState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DetectState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isDetectedPlayer = entity.CheckPlayerDectedRange();
        isPlayerInMeleeAttackRange = entity.CheckPlayerInMeleeAttackRange();
    }

    public override void Enter()
    {
        base.Enter();

        entity.SetVelocity(stateData.detectSpeed);
        isPlayerInMeleeAttackRange = false;
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
