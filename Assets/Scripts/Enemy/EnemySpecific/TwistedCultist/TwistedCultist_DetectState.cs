using UnityEngine;

public class TwistedCultist_DetectState : DetectState
{
    private TwistedCultist enemy;
    public TwistedCultist_DetectState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DetectState stateData, TwistedCultist enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        else if (isPlayerInMeleeAttackRange && enemy.entityData.AttackCoolTime + enemy.LastAttackTime <= Time.time)
            stateMachine.ChangeState(enemy.meleeAttackState);
        else if (!isDetectLedge)
            stateMachine.ChangeState(enemy.moveState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
