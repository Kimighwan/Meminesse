using UnityEngine;

public class Bat_IdleState : IdleState
{
    private Bat enemy;

    private bool isStartCountDown;

    private float startCountDownTime;

    public Bat_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Bat enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isDetectedPlayer = entity.CheckPlayerInDetectRangeTpyeCircle();
    }

    public override void Enter()
    {
        base.Enter();

        entity.SetVelocityY(0);
        isStartCountDown = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        entity.CheckXPositionForFlip();



        if (isStartCountDown && Time.time >= startCountDownTime + 2f)
            stateMachine.ChangeState(enemy.sleepState);

        if (!isPlayerInRangeAttackRange)
        {
            if(!isStartCountDown)
            {
                isStartCountDown = true;
                startCountDownTime = Time.time;
            }
        }
        else if (isDetectedPlayer)
            stateMachine.ChangeState(enemy.detectState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
