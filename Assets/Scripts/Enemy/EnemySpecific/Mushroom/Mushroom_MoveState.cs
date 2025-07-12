using UnityEngine;

public class Mushroom_MoveState : MoveState
{
    private Mushroom enemy;

    public Mushroom_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, Mushroom enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (isDetectWall || !isDetectLedge)
        {
            // 벽과 낭떨어지 감지하면 Idle 상태로 전환
            // 벽과 낭떨어지 이므로 방향 전환 해주기
            enemy.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemy.idleState);
        }
        else if (isPlayerInMeleeAttackRange)
            stateMachine.ChangeState(enemy.meleeAttackState);
        //else if (isDetectedPlayer)
        //    stateMachine.ChangeState(enemy.detectState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
