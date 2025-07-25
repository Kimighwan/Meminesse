using UnityEngine;

public class Wolf_MeleeAttackState : MeleeAttackState
{
    private Wolf enemy;

    public Wolf_MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttackState stateData, Wolf enemy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
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
        enemy.LastAttackTime = startTIme;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        if (isAttackAnimationFinished)
        {
            //if(isPlayerInMeleeAttackRange)
            //    stateMachine.ChangeState(this);
            //else
            enemy.idleState.SetFlipAfterIdle(false);
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
    }
}
