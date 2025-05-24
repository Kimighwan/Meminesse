using UnityEngine;

public class DodgeState : State
{
    protected D_DodgeState stateData;

    protected bool isPlayerInRangeAttackRange;
    protected bool isDetectedPlayer;
    protected bool isDetectWall;                // 벽을 감지했는가?
    protected bool isDetectLedge;               // Platform을 감지하고 있는가? 아니라면 빈 공간,즉 낭떨어지
    protected bool isDodgeTimeOver;
    protected bool isGround;

    public DodgeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DodgeState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isPlayerInRangeAttackRange = entity.CheckPlayerInRangeAttackRange();
        isDetectedPlayer = entity.CheckPlayerDectedRange();
        isDetectWall = entity.CheckWall();
        isDetectLedge = entity.CheckLedge();
        isGround = entity.CheckGround();
    }

    public override void Enter()
    {
        base.Enter();

        isDodgeTimeOver = false;
        entity.SetVelocity(stateData.dodgeSpeed, stateData.dodgeAngle, -entity.facingDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        if (Time.time >= startTIme + stateData.dodgeDuration && isGround)
        {
            isDodgeTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
