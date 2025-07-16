using UnityEngine;

public class Wolf_MoveState : MoveState
{
    private Wolf enemy;

    public Wolf_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, Wolf enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        else if (isPlayerInMeleeAttackRange && enemy.LastAttackTime + enemy.AttackCoolTime <= Time.time)
            stateMachine.ChangeState(enemy.meleeAttackState);
        else if (isDetectedPlayer)
            stateMachine.ChangeState(enemy.detectState);
        else if (isPlayerInChargeRange && enemy.LastChargeTime + enemy.ChargeCoolTime <= Time.time)
            stateMachine.ChangeState(enemy.chargeState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
