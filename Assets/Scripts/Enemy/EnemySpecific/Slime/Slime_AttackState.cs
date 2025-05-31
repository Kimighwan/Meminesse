using UnityEngine;

public class Slime_AttackState : MeleeAttackState
{
    private Slime enemy;

    public Slime_AttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttackState stateData, Slime enemy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
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

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (isAttackAnimationFinished)
        {
            if(isPlayerInMeleeAttackRange)
            {
                stateMachine.ChangeState(enemy.attackState);
            }
            else
                stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
    }
}
