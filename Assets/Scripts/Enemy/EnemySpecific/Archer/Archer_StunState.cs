using UnityEngine;

public class Archer_StunState : StunState
{
    private Archer enemy;

    public Archer_StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData, Archer enemy) : base(entity, stateMachine, animBoolName, stateData)
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
                stateMachine.ChangeState(enemy.detectState);
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
