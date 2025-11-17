using UnityEngine;

public class TwistedCultist_IdleState : IdleState
{
    private TwistedCultist enemy;
    public TwistedCultist_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, TwistedCultist enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (isIdleTimeOver)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
        else if (isPlayerInMeleeAttackRange && enemy.entityData.AttackCoolTime + enemy.LastAttackTime <= Time.time)
        {
            stateMachine.ChangeState(enemy.meleeAttackState);
        }
        else if (isDetectedPlayer)
        {
            stateMachine.ChangeState(enemy.detectState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
