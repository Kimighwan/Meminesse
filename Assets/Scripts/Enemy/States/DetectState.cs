using UnityEngine;

public class DetectState : State
{
    protected D_DetectState stateData;

    protected bool isPlayerInMeleeAttackRange;
    protected bool isPlayerInRangeAttackRange;
    protected bool isPlayerInChargeRange;
    protected bool isDetectedPlayer;
    protected bool isDetectLedge;

    public DetectState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DetectState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isDetectedPlayer = entity.CheckPlayerInDetectRangeTpyeLine();
        isPlayerInMeleeAttackRange = entity.CheckPlayerInMeleeAttackRange();
        isPlayerInRangeAttackRange = entity.CheckPlayerInRangeAttackRange();
        isDetectLedge = entity.CheckLedge();
        isPlayerInChargeRange = entity.CheckPlayerInChargeRange();
    }

    public override void Enter()
    {
        base.Enter();

        entity.SetVelocityX(stateData.detectSpeed);
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
