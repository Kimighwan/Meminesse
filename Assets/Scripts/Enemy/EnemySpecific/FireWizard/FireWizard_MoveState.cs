using UnityEngine;

public class FireWizard_MoveState : MoveState
{
    private FireWizard enemy;

    public FireWizard_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, FireWizard enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        if(isDetectWall || !isDetectLedge)
        {
            enemy.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemy.idleState);
        }
        else if (isPlayerInMeleeAttackRange && enemy.LastAttackTime + enemy.entityData.AttackCoolTime <= Time.time)
            stateMachine.ChangeState(enemy.meleeAttackState);
        else if (isDetectedPlayer)
            stateMachine.ChangeState(enemy.detectState);
        else if (isDetectedPlayerReverse)
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
