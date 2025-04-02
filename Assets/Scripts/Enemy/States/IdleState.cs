using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    protected D_IdelState stateData;

    protected bool flipAfterIdle;               // Idle 상태 종료 후 Flip을 실행할 것이냐?
    protected bool isIdleTimeOver;              // Idle 상태 지속 시간을 초과 했는가?
    protected bool isPlayerInMinDetectedRange;  //

    protected float idleTime;                   // idle 상태 지속 시간

    public IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdelState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isPlayerInMinDetectedRange = entity.CheckPlayerInMinRange();
    }

    public override void Enter()
    {
        base.Enter();

        entity.SetVelocity(0f);
        isIdleTimeOver = false;
        SetRandomIdleTime();
    }

    public override void Exit()
    {
        base.Exit();

        if (flipAfterIdle)
        {
            entity.Flip();
        }
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        if(Time.time >= startTIme + idleTime)
        {
            isIdleTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void SetFlipAfterIdle(bool flip) // 상태 종료 후 flip 설정
    {
        flipAfterIdle = flip;
    }

    private void SetRandomIdleTime()        // 랜덤 idle 시간 정하기
    {
        idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);
    }
}
