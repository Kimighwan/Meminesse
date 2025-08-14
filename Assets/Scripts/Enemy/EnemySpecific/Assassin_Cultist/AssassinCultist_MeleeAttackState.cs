using UnityEngine;

public class AssassinCultist_MeleeAttackState : MeleeAttackState
{
    private AssassinCultist enemy;
    public AssassinCultist_MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttackState stateData, AssassinCultist enemy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
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

        if(enemy.firstAttack)
            enemy.firstAttack = false;
        enemy.SetVelocityX(12);
        enemy.isAttackFinish = false;
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
            enemy.isAttackFinish = true;
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

        enemy.SetVelocityX(0);
    }
}
