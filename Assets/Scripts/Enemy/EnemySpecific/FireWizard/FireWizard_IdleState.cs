using Mono.Cecil.Cil;
using UnityEngine;

public class FireWizard_IdleState : IdleState
{
    private FireWizard enemy;
    public FireWizard_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, FireWizard enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        if(isIdleTimeOver)
            stateMachine.ChangeState(enemy.moveState);
        else if(isPlayerInMeleeAttackRange && enemy.LastAttackTime + enemy.entityData.AttackCoolTime <= Time.time)
            stateMachine.ChangeState(enemy.meleeAttackState);
        else if(isDetectedPlayer)
            stateMachine.ChangeState(enemy.detectState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
