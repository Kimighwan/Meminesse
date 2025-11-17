using UnityEngine;

public class EvilWizard_IdleState : IdleState
{
    private EvilWizard enemy;

    public EvilWizard_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, EvilWizard enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        else if (isPlayerInMeleeAttackRange && enemy.LastAttackTime + enemy.entityData.AttackCoolTime <= Time.time)
        {
            int randomValue = RandomAttack();

            if(randomValue == 0)
                stateMachine.ChangeState(enemy.meleeAttack1State);
            else 
                stateMachine.ChangeState(enemy.meleeAttack2State);
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

    private int RandomAttack()
    {
        return Random.Range(0, 2);
    }
}
