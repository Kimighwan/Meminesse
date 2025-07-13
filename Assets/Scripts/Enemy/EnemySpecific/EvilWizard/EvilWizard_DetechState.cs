using UnityEngine;

public class EvilWizard_DetechState : DetectState
{
    private EvilWizard enemy;

    public EvilWizard_DetechState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DetectState stateData, EvilWizard enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        else if (isPlayerInMeleeAttackRange)
        {
            int randomValue = RandomAttack();

            if (randomValue == 0)
                stateMachine.ChangeState(enemy.meleeAttack1State);
            else
                stateMachine.ChangeState(enemy.meleeAttack2State);
        }
        else if (isDetectLedge)
        {
            stateMachine.ChangeState(enemy.moveState);
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
