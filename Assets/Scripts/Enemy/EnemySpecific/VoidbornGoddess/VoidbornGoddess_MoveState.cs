using UnityEngine;

public class VoidbornGoddess_MoveState : MoveState
{
    private VoidbornGoddess enemy;
    public VoidbornGoddess_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, VoidbornGoddess enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        else if (isPlayerInRangeAttackRange)
            stateMachine.ChangeState(enemy.cast1State);
        //else if (isPlayerInMeleeAttackRange && enemy.LastAttackTime + enemy.AttackCoolTime <= Time.time)
        //    stateMachine.ChangeState(enemy.meleeAttackState);
        //else if (isDetectedPlayer)
        //    stateMachine.ChangeState(enemy.detectState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
