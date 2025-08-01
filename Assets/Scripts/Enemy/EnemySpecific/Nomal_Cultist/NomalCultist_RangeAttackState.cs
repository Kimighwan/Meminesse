using UnityEngine;

public class NomalCultist_RangeAttackState : RangeAttackState
{
    private NomalCultist enemy;
    public NomalCultist_RangeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_RangeAttackState stateData, NomalCultist enemy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
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
