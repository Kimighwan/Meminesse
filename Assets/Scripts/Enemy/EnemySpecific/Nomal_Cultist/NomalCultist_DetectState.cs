using UnityEngine;

public class NomalCultist_DetectState : DetectState
{
    private NomalCultist enemy;
    public NomalCultist_DetectState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DetectState stateData, NomalCultist enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (!isDetectedPlayer)
            stateMachine.ChangeState(enemy.idleState);
        else if (isPlayerInRangeAttackRange && enemy.AttackCoolTime + enemy.LastAttackTime <= Time.time)
            stateMachine.ChangeState(enemy.rangeAttackState);
        else if (!isDetectLedge)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
