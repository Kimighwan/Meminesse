using UnityEngine;

public class TestEnemy_MoveState : MoveState
{
    private TestEnemy enemy;

    public TestEnemy_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, TestEnemy enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        if (isDetectWall || isDetectLedge)
        {
            // 벽과 낭떨어지 감지하면 Idle 상태로 전환
            // 벽과 낭떨어지 이므로 방향 전환 해주기
            enemy.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemy.idleState);
        }
        else if(isPlayerInMinDetectedRange)
            stateMachine.ChangeState(enemy.chargeState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
