using UnityEngine;

public class StunState : State
{
    protected D_StunState stateData;

    protected bool isStunTimeOver;              // 스턴 지속 시간 오버 했는가?
    protected bool isGround;                    // 땅에 접촉되었는가?
    protected bool isMoveStopped;               // 움직임이 멈췄는가? if문 한 번 실행하기 위함
    protected bool isPlayerInMeleeAttackRange;  // 플레이어가 근접 공격 범위에 있는가?
    protected bool isDetectedPlayer;            // 플레이어가 추적 거리에서 탐지되는가?

    public StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isGround = entity.CheckGround();
        isPlayerInMeleeAttackRange = entity.CheckPlayerInMeleeAttackRange();
        isDetectedPlayer = entity.CheckPlayerDectedRange();
    }

    public override void Enter()
    {
        base.Enter();

        isStunTimeOver = false;
        isMoveStopped = false;
        entity.Knockback(stateData.stunKnocbackSpeed, stateData.stunKnocbackAngle, entity.LastDamagedDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        if(Time.time >= startTIme + stateData.stunTime)
        {
            isStunTimeOver = true;
        }

        // 지면에 닫고 && 넉백되는 시간이 완료시
        // 넉백시 설정되는 속도를 제거하는 용도
        if(isGround && Time.time >= startTIme + stateData.stunKnocbackTime && !isMoveStopped)
        {
            isMoveStopped = true;   // 해당 if문 한 번만 실행
            entity.SetVelocity(0f);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
