using UnityEngine;

public class Bat_StunState : StunState
{
    private Bat enemy;
    public Bat_StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData, Bat enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (isStunTimeOver)
        {
            if (isDetectedPlayer)
            {
                stateMachine.ChangeState(enemy.moveState);
            }
            else
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
