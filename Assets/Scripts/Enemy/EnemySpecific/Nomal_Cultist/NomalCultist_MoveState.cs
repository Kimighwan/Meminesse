using UnityEngine;

public class NomalCultist_MoveState : MoveState
{
    private NomalCultist enemy;
    public NomalCultist_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, NomalCultist enemy) : base(entity, stateMachine, animBoolName, stateData)
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
            enemy.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemy.idleState);
        }
        else if (isPlayerInRangeAttackRange && enemy.AttackCoolTime + enemy.LastAttackTime <= Time.time)
            stateMachine.ChangeState(enemy.rangeAttackState);
        else if (isDetectedPlayer)
            stateMachine.ChangeState(enemy.detectState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
