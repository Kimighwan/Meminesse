using UnityEngine;

public class TestEnemy_DetectState : DetectState
{
    private TestEnemy enemy;
    public TestEnemy_DetectState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DetectState stateData, TestEnemy enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        else if(isPlayerInMeleeAttackRange)
            stateMachine.ChangeState(enemy.meleeAttackState);
        else if(isDetectLedge)
        {
            enemy.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
