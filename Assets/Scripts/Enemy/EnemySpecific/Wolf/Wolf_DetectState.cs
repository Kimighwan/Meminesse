using UnityEngine;

public class Wolf_DetectState : DetectState
{
    private Wolf enemy;

    public Wolf_DetectState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DetectState stateData, Wolf enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (isPlayerInMeleeAttackRange && enemy.LastAttackTime + enemy.AttackCoolTime <= Time.time)
            stateMachine.ChangeState(enemy.meleeAttackState);
        else if(!isDetectedPlayer)
            stateMachine.ChangeState(enemy.idleState);
        else if(!isDetectLedge)
        {
            enemy.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemy.idleState);
        }
        else if (isPlayerInChargeRange && enemy.LastChargeTime + enemy.ChargeCoolTime <= Time.time)
            stateMachine.ChangeState(enemy.chargeState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
