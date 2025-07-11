using UnityEngine;

public class Wolf_IdleState : IdleState
{
    private Wolf enemy;

    public Wolf_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Wolf enemy) : base(entity, stateMachine, animBoolName, stateData)
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
            stateMachine.ChangeState(enemy.moveState);
        else if(isDetectedPlayer)
            stateMachine.ChangeState(enemy.detectState);
        else if(isPlayerInMeleeAttackRange)
            stateMachine.ChangeState(enemy.meleeAttackState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
