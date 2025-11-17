using UnityEngine;

public class Wolf_ChargeState : ChargeState
{
    private Wolf enemy;

    public Wolf_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData, Wolf enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        enemy.LastChargeTime = startTIme;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishCharge()
    {
        base.FinishCharge();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        if (isDetectWall || !isDetectLedge || isChargeTimeOver)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
        else if (isPlayerInMeleeAttackRange && enemy.LastAttackTime + enemy.entityData.AttackCoolTime <= Time.time)
            stateMachine.ChangeState(enemy.meleeAttackState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
