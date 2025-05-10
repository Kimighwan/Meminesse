using UnityEngine;

public class MoveState : State
{
    protected D_MoveState stateData;

    protected bool isDetectWall;        // 벽을 감지했는가?
    protected bool isDetectLedge;     // Platform을 감지하고 있는가? 아니라면 빈 공간,즉 낭떨어지 
    protected bool isPlayerInMeleeAttackRange;

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
    }

    public override void Enter()
    {
        base.Enter();
        entity.SetVelocity(stateData.moveSpeed);
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
