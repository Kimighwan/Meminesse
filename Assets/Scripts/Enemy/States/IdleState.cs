using UnityEngine;

public class IdleState : State
{
    protected D_IdleState stateData;

    protected bool flipAfterIdle;                   // Idle 상태 종료후 Flip을 실행할 것인가?
    protected bool isIdleTimeOver;                  // Idle 상태 지속 시간을 초과 했는가?
    protected bool isDetectedPlayer;                // 플레이어를 탐지했는가?
    protected bool isPlayerInMeleeAttackRange;      // 플레이어 근접 공격 범위에 들어왔는가?
    protected bool isPlayerInRangeAttackRange;      
    protected float idleTime;                       // Idle 상태 지속 시간

    public IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isPlayerInMeleeAttackRange = entity.CheckPlayerInMeleeAttackRange();
        isPlayerInRangeAttackRange = entity.CheckPlayerInRangeAttackRange();
        isDetectedPlayer = entity.CheckPlayerInDetectRangeTpyeLine();
    }

    public override void Enter()
    {
        base.Enter();

        entity.SetVelocityX(0f);
        isIdleTimeOver = false;
        isDetectedPlayer = false;
        SetRandomIdleTIme();
    }

    public override void Exit()
    {
        base.Exit();

        if (flipAfterIdle)
            entity.Flip();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        if (Time.time > startTIme + idleTime)
            isIdleTimeOver = true;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void SetFlipAfterIdle(bool flip)     // 상태 종료후 Flip 설정
    {
        flipAfterIdle = flip;
    }

    private void SetRandomIdleTIme()        // 랜덤 Idle 시간 정하기
    {
        idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);
    }
}
