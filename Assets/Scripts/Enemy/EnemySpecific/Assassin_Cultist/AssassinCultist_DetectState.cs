using UnityEngine;

public class AssassinCultist_DetectState : DetectState
{
    private AssassinCultist enemy;
    public AssassinCultist_DetectState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DetectState stateData, AssassinCultist enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        else if (isPlayerInMeleeAttackRange && (enemy.firstAttack || enemy.LastAttackTime + enemy.entityData.AttackCoolTime <= Time.time))
            stateMachine.ChangeState(enemy.meleeAttackState);
        else
            stateMachine.ChangeState(enemy.moveState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
