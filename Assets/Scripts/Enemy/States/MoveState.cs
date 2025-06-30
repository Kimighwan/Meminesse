using UnityEngine;

public class MoveState : State
{
    protected D_MoveState stateData;

    protected bool isDetectWall;                // 벽을 감지했는가?
    protected bool isDetectLedge;               // Platform을 감지하고 있는가? 아니라면 빈 공간,즉 낭떨어지 
    protected bool isPlayerInMeleeAttackRange;  // 플레이어가 근접 공격 거리에 있는가?
    protected bool isPlayerInRangeAttackRange;
    protected bool isDetectedPlayer;            // 플레이어를 감지했는가
    protected bool isPlayerInChargeRange;

    public MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isDetectWall = entity.CheckWall();
        isDetectLedge = entity.CheckLedge();
        isPlayerInMeleeAttackRange = entity.CheckPlayerInMeleeAttackRange();
        isPlayerInRangeAttackRange = entity.CheckPlayerInRangeAttackRange();
        isDetectedPlayer = entity.CheckPlayerInDetectRangeTpyeLine();
        isPlayerInChargeRange = entity.CheckPlayerInChargeRange();
    }

    public override void Enter()
    {
        base.Enter();

        entity.SetVelocityX(stateData.moveSpeed);
        isDetectedPlayer = false;
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
