using UnityEngine;

public class FireWizard_DetectState : DetectState
{
    private FireWizard enemy;

    public FireWizard_DetectState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DetectState stateData, FireWizard enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        else if (isPlayerInMeleeAttackRange && enemy.LastAttackTime + enemy.AttackCoolTime <= Time.time)
            stateMachine.ChangeState(enemy.meleeAttackState);
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
