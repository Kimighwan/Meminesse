using UnityEngine;

public class VoidbornGoddess_IdleState : IdleState
{
    private VoidbornGoddess enemy;
    public VoidbornGoddess_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, VoidbornGoddess enemy) : base(entity, stateMachine, animBoolName, stateData)
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

    public override void FinishTransition()
    {
        base.FinishTransition();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        if (isIdleTimeOver)
        {
            int randomValue = Random.Range(1, 11);
            if (randomValue < 7) stateMachine.ChangeState(enemy.rangeAttackState);
            else stateMachine.ChangeState(enemy.chargeState);
        }
        else if(isPlayerInMeleeAttackRange && enemy.LastAttackTime + enemy.entityData.AttackCoolTime <= Time.time)
            stateMachine.ChangeState(enemy.meleeAttackState);
        else if(enemy.firstHandPatternStart)
        {
            enemy.firstHandPatternStart = false;
            stateMachine.ChangeState(enemy.cast2State);
        }
        else if(enemy.secondHandPatternStart)
        {
            enemy.secondHandPatternStart = false;
            stateMachine.ChangeState(enemy.cast2State);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
