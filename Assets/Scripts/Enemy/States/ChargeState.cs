using UnityEngine;

public class ChargeState : State
{
    protected D_ChargeState stateData;

    protected bool isDetectWall;        // 벽을 감지했는가?
    protected bool isDetectLedge;       // 절벽을 감지했는가?
    protected bool isChargeTimeOver;    // 돌진 시간을 초과했는가?

    public ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isDetectWall = entity.CheckWall();
        isDetectLedge = entity.CheckLedge();
    }

    public override void Enter()
    {
        base.Enter();

        entity.SetVelocity(stateData.chargeSpeed);
        isChargeTimeOver = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        if (Time.time >= startTIme + stateData.chargeTime)
            isChargeTimeOver = true;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
